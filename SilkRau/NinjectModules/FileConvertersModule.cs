/*
* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/
using Ninject.Modules;
using SAGESharp.IO.Binary;
using SAGESharp.IO.Yaml;
using SilkRau.FileConverters;
using System;
using System.Collections.Generic;
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
                io: new SLBToYamlConverter<T>.IO()
            );

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
                io: new YamlToSLBConverter<T>.IO()
            );
    }
}
