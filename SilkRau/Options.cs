/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using CommandLine;

namespace SilkRau
{
    internal sealed class Options
    {
        public Options(string fileType, FileFormat inputFormat, string inputFile, FileFormat outputFormat, string outputFile)
        {
            FileType = fileType;
            InputFormat = inputFormat;
            InputFile = inputFile;
            OutputFormat = outputFormat;
            OutputFile = outputFile;
        }

        [Option('t', "file-type", Required = true, HelpText = "The type of the file to convert.")]
        public string FileType { get; }

        [Option("input-format", Required = true, HelpText = "The format of the file to convert.")]
        public FileFormat InputFormat { get; }

        [Option('i', "input-file", Required = true, HelpText = "The path of the file to convert.")]
        public string InputFile { get; }

        [Option("output-format", Required = true, HelpText = "The format of the output file.")]
        public FileFormat OutputFormat { get; }

        [Option('o', "output-file", Required = false, HelpText = "The path of the output file.")]
        public string OutputFile { get; }
    }
}
