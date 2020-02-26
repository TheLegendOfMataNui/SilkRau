/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using Equ;
using NUtils.MethodBuilders;
using System;
using System.Collections.Generic;

namespace SilkRau
{
    internal sealed class FileConversion : IEquatable<FileConversion>
    {
        private static readonly IEqualityComparer<FileConversion> equalityComparer
            = MemberwiseEqualityComparer<FileConversion>.ByProperties;

        private static readonly ToStringMethod<FileConversion> toStringMethod
            = new ToStringMethodBuilder<FileConversion>().UseProperties().Build();

        public FileConversion(FileFormat inputFileFormat, FileFormat outputFileFormat)
        {
            InputFileFormat = inputFileFormat;
            OutputFileFormat = outputFileFormat;
        }

        public FileFormat InputFileFormat { get; }

        public FileFormat OutputFileFormat { get; }

        public bool Equals(FileConversion other) => equalityComparer.Equals(this, other);

        public override bool Equals(object obj) => Equals(obj as FileConversion);

        public override int GetHashCode() => equalityComparer.GetHashCode(this);

        public override string ToString() => toStringMethod(this);

        public static bool operator ==(FileConversion left, FileConversion right)
            => left?.Equals(right) ?? right?.Equals(left) ?? true;

        public static bool operator !=(FileConversion left, FileConversion right)
            => !(left == right);
    }
}
