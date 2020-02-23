/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

namespace SilkRau.Tests
{
    class FileTypeRegistryTests
    {
        [Test]
        public void Test_Getting_The_Type_From_A_Valid_FileType()
        {
             string supportedFileType = FileTypeRegistry
                .SupportedFileTypes
                .First();

            Type result = FileTypeRegistry.GetTypeForFileType(supportedFileType);

            result.Should().NotBeNull();
        }

        [Test]
        public void Test_Getting_The_Type_From_An_Invalid_FileType()
        {
            string invalidFileType = nameof(invalidFileType);
            Action action = () => FileTypeRegistry.GetTypeForFileType(invalidFileType);

            action.Should()
                .ThrowExactly<InvalidFileTypeException>()
                .Where(exception => exception.InvalidFileType == invalidFileType);
        }
    }
}
