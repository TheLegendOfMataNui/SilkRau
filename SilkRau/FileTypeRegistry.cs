/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using System;

namespace SilkRau
{
    internal static class FileTypeRegistry
    {
        public static Type GetTypeForFileType(string fileType)
        {
            if (fileType == "SLB.Level.Conversation")
            {
                return typeof(SAGESharp.SLB.Level.Conversation.CharacterTable);
            }
            else
            {
                throw new InvalidFileTypeException(fileType);
            }
        }
    }
}
