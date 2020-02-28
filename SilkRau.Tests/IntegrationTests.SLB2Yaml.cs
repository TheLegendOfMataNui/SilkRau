/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using FluentAssertions;
using Ninject;
using NUnit.Framework;
using SilkRau.Options;
using System;

namespace SilkRau.Tests
{
    partial class IntegrationTests
    {
        [Test]
        public void Test_Converting_A_File_From_SLB_To_Yaml()
        {
            string contents = "hello world!";
            string inputFilePath = FileManager.GetPathForFile($"{FileName}.slb");
            string outputFilePath = FileManager.GetPathForFile($"{FileName}.yaml");

            SetupSLBFile(filePath: inputFilePath, contents: contents);

            kernel.Get<Program>().Run(new ConvertOptions(
                fileType: typeof(string).Name,
                inputFormat: FileFormat.SLB,
                inputFile: inputFilePath,
                outputFormat: FileFormat.Yaml,
                outputFile: outputFilePath,
                force: false
            ));

            ValidateYamlFile(filePath: outputFilePath, expectedContents: contents);
        }

        [Test]
        public void Test_Converting_A_File_From_SLB_To_Yaml_Without_Output_Path()
        {
            string contents = "hello world!";
            string inputFilePath = FileManager.GetPathForFile($"{FileName}.slb");
            string outputFilePath = FileManager.GetPathForFile($"{FileName}.yaml");

            SetupSLBFile(filePath: inputFilePath, contents: contents);

            kernel.Get<Program>().Run(new ConvertOptions(
                fileType: typeof(string).Name,
                inputFormat: FileFormat.SLB,
                inputFile: inputFilePath,
                outputFormat: FileFormat.Yaml,
                outputFile: null,
                force: false
            ));

            ValidateYamlFile(filePath: outputFilePath, expectedContents: contents);
        }

        [Test]
        public void Test_Converting_A_File_From_SLB_To_An_Existing_Yaml_With_Force()
        {
            string contents = "hello world!";
            string inputFilePath = FileManager.GetPathForFile($"{FileName}.slb");
            string outputFilePath = FileManager.GetPathForFile($"{FileName}.yaml");

            SetupSLBFile(filePath: inputFilePath, contents: contents);
            FileManager.CreateFile(outputFilePath);

            kernel.Get<Program>().Run(new ConvertOptions(
                fileType: typeof(string).Name,
                inputFormat: FileFormat.SLB,
                inputFile: inputFilePath,
                outputFormat: FileFormat.Yaml,
                outputFile: outputFilePath,
                force: true
            ));

            ValidateYamlFile(filePath: outputFilePath, expectedContents: contents);
        }

        [Test]
        public void Test_Converting_A_File_From_SLB_To_An_Existing_Yaml_Without_Force()
        {
            string contents = "hello world!";
            string expectedContents = "this shouldn't be changed";
            string inputFilePath = FileManager.GetPathForFile($"{FileName}.slb");
            string outputFilePath = FileManager.GetPathForFile($"{FileName}.yaml");

            SetupSLBFile(filePath: inputFilePath, contents: contents);
            SetupYamlFile(filePath: outputFilePath, contents: expectedContents);

            Action action = () => kernel.Get<Program>().Run(new ConvertOptions(
                fileType: typeof(string).Name,
                inputFormat: FileFormat.SLB,
                inputFile: inputFilePath,
                outputFormat: FileFormat.Yaml,
                outputFile: outputFilePath,
                force: false
            ));

            action.Should()
                .ThrowExactly<ValidationException>()
                .WithMessage($"*{outputFilePath}*");

            ValidateYamlFile(filePath: outputFilePath, expectedContents: expectedContents);
        }

        [Test]
        public void Test_Converting_A_File_From_SLB_To_An_Existing_Yaml_Without_Output_Path_Or_Force()
        {
            string contents = "hello world!";
            string expectedContents = "this shouldn't be changed";
            string inputFilePath = FileManager.GetPathForFile($"{FileName}.slb");
            string outputFilePath = FileManager.GetPathForFile($"{FileName}.yaml");

            SetupSLBFile(filePath: inputFilePath, contents: contents);
            SetupYamlFile(filePath: outputFilePath, contents: expectedContents);

            Action action = () => kernel.Get<Program>().Run(new ConvertOptions(
                fileType: typeof(string).Name,
                inputFormat: FileFormat.SLB,
                inputFile: inputFilePath,
                outputFormat: FileFormat.Yaml,
                outputFile: null,
                force: false
            ));

            action.Should()
                .ThrowExactly<ValidationException>()
                .WithMessage($"*{outputFilePath}*");

            ValidateYamlFile(filePath: outputFilePath, expectedContents: expectedContents);
        }
    }
}
