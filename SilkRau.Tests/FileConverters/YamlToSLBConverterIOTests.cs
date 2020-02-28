/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using FluentAssertions;
using NUnit.Framework;
using SilkRau.FileConverters;
using System;
using System.IO;
using System.Text;

namespace SilkRau.Tests.FileConverters
{
    class YamlToSLBConverterIOTests
    {
        private readonly YamlToSLBConverter<object>.IIO io;

        public YamlToSLBConverterIOTests()
        {
            io = new YamlToSLBConverter<object>.IO();
        }

        [Test]
        public void Test_Writing_To_An_Existing_File()
        {
            byte[] expected = GetBinaryFileContents();
            string path = FileManager.CreateFile("existing_file");

            File.WriteAllBytes(path, Encoding.ASCII.GetBytes("This should be gone"));

            io.WriteBinaryToFile(path, binaryWriter => binaryWriter.WriteBytes(expected));

            byte[] result = File.ReadAllBytes(path);

            result.Should().Equal(expected);
        }

        [Test]
        public void Test_Writing_To_An_Non_Existing_File()
        {
            byte[] expected = GetBinaryFileContents();
            string path = FileManager.GetPathForFile("new_file");

            io.WriteBinaryToFile(path, binaryWriter => binaryWriter.WriteBytes(expected));

            byte[] result = File.ReadAllBytes(path);

            result.Should().Equal(expected);
        }

        private byte[] GetBinaryFileContents() => Encoding.ASCII.GetBytes(
            $"Running {GetType().FullName} @ {DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss:ffff")}"
        );
    }
}
