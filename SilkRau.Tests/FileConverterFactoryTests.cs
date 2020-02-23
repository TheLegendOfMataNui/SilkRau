/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SilkRau.Tests
{
    class FileConverterFactoryTests
    {
        private static readonly FileConversion validFileConversion = new FileConversion(
            inputFileFormat: FileFormat.SLB, outputFileFormat: FileFormat.Yaml
        );

        private readonly IReadOnlyDictionary<FileConversion, FileConverterProvider>
            conversions;

        private readonly IFileConverterFactory factory;

        public FileConverterFactoryTests()
        {
            conversions = new Dictionary<FileConversion, FileConverterProvider>
            {
                [validFileConversion] = Substitute.For<FileConverterProvider>()
            };

            factory = new FileConverterFactory(conversions);
        }

        [Test]
        public void Test_The_Valid_Conversions_Are_As_Expected()
        {
            ISet<FileConversion> result = factory.ValidConversions;

            result.Count.Should().Be(conversions.Keys.Count());
            result.SetEquals(conversions.Keys).Should().BeTrue();
        }

        [Test]
        public void Test_Creating_A_Converter_For_A_Valid_Conversion()
        {
            IFileConverter fileConverter = Substitute.For<IFileConverter>();
            Type fileType = typeof(string);

            conversions[validFileConversion].Invoke(fileType).Returns(fileConverter);

            IFileConverter result = factory.BuildFileConverter(fileType, validFileConversion);

            result.Should().BeSameAs(fileConverter);
        }

        [Test]
        public void Test_Creating_A_Converter_For_An_Invalid_FileConversion()
        {
            FileConversion fileConversion = new FileConversion(
                    inputFileFormat: FileFormat.SLB,
                    outputFileFormat: FileFormat.SLB
            );
            Action action = () => factory.BuildFileConverter(typeof(string), fileConversion);

            action.Should()
                .ThrowExactly<InvalidConversionException>()
                .Where(exception => exception.FileConversion == fileConversion);
        }
    }
}
