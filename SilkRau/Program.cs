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

        public Program(
            IFileTypeRegistry fileTypeRegistry,
            IFileConverterFactory fileConverterFactory
        ) {
            this.fileTypeRegistry = fileTypeRegistry;
            this.fileConverterFactory = fileConverterFactory;
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
    }
}
