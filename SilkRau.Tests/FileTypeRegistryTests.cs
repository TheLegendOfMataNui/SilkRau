/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace SilkRau.Tests
{
    class FileTypeRegistryTests
    {
        [Test]
        public void Test_Getting_The_Supported_Types()
        {
            IReadOnlyDictionary<string, Type> mapping = new Dictionary<string, Type>
            {
                ["type1"] = typeof(int),
                ["type2"] = typeof(string),
                ["type3"] = typeof(Type)
            };
            IFileTypeRegistry registry = new FileTypeRegistry(mapping);

            ISet<string> result = registry.SupportedFileTypes;

            result.Should().Equal(mapping.Keys);
        }

        [Test]
        public void Test_Getting_The_Type_From_A_Valid_FileType()
        {
            string registryName = nameof(registryName);
            IReadOnlyDictionary<string, Type> mapping = new Dictionary<string, Type>
            {
                [registryName] = typeof(IFileTypeRegistry)
            };

            IFileTypeRegistry registry = new FileTypeRegistry(mapping);

            Type result = registry.GetTypeForFileType(registryName);

            result.Should().Be(typeof(IFileTypeRegistry));
        }

        [Test]
        public void Test_Getting_The_Type_From_An_Invalid_FileType()
        {
            string invalidFileType = nameof(invalidFileType);
            IFileTypeRegistry registry = new FileTypeRegistry(new Dictionary<string, Type>());
            Action action = () => registry.GetTypeForFileType(invalidFileType);

            action.Should()
                .ThrowExactly<InvalidFileTypeException>()
                .Where(exception => exception.InvalidFileType == invalidFileType);
        }
    }
}
