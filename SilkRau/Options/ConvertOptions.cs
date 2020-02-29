/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using CommandLine;
using NUtils.MethodBuilders;

namespace SilkRau.Options
{
    [Verb("convert", HelpText = "Converts a file from a format to another.")]
    internal sealed class ConvertOptions
    {
        private static readonly ToStringMethod<ConvertOptions> toStringMethod
            = new ToStringMethodBuilder<ConvertOptions>().UseProperties().Build();

        public ConvertOptions(
            string fileType,
            FileFormat inputFormat,
            string inputFile,
            FileFormat outputFormat,
            string outputFile,
            bool force
        ) {
            FileType = fileType;
            InputFormat = inputFormat;
            InputFile = inputFile;
            OutputFormat = outputFormat;
            OutputFile = outputFile;
            Force = force;
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

        [Option('f', "force", Required = false, HelpText = "Forces the conversion despite any warnings.")]
        public bool Force { get; }

        public override string ToString() => toStringMethod(this);
    }
}
