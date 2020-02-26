/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using SAGESharp.IO.Binary;
using System;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace SilkRau.FileConverters
{
    internal sealed class YamlToSLBConverter<T> : IFileConverter
    {
        private readonly IDeserializer yamlDeserializer;

        private readonly IBinarySerializer<T> slbSerializer;

        private readonly IIO io;

        public YamlToSLBConverter(
            IDeserializer yamlDeserializer,
            IBinarySerializer<T> slbSerializer,
            IIO io
        ) {
            this.yamlDeserializer = yamlDeserializer;
            this.slbSerializer = slbSerializer;
            this.io = io;
        }

        public void Convert(string inputFilePath, string outputFilePath)
        {
            string contents = io.ReadTextFromFile(inputFilePath);

            T value;
            try
            {
                value = yamlDeserializer.Deserialize<T>(contents);
            }
            catch (YamlException exception)
            {
                throw new BadFormatException($"{inputFilePath} has an invalid format", exception);
            }

            io.WriteBinaryToFile(
                fileName: outputFilePath,
                action: binaryWriter => slbSerializer.Write(binaryWriter, value)
            );
        }

        public interface IIO
        {
            string ReadTextFromFile(string filePath);

            void WriteBinaryToFile(string fileName, Action<IBinaryWriter> action);
        }

        internal sealed class IO : IIO
        {
            public string ReadTextFromFile(string filePath)
            {
                using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (TextReader textReader = new StreamReader(stream))
                    {
                        return textReader.ReadToEnd();
                    }
                }
            }

            public void WriteBinaryToFile(string filePath, Action<IBinaryWriter> action)
            {
                using (Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    action(Writer.ForStream(stream));
                }
            }
        }
    }
}
