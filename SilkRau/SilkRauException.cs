/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using System;
using System.Runtime.Serialization;

namespace SilkRau
{
    [Serializable]
    internal abstract class SilkRauException : Exception
    {
        public SilkRauException()
        {
        }

        public SilkRauException(string message) : base(message)
        {
        }

        public SilkRauException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SilkRauException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
