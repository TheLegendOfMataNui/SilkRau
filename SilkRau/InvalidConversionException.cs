/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using System;

namespace SilkRau
{
    internal sealed class InvalidConversionException : SilkRauException
    {
        public InvalidConversionException(FileConversion fileConversion)
            : base($"Invalid conversion from {fileConversion.InputFileFormat} to {fileConversion.OutputFileFormat}")
        {
            FileConversion = fileConversion;
        }

        public FileConversion FileConversion { get; }
    }
}
