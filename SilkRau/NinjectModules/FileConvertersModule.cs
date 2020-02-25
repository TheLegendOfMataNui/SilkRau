/*
* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/
using Ninject.Modules;
using NUtils.Extensions;
using SAGESharp.IO.Binary;
using SAGESharp.IO.Yaml;
using SilkRau.FileConverters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using ProvidersDictionary = System.Collections.Generic.IReadOnlyDictionary<SilkRau.FileConversion, SilkRau.FileConverterProvider>;

namespace SilkRau.NinjectModules
{
    internal sealed class FileConvertersModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ProvidersDictionary>().ToConstant(BuildProvidersDictionary());
        }

        private static ProvidersDictionary BuildProvidersDictionary() => new Dictionary<FileConversion, FileConverterProvider>
        {
            [new FileConversion(inputFileFormat: FileFormat.SLB, outputFileFormat: FileFormat.Yaml)] = BuildSLBToYamlConverter,
            [new FileConversion(inputFileFormat: FileFormat.Yaml, outputFileFormat: FileFormat.SLB)] = BuildYamlToSLBConverter
        };

        private static IFileConverter BuildSLBToYamlConverter(Type fileType)
        {
            return (IFileConverter)typeof(FileConverterFactory)
                .GetMethod(nameof(BuildTypedSLBToYamlConverter), BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(fileType)
                .Invoke(null, Array.Empty<object>());
        }

        private static SLBToYamlConverter<T> BuildTypedSLBToYamlConverter<T>()
            => new SLBToYamlConverter<T>(
                slbSerializer: BinarySerializer.ForType<T>(),
                yamlSerializer: YamlSerializer.BuildSLBSerializer(),
                readBinaryFromFile: ReadBinaryFromFile,
                writeTextToFile: WriteTextToFile
            );

        private static T ReadBinaryFromFile<T>(string filePath, Func<IBinaryReader, T> function)
        {
            using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return Reader.ForStream(stream).Let(function);
            }
        }

        private static void WriteTextToFile(string filePath, string contents)
        {
            using (Stream stream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write))
            {
                using (TextWriter textWriter = new StreamWriter(stream))
                {
                    textWriter.Write(contents);
                }
            }
        }

        public static IFileConverter BuildYamlToSLBConverter(Type fileType)
        {
            return (IFileConverter)typeof(FileConverterFactory)
                .GetMethod(nameof(BuildTypedYamlToSLBConverter), BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(fileType)
                .Invoke(null, Array.Empty<object>());
        }

        private static YamlToSLBConverter<T> BuildTypedYamlToSLBConverter<T>()
            => new YamlToSLBConverter<T>(
                yamlDeserializer: YamlDeserializer.BuildSLBDeserializer(),
                slbSerializer: BinarySerializer.ForType<T>(),
                readTextFromFile: ReadTextFromFile,
                writeBinaryToFile: WriteBinaryToFile
            );

        private static string ReadTextFromFile(string filePath)
        {
            using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (TextReader textReader = new StreamReader(stream))
                {
                    return textReader.ReadToEnd();
                }
            }
        }

        private static void WriteBinaryToFile(string filePath, Action<IBinaryWriter> action)
        {
            using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Write))
            {
                action(Writer.ForStream(stream));
            }
        }
    }
}
