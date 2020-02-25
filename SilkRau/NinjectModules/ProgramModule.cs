/*
* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/
using Ninject.Modules;
using System;
using System.Collections.Generic;

namespace SilkRau.NinjectModules
{
    internal sealed class ProgramModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IFileTypeRegistry>()
                .ToConstant(BuildFileTypeRegistry())
                .InSingletonScope();

            Bind<IFileConverterFactory>()
                .To<FileConverterFactory>()
                .InSingletonScope();

            Bind<Program>()
                .ToSelf()
                .InSingletonScope();
        }

        private IFileTypeRegistry BuildFileTypeRegistry()
        {
            IReadOnlyDictionary<string, Type> mapping = new Dictionary<string, Type>
            {
                ["SLB.Level.Conversation"] = typeof(SAGESharp.SLB.Level.Conversation.CharacterTable)
            };

            return new FileTypeRegistry(mapping);
        }
    }
}
