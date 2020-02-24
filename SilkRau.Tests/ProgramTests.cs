/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using NSubstitute;
using NUnit.Framework;
using SilkRau.Options;
using System;
using System.Collections.Generic;
using System.IO;

namespace SilkRau.Tests
{
    class ProgramTests
    {
        private readonly IFileTypeRegistry fileTypeRegistry;

        private readonly IFileConverterFactory fileConverterFactory;

        private readonly TextWriter textWriter;

        private readonly Program program;

        public ProgramTests()
        {
            fileTypeRegistry = Substitute.For<IFileTypeRegistry>();
            fileConverterFactory = Substitute.For<IFileConverterFactory>();
            textWriter = Substitute.For<TextWriter>();
            program = new Program(fileTypeRegistry, fileConverterFactory, textWriter);
        }

        [Test]
        public void Test_Running_A_Program_For_Converting_A_File_With_Output()
        {
            ConvertOptions options = new ConvertOptions(
                fileType: "fileType",
                inputFormat: FileFormat.SLB,
                inputFile: "inputFile.slb",
                outputFormat: FileFormat.Yaml,
                outputFile: "outputFile.yaml"
            );
            FileConversion fileConversion = new FileConversion(
                inputFileFormat: options.InputFormat,
                outputFileFormat: options.OutputFormat
            );
            Type fileType = typeof(string);
            IFileConverter fileConverter = Substitute.For<IFileConverter>();

            fileTypeRegistry.GetTypeForFileType(options.FileType).Returns(fileType);
            fileConverterFactory.BuildFileConverter(Arg.Is(fileType), Arg.Is(fileConversion)).Returns(fileConverter);

            program.Run(options);

            fileConverter.Received().Convert(options.InputFile, options.OutputFile);
        }

        [Test]
        public void Test_Running_A_Program_For_Converting_A_File_Without_Output()
        {
            string fileName = nameof(fileName);
            ConvertOptions options = new ConvertOptions(
                fileType: "fileType",
                inputFormat: FileFormat.SLB,
                inputFile: $"{fileName}.slb",
                outputFormat: FileFormat.Yaml,
                outputFile: null
            );
            FileConversion fileConversion = new FileConversion(
                inputFileFormat: options.InputFormat,
                outputFileFormat: options.OutputFormat
            );
            Type fileType = typeof(string);
            IFileConverter fileConverter = Substitute.For<IFileConverter>();

            fileTypeRegistry.GetTypeForFileType(options.FileType).Returns(fileType);
            fileConverterFactory.BuildFileConverter(Arg.Is(fileType), Arg.Is(fileConversion)).Returns(fileConverter);

            program.Run(options);

            fileConverter.Received().Convert(options.InputFile, $"{fileName}.yaml");
        }

        [Test]
        public void Test_Running_A_Program_For_Printing_File_Types()
        {
            ISet<string> fileTypes = new HashSet<string>
            {
                "type 1",
                "type 2",
                "type 3",
                "type 4"
            };

            fileTypeRegistry.SupportedFileTypes.Returns(fileTypes);

            program.Run(new PrintOptions(
                fileTypes: true,
                conversions: false
            ));

            foreach (string fileType in fileTypes)
            {
                textWriter.Received().WriteLine(fileType);
            }
        }

        [Test]
        public void Test_Running_A_Program_For_Printing_Available_Conversions()
        {
            ISet<FileConversion> conversions = new HashSet<FileConversion>
            {
                new FileConversion(
                    inputFileFormat: FileFormat.SLB,
                    outputFileFormat: FileFormat.Yaml
                ),
                new FileConversion(
                    inputFileFormat: FileFormat.Yaml,
                    outputFileFormat: FileFormat.SLB
                )
            };

            fileConverterFactory.ValidConversions.Returns(conversions);

            program.Run(new PrintOptions(
                fileTypes: false,
                conversions: true
            ));

            foreach (FileConversion fileConversion in conversions)
            {
                textWriter.Received().WriteLine($"{fileConversion.InputFileFormat} to {fileConversion.OutputFileFormat}");
            }
        }
    }
}
