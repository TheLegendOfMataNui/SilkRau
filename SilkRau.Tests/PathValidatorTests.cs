/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Reflection;

namespace SilkRau.Tests
{
    class PathValidatorTests
    {
        private readonly IPathValidator pathValidator;

        public PathValidatorTests()
        {
            pathValidator = new PathValidator();
        }

        [Test]
        public void Test_Validate_An_Inexisting_File_Does_Not_Exist()
        {
            string path = $"{Assembly.GetExecutingAssembly().Location}.doesnt_exist";
            Action action = () => pathValidator.ValidateFileDoesNotExist(path);

            action.Should()
                .NotThrow();
        }

        [Test]
        public void Test_Validate_An_Existing_File_Does_Not_Exist()
        {
            string path = Assembly.GetExecutingAssembly().Location;
            Action action = () => pathValidator.ValidateFileDoesNotExist(path);

            action.Should()
                .Throw<ValidationException>()
                .WithMessage($"{path} already exists");
        }
    }
}
