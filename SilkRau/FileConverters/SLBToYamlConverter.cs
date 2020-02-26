/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using NUtils.Extensions;
using SAGESharp.IO.Binary;
using System;
using System.IO;
using YamlDotNet.Serialization;

namespace SilkRau.FileConverters
{
    internal sealed class SLBToYamlConverter<T> : IFileConverter
    {
        private readonly IBinarySerializer<T> slbSerializer;

        private readonly ISerializer yamlSerializer;

        private readonly IIO io;

        public SLBToYamlConverter(
            IBinarySerializer<T> slbSerializer,
            ISerializer yamlSerializer,
            IIO io
        ) {
            this.slbSerializer = slbSerializer;
            this.yamlSerializer = yamlSerializer;
            this.io = io;
        }

        public void Convert(string inputFilePath, string outputFilePath)
        {
            T value;
            try
            {
                value = io.ReadBinaryFromFile(
                    filePath: inputFilePath,
                    function: binaryReader => slbSerializer.Read(binaryReader)
                );
            }
            catch (EndOfStreamException exception)
            {
                throw new BadFormatException($"{inputFilePath} has an invalid format", exception);
            }

            string yaml = yamlSerializer.Serialize(value);

            io.WriteTextToFile(filePath: outputFilePath, contents: yaml);
        }

        public interface IIO
        {
            T ReadBinaryFromFile(string filePath, Func<IBinaryReader, T> function);

            void WriteTextToFile(string filePath, string contents);
        }

        internal sealed class IO : IIO
        {
            public T ReadBinaryFromFile(string filePath, Func<IBinaryReader, T> function)
            {
                using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    return Reader.ForStream(stream).Let(function);
                }
            }

            public void WriteTextToFile(string filePath, string contents)
            {
                using (Stream stream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write))
                {
                    using (TextWriter textWriter = new StreamWriter(stream))
                    {
                        textWriter.Write(contents);
                    }
                }
            }
        }
    }
}
