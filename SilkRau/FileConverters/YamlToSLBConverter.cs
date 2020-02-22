/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using SAGESharp.IO.Binary;
using System;
using YamlDotNet.Serialization;

namespace SilkRau.FileConverters
{
    internal static class YamlToSLBConverter
    {
        public delegate string ReadTextFromFile(string filePath);

        public delegate void WriteBinaryToFile(string fileName, Action<IBinaryWriter> action);
    }

    internal sealed class YamlToSLBConverter<T> : IFileConverter
    {
        private readonly IDeserializer yamlDeserializer;

        private readonly IBinarySerializer<T> slbSerializer;

        private readonly YamlToSLBConverter.ReadTextFromFile readTextFromFile;

        private readonly YamlToSLBConverter.WriteBinaryToFile writeBinaryToFile;

        public YamlToSLBConverter(
            IDeserializer yamlDeserializer,
            IBinarySerializer<T> slbSerializer,
            YamlToSLBConverter.ReadTextFromFile readTextFromFile,
            YamlToSLBConverter.WriteBinaryToFile writeBinaryToFile
        ) {
            this.yamlDeserializer = yamlDeserializer;
            this.slbSerializer = slbSerializer;
            this.readTextFromFile = readTextFromFile;
            this.writeBinaryToFile = writeBinaryToFile;
        }

        public void Convert(string inputFilePath, string outputFilePath)
        {
            string contents = readTextFromFile(inputFilePath);

            T value = yamlDeserializer.Deserialize<T>(contents);

            writeBinaryToFile(
                fileName: outputFilePath,
                action: binaryWriter => slbSerializer.Write(binaryWriter, value)
            );
        }
    }
}
