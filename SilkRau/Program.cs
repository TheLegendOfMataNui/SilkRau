/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using SilkRau.Options;
using System;
using System.IO;

namespace SilkRau
{
    internal sealed class Program
    {
        private readonly IFileTypeRegistry fileTypeRegistry;

        private readonly IFileConverterFactory fileConverterFactory;

        private readonly TextWriter textWriter;

        public Program(
            IFileTypeRegistry fileTypeRegistry,
            IFileConverterFactory fileConverterFactory,
            TextWriter textWriter
        ) {
            this.fileTypeRegistry = fileTypeRegistry;
            this.fileConverterFactory = fileConverterFactory;
            this.textWriter = textWriter;
        }

        static void Main(string[] args)
        {
        }

        public void Run(ConvertOptions options)
        {
            IFileConverter fileConverter = fileConverterFactory.BuildFileConverter(
                fileType: fileTypeRegistry.GetTypeForFileType(options.FileType),
                fileConversion: new FileConversion(
                    inputFileFormat: options.InputFormat,
                    outputFileFormat: options.OutputFormat
                )
            );

            string outputFile = options.OutputFile ??
                Path.ChangeExtension(options.InputFile, GetExtensionForFormat(options.OutputFormat));

            fileConverter.Convert(
                 inputFilePath: options.InputFile,
                 outputFilePath: outputFile
            );
        }

        private static string GetExtensionForFormat(FileFormat fileFormat)
        {
            if (FileFormat.SLB == fileFormat)
            {
                return "slb";
            }
            else if (FileFormat.Yaml == fileFormat)
            {
                return "yaml";
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        public void Run(PrintOptions options)
        {
            if (options.FileTypes)
            {
                foreach (string fileType in fileTypeRegistry.SupportedFileTypes)
                {
                    textWriter.WriteLine(fileType);
                }
            }
            else if (options.Conversions)
            {
                foreach (FileConversion fileConversion in fileConverterFactory.ValidConversions)
                {
                    textWriter.WriteLine($"{fileConversion.InputFileFormat} to {fileConversion.OutputFileFormat}");
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
