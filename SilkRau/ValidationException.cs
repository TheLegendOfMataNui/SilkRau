/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
using System;

namespace SilkRau
{

    internal sealed class ValidationException : SilkRauException
    {
        public ValidationException()
        {
        }

        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
