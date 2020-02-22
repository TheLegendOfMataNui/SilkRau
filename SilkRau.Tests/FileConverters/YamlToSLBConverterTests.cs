/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using NSubstitute;
using NUnit.Framework;
using SAGESharp.IO.Binary;
using SilkRau.FileConverters;
using System;
using YamlDotNet.Serialization;

namespace SilkRau.Tests.FileConverters
{
    class YamlToSLBConverterTests
    {
        private readonly IDeserializer yamlDeserializer
            = Substitute.For<IDeserializer>();

        private readonly IBinarySerializer<object> slbSerializer
            = Substitute.For<IBinarySerializer<object>>();

        private readonly YamlToSLBConverter.ReadTextFromFile readTextFromFile
            = Substitute.For<YamlToSLBConverter.ReadTextFromFile>();

        private readonly YamlToSLBConverter.WriteBinaryToFile writeBinaryToFile
            = Substitute.For<YamlToSLBConverter.WriteBinaryToFile>();

        private readonly IFileConverter fileConverter;

        public YamlToSLBConverterTests()
        {
            fileConverter = new YamlToSLBConverter<object>(
                yamlDeserializer,
                slbSerializer,
                readTextFromFile,
                writeBinaryToFile
            );
        }

        [Test]
        public void Test_Converting_A_File_To_Another()
        {
            IBinaryWriter binaryWriter = Substitute.For<IBinaryWriter>();
            object value = new object();
            string inputFileName = nameof(inputFileName);
            string outputFileName = nameof(outputFileName);
            string contents = nameof(contents);

            readTextFromFile.Invoke(inputFileName).Returns(contents);
            yamlDeserializer.Deserialize<object>(contents).Returns(value);
            writeBinaryToFile.Invoke(
                fileName: Arg.Is(outputFileName),
                action: Arg.Do<Action<IBinaryWriter>>(action => action(binaryWriter))
            );

            fileConverter.Convert(inputFilePath: inputFileName, outputFilePath: outputFileName);

            slbSerializer.Received().Write(binaryWriter, value);
        }
    }
}
