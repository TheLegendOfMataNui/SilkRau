/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using Ninject.Activation;
using Ninject.Modules;
using SAGESharp.IO.Binary;
using SAGESharp.IO.Yaml;
using System;
using YamlDotNet.Serialization;

namespace SilkRau.NinjectModules
{
    internal sealed class SAGESharpModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IBinarySerializer<>))
                .ToMethod(BuildBinarySerializer);

            Bind<ISerializer>()
                .ToMethod(_ => YamlSerializer.BuildSLBSerializer());

            Bind<IDeserializer>()
                .ToMethod(_ => YamlDeserializer.BuildSLBDeserializer());
        }

        private static object BuildBinarySerializer(IContext context)
            => typeof(BinarySerializer)
                .GetMethod(nameof(BinarySerializer.ForType))
                .MakeGenericMethod(context.GenericArguments)
                .Invoke(null, Array.Empty<object>());
    }
}
