/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using CommandLine;
using Ninject.Modules;
using System;
using System.IO;

namespace SilkRau.NinjectModules
{
    internal sealed class CommandLineModule : NinjectModule
    {
        public override void Load()
        {
            Bind<TextWriter>().ToConstant(Console.Out).InSingletonScope();
            Bind<Parser>().ToConstant(new Parser(settings =>
            {
                settings.CaseSensitive = false;
                settings.HelpWriter = Console.Out;
            })).InSingletonScope();
        }
    }
}
