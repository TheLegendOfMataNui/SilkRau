/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using CommandLine;
using NUtils.MethodBuilders;

namespace SilkRau.Options
{
    [Verb("print", HelpText = "Prints information about what operations are available.")]
    internal sealed class PrintOptions
    {
        private static readonly ToStringMethod<PrintOptions> toStringMethod
            = new ToStringMethodBuilder<PrintOptions>().UseProperties().Build();

        public PrintOptions(bool fileTypes, bool conversions)
        {
            FileTypes = fileTypes;
            Conversions = conversions;
        }

        [Option('t', "file-types", HelpText = "Prints all the file types supported for conversion.", SetName = nameof(FileTypes), Required = true)]
        public bool FileTypes { get; }

        [Option('c', "conversions", HelpText = "Prints all the supported conversions.", SetName = nameof(Conversions), Required = true)]
        public bool Conversions { get; }

        public override string ToString() => toStringMethod(this);
    }
}
