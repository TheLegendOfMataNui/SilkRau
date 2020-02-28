/*
* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/
using Ninject.Modules;
using NUtils.Extensions;
using System;
using System.Collections.Generic;

namespace SilkRau.NinjectModules
{
    internal sealed class ProgramModule : NinjectModule
    {
        private readonly IReadOnlyDictionary<string, Type> fileTypeMapping;

        public ProgramModule()
        {
            fileTypeMapping = BuildDefaultFileTypeMapping();
        }

        public ProgramModule(IReadOnlyDictionary<string, Type> fileTypeMapping)
        {
            this.fileTypeMapping = new Dictionary<string, Type>()
                .Also(it => fileTypeMapping.ForEach(it.Add));
        }

        public override void Load()
        {
            Bind<IFileTypeRegistry>()
                .ToConstant(new FileTypeRegistry(fileTypeMapping));

            Bind<IFileConverterFactory>()
                .To<FileConverterFactory>()
                .InSingletonScope();

            Bind<IPathValidator>()
                .To<PathValidator>()
                .InSingletonScope();

            Bind<Program>()
                .ToSelf()
                .InSingletonScope();
        }

        private static IReadOnlyDictionary<string, Type> BuildDefaultFileTypeMapping() => new Dictionary<string, Type>
        {
            ["SLB.Level.Conversation"] = typeof(SAGESharp.SLB.Level.Conversation.CharacterTable)
        };
    }
}
