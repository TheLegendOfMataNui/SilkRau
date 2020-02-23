/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using System;
using System.Collections.Generic;

namespace SilkRau
{
    internal static class FileTypeRegistry
    {
        private static readonly IReadOnlyDictionary<string, Type> registry;

        static FileTypeRegistry()
        {
            registry = new Dictionary<string, Type>
            {
                ["SLB.Level.Conversation"] = typeof(SAGESharp.SLB.Level.Conversation.CharacterTable)
            };
        }

        public static ISet<string> SupportedFileTypes { get => new HashSet<string>(registry.Keys); }

        public static Type GetTypeForFileType(string fileType)
        {
            if (registry.ContainsKey(fileType))
            {
                return registry[fileType];
            }
            else
            {
                throw new InvalidFileTypeException(fileType);
            }
        }
    }
}
