/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */
namespace SilkRau
{
    /// <summary>
    /// Interface that represents an object that converts a file from one format to another.
    /// </summary>
    internal interface IFileConverter
    {
        /// <summary>
        /// Converts <paramref name="inputFilePath"/> to <paramref name="outputFilePath"/>.
        /// </summary>
        /// 
        /// <param name="inputFilePath">
        /// The path of the input file to convert.
        /// </param>
        /// 
        /// <param name="outputFilePath">
        /// The path of the output file that got converted.
        /// </param>
        void Convert(string inputFilePath, string outputFilePath);
    }
}
