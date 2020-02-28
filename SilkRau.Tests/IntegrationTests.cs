/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using FluentAssertions;
using Ninject;
using Ninject.Modules;
using NSubstitute;
using NSubstitute.ClearExtensions;
using NUnit.Framework;
using NUtils.Extensions;
using SAGESharp.IO.Binary;
using SilkRau.NinjectModules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;

namespace SilkRau.Tests
{
    [Category("integration")]
    partial class IntegrationTests
    {
        private static readonly IReadOnlyDictionary<string, Type> fileTypeMapping = new Dictionary<string, Type>
        {
            [typeof(string).Name] = typeof(string)
        };

        private readonly IKernel kernel;

        public IntegrationTests()
        {
            kernel = new StandardKernel(
                new TestsModule(),
                new FileConvertersModule(),
                new ProgramModule(fileTypeMapping)
            );
        }

        private string FileName { get => TestContext.CurrentContext.Test.MethodName; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            kernel.Get<ISerializer>()
                .Serialize(Arg.Any<string>())
                .Returns(callInfo => $"\"{callInfo.Arg<string>()}\"");

            kernel.Get<IDeserializer>()
                .Deserialize<string>(Arg.Any<string>())
                .Returns(callInfo => callInfo.Arg<string>().Trim('"'));
        }

        private void SetupSLBFile(string filePath, string contents)
        {
            using (Stream stream = new FileStream(filePath, FileMode.Create))
            {
                kernel.Get<IBinarySerializer<string>>().Write(stream, contents);
            }
        }

        private void SetupYamlFile(string filePath, string contents)
        {
            using (Stream stream = new FileStream(filePath, FileMode.Create))
            {
                using (TextWriter writer = new StreamWriter(stream))
                {
                    kernel.Get<ISerializer>()
                        .Serialize(contents)
                        .Also(writer.Write);
                }
            }
        }

        private void ValidateSLBFile(string filePath, string expectedContents)
        {
            File.Exists(filePath).Should().BeTrue();

            using (Stream stream = File.ReadAllBytes(filePath).Let(it => new MemoryStream(it)))
            {
                kernel.Get<IBinarySerializer<string>>()
                    .Read(stream)
                    .Should()
                    .Be(expectedContents);
            }
        }

        private void ValidateYamlFile(string filePath, string expectedContents)
        {
            File.Exists(filePath).Should().BeTrue();

            string contents = File.ReadAllText(filePath);

            contents.Should().Be(kernel.Get<ISerializer>().Serialize(expectedContents));
        }

        private class TestsModule : NinjectModule
        {
            public override void Load()
            {
                Bind<TextWriter>()
                    .ToConstant(TestContext.Out);

                Bind<IBinarySerializer<string>>()
                    .ToConstant(new StringBinarySerializer());

                Bind<IDeserializer>()
                    .ToConstant(Substitute.For<IDeserializer>());

                Bind<ISerializer>()
                    .ToConstant(Substitute.For<ISerializer>());
            }
        }

        private class StringBinarySerializer : IBinarySerializer<string>
        {
            public string Read(IBinaryReader binaryReader)
            {
                int length = binaryReader.ReadInt32();

                return binaryReader
                    .ReadBytes(length)
                    .Let(Encoding.ASCII.GetString);
            }

            public void Write(IBinaryWriter binaryWriter, string value)
            {
                binaryWriter.WriteInt32(value.Length);

                value.ToCharArray()
                    .Let(Encoding.ASCII.GetBytes)
                    .Also(binaryWriter.WriteBytes);
            }
        }
    }
}
