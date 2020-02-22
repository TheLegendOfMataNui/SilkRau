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
    internal static class SLBToYamlConverter
    {
        public delegate TResult ReadBinaryFromFile<TResult>(string filePath, Func<IBinaryReader, TResult> function);

        public delegate void WriteTextToFile(string filePath, string contents);
    }

    internal sealed class SLBToYamlConverter<T> : IFileConverter
    {
        private readonly IBinarySerializer<T> slbSerializer;

        private readonly ISerializer yamlSerializer;

        private readonly SLBToYamlConverter.ReadBinaryFromFile<T> readBinaryFromFile;

        private readonly SLBToYamlConverter.WriteTextToFile writeTextToFile;

        public SLBToYamlConverter(
            IBinarySerializer<T> slbSerializer,
            ISerializer yamlSerializer,
            SLBToYamlConverter.ReadBinaryFromFile<T> readBinaryFromFile,
            SLBToYamlConverter.WriteTextToFile writeTextToFile
        ) {
            this.slbSerializer = slbSerializer;
            this.yamlSerializer = yamlSerializer;
            this.readBinaryFromFile = readBinaryFromFile;
            this.writeTextToFile = writeTextToFile;
        }

        public void Convert(string inputFilePath, string outputFilePath)
        {
            T value = readBinaryFromFile(
                filePath: inputFilePath,
                function: binaryReader => slbSerializer.Read(binaryReader)
            );

            string yaml = yamlSerializer.Serialize(value);

            writeTextToFile(filePath: outputFilePath, contents: yaml);
        }
    }
}
