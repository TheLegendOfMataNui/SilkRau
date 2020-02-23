/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using NUtils.Extensions;
using System;
using System.Collections.Generic;

namespace SilkRau
{
    internal delegate IFileConverter FileConverterProvider(Type fileType);

    internal sealed class FileConverterFactory : IFileConverterFactory
    {
        private readonly IReadOnlyDictionary<FileConversion, FileConverterProvider> conversions;

        public FileConverterFactory(IReadOnlyDictionary<FileConversion, FileConverterProvider> conversions)
        {
            // Create and initialize a copy of the input dictionary
            this.conversions = new Dictionary<FileConversion, FileConverterProvider>()
                .Also(dictionary => conversions.ForEach(dictionary.Add));
        }

        public ISet<FileConversion> ValidConversions
        {
            get => new HashSet<FileConversion>(conversions.Keys);
        }

        public IFileConverter BuildFileConverter(Type fileType, FileConversion fileConversion)
        {
            if (conversions.ContainsKey(fileConversion))
            {
                return conversions[fileConversion](fileType);
            }
            else
            {
                throw new InvalidConversionException(fileConversion);
            }
        }
    }
}
