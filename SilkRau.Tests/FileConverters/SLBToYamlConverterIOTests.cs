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

namespace SilkRau.Tests.FileConverters
{
    class SLBToYamlConverterIOTests
    {
        private readonly SLBToYamlConverter<object>.IIO io;

        public SLBToYamlConverterIOTests()
        {
            io = new SLBToYamlConverter<object>.IO();
        }

        [Test]
        public void Test_Writing_To_An_Existing_File()
        {
            string expected = GetTextFileContents();
            string path = FileManager.CreateFile("existing_file");

            File.WriteAllText(path, "this should be gone");

            io.WriteTextToFile(path, expected);

            string result = File.ReadAllText(path);

            result.Should().Be(expected);
        }

        [Test]
        public void Test_Writing_To_An_Non_Existing_File()
        {
            string expected = GetTextFileContents();
            string path = FileManager.GetPathForFile("new_file");

            io.WriteTextToFile(path, expected);

            string result = File.ReadAllText(path);

            result.Should().Be(expected);
        }

        private string GetTextFileContents()
            => $"Running {GetType().FullName} @ {DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss:ffff")}";
    }
}
