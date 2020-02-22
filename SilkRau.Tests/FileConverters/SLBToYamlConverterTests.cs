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
    class SLBToYamlConverterTests
    {
        private readonly IBinarySerializer<object> slbSerializer
            = Substitute.For<IBinarySerializer<object>>();

        private readonly ISerializer yamlSerializer = Substitute.For<ISerializer>();

        private readonly SLBToYamlConverter.ReadBinaryFromFile<object> readBinaryFromFile
            = Substitute.For<SLBToYamlConverter.ReadBinaryFromFile<object>>();

        private readonly SLBToYamlConverter.WriteTextToFile writeTextToFile
            = Substitute.For<SLBToYamlConverter.WriteTextToFile>();

        private readonly IFileConverter fileConverter;

        public SLBToYamlConverterTests()
        {
            fileConverter = new SLBToYamlConverter<object>(
                slbSerializer,
                yamlSerializer,
                readBinaryFromFile,
                writeTextToFile
            );
        }

        [Test]
        public void Test_Converting_A_File_To_Another()
        {
            IBinaryReader binaryReader = Substitute.For<IBinaryReader>();
            object value = new object();
            string inputFileName = nameof(inputFileName);
            string outputFileName = nameof(outputFileName);
            string contents = nameof(contents);

            readBinaryFromFile.Invoke(
                filePath: Arg.Is(inputFileName),
                function: Arg.Any<Func<IBinaryReader, object>>()
            ).Returns(callInfo => callInfo.Arg<Func<IBinaryReader, object>>().Invoke(binaryReader));
            slbSerializer.Read(binaryReader).Returns(value);
            yamlSerializer.Serialize(value).Returns(contents);

            fileConverter.Convert(inputFilePath: inputFileName, outputFilePath: outputFileName);

            writeTextToFile.Received().Invoke(filePath: outputFileName, contents: contents);
        }
    }
}
