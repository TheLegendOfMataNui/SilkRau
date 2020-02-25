/*
* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/
using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using SilkRau.FileConverters;
using System;
using System.Collections.Generic;

namespace SilkRau.NinjectModules
{
    internal sealed class FileConvertersModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(SLBToYamlConverter<>)).ToSelf();
            Bind(typeof(SLBToYamlConverter<>.IIO)).To(typeof(SLBToYamlConverter<>.IO));

            Bind(typeof(YamlToSLBConverter<>)).ToSelf();
            Bind(typeof(YamlToSLBConverter<>.IIO)).To(typeof(YamlToSLBConverter<>.IO));

            Bind<IReadOnlyDictionary<FileConversion, FileConverterProvider>>().ToMethod(BuildProvidersDictionary);
        }

        private IReadOnlyDictionary<FileConversion, FileConverterProvider> BuildProvidersDictionary(IContext context)
        {
            IKernel kernel = context.Kernel;
            IFileConverter buildFileConverterWithType(Type fileConverterType)
                => (IFileConverter)kernel.Get(fileConverterType);

            return new Dictionary<FileConversion, FileConverterProvider>
            {
                [
                    new FileConversion(
                        inputFileFormat: FileFormat.SLB,
                        outputFileFormat: FileFormat.Yaml
                    )
                ] = fileType => buildFileConverterWithType(typeof(SLBToYamlConverter<>).MakeGenericType(fileType)),
                [
                    new FileConversion(
                        inputFileFormat: FileFormat.Yaml,
                        outputFileFormat: FileFormat.SLB
                    )
                ] = fileType => buildFileConverterWithType(typeof(YamlToSLBConverter<>).MakeGenericType(fileType))
            };
        }
    }
}
