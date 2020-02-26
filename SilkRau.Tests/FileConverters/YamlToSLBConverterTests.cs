/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using FluentAssertions;
using NSubstitute;
using NSubstitute.ClearExtensions;
using NUnit.Framework;
using SAGESharp.IO.Binary;
using SilkRau.FileConverters;
using System;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace SilkRau.Tests.FileConverters
{
    class YamlToSLBConverterTests
    {
        private readonly IDeserializer yamlDeserializer
            = Substitute.For<IDeserializer>();

        private readonly IBinarySerializer<object> slbSerializer
            = Substitute.For<IBinarySerializer<object>>();

        private readonly YamlToSLBConverter<object>.IIO io
            = Substitute.For<YamlToSLBConverter<object>.IIO>();

        private readonly IFileConverter fileConverter;

        public YamlToSLBConverterTests()
        {
            fileConverter = new YamlToSLBConverter<object>(
                yamlDeserializer,
                slbSerializer,
                io
            );
        }

        [TearDown]
        public void TearDown()
        {
            yamlDeserializer.ClearSubstitute();
            slbSerializer.ClearSubstitute();
            io.ClearSubstitute();
        }

        [Test]
        public void Test_Converting_A_File_To_Another()
        {
            IBinaryWriter binaryWriter = Substitute.For<IBinaryWriter>();
            object value = new object();
            string inputFileName = nameof(inputFileName);
            string outputFileName = nameof(outputFileName);
            string contents = nameof(contents);

            io.ReadTextFromFile(inputFileName).Returns(contents);
            yamlDeserializer.Deserialize<object>(contents).Returns(value);
            io.WriteBinaryToFile(
                fileName: Arg.Is(outputFileName),
                action: Arg.Do<Action<IBinaryWriter>>(action => action(binaryWriter))
            );

            fileConverter.Convert(inputFilePath: inputFileName, outputFilePath: outputFileName);

            slbSerializer.Received().Write(binaryWriter, value);
        }

        [Test]
        public void Test_Converting_A_File_With_A_Bad_Format()
        {
            string inputFileName = nameof(inputFileName);
            string outputFileName = nameof(outputFileName);
            string contents = nameof(contents);
            YamlException cause = new YamlException(string.Empty);

            io.ReadTextFromFile(inputFileName).Returns(contents);
            yamlDeserializer.Deserialize<object>(contents).Returns(_ => throw cause);

            Action action = () => fileConverter
                .Convert(inputFilePath: inputFileName, outputFilePath: outputFileName);

            action.Should()
                .ThrowExactly<BadFormatException>()
                .Which
                .InnerException
                .Should()
                .BeSameAs(cause);

            slbSerializer.DidNotReceiveWithAnyArgs().Write(Arg.Any<IBinaryWriter>(), Arg.Any<string>());
        }
    }
}
