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
    internal sealed class FileTypeRegistry : IFileTypeRegistry
    {
        private readonly IReadOnlyDictionary<string, Type> mapping;

        public FileTypeRegistry(IReadOnlyDictionary<string, Type> mapping)
        {
            // Create and initialize a copy of the input dictionary
            this.mapping = new Dictionary<string, Type>()
                .Also(dictionary => mapping.ForEach(dictionary.Add));
        }

        public ISet<string> SupportedFileTypes { get => new HashSet<string>(mapping.Keys); }

        public Type GetTypeForFileType(string fileType)
        {
            if (mapping.ContainsKey(fileType))
            {
                return mapping[fileType];
            }
            else
            {
                throw new InvalidFileTypeException(fileType);
            }
        }
    }
}
