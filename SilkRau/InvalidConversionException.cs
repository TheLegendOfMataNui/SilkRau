/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using System;

namespace SilkRau
{
    internal sealed class InvalidConversionException : Exception
    {
        public InvalidConversionException(FileFormat inputFileFormat, FileFormat outputFileFormat)
            : base($"Invalid conversion from {inputFileFormat} to {outputFileFormat}")
        {
            InputFormat = inputFileFormat;
            OutputFormat = outputFileFormat;
        }

        public FileFormat InputFormat { get; }

        public FileFormat OutputFormat { get; }
    }
}
