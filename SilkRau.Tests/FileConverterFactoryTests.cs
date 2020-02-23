/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using FluentAssertions;
using NUnit.Framework;
using SAGESharp.IO;
using SilkRau.FileConverters;
using System;

namespace SilkRau.Tests
{
    class FileConverterFactoryTests
    {
        [Test]
        public void Test_Creating_A_Converter_From_SLB_To_Yaml()
        {
            FileConverterFactory factory = new FileConverterFactory(
                fileType: typeof(MockSLBType),
                inputFormat: FileFormat.SLB,
                outputFormat: FileFormat.Yaml
            );

            IFileConverter result = factory.BuildFileConverter();

            result.Should()
                .NotBeNull()
                .And
                .BeOfType<SLBToYamlConverter<MockSLBType>>();
        }


        [Test]
        public void Test_Creating_A_Converter_From_Yaml_To_SBL()
        {
            FileConverterFactory factory = new FileConverterFactory(
                fileType: typeof(MockSLBType),
                inputFormat: FileFormat.Yaml,
                outputFormat: FileFormat.SLB
            );

            IFileConverter result = factory.BuildFileConverter();

            result.Should()
                .NotBeNull()
                .And
                .BeOfType<YamlToSLBConverter<MockSLBType>>();
        }

        private class MockSLBType
        {
            [SerializableProperty(0)]
            public int Value { get; set; }
        }

        [Test]
        public void Test_Creating_A_Converter_For_An_Invalid_Pair_Of_Formats()
        {
            FileConverterFactory factory = new FileConverterFactory(
                fileType: typeof(MockSLBType),
                inputFormat: FileFormat.SLB,
                outputFormat: FileFormat.SLB
            );
            Action action = () => factory.BuildFileConverter();

            action.Should()
                .ThrowExactly<InvalidConversionException>()
                .Where(exception => exception.InputFormat == FileFormat.SLB)
                .Where(exception => exception.OutputFormat == FileFormat.SLB);
        }
    }
}
