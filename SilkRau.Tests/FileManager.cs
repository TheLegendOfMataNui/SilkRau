/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using NUnit.Framework;
using NUtils.Extensions;
using System;
using System.IO;

namespace SilkRau.Tests
{
    static class FileManager
    {
        private static readonly string outputDir;

        static FileManager()
        {
            Guid testExecutionId = Guid.NewGuid();

            outputDir = Path.Combine(TestContext.CurrentContext.TestDirectory, testExecutionId.ToString());

            Directory.CreateDirectory(outputDir);

            TestContext.WriteLine($"Files created by tests can be found at {outputDir}");
        }

        public static string GetPathForFile(string name)
            => Path.Combine(GetDirForCurrentClass(), name);

        public static string CreateFile(string name)
            => GetPathForFile(name).Also(it => File.Create(it).Dispose());

        private static string GetDirForCurrentClass()
            => Path.Combine(outputDir, TestContext.CurrentContext.Test.ClassName)
                .Also(it => Directory.CreateDirectory(it));
    }
}
