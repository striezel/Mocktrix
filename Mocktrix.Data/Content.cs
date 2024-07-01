/*
    This file is part of Mocktrix.
    Copyright (C) 2024  Dirk Stolle

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Security.Cryptography;

namespace Mocktrix.Data
{
    /// <summary>
    /// Holds Matrix Content (MXC) data.
    /// </summary>
    /// <param name="id">the media id, i.e. some opaque id</param>
    /// <param name="content_type">the Content-Type of the uploaded file, e.g. "text/plain"</param>
    /// <param name="file_name">the file name used during upload</param>
    /// <param name="bytes">the file content</param>
    public class Content(string id, string? content_type, string? file_name, byte[] bytes)
    {
        /// <summary>
        /// id of the content, i.e. the path component of the MXC URI
        /// </summary>
        public string Id = id;


        /// <summary>
        /// hashed password
        /// </summary>
        public string? ContentType = content_type;


        /// <summary>
        /// the file name used during the upload, if any
        /// </summary>
        public string? FileName = file_name;


        /// <summary>
        /// the actual file content
        /// </summary>
        public byte[] Bytes = bytes;


        /// <summary>
        /// Generates a random media id.
        /// </summary>
        /// <returns>Returns a new media id.</returns>
        public static string GenerateRandomId()
        {
            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".AsSpan();
            return RandomNumberGenerator.GetString(alphabet, 24);
        }


        /// <summary>
        /// Tries to sanitize a given file name.
        /// </summary>
        /// <param name="file_name">the file name to sanitize</param>
        /// <returns>Returns the sanitized file name.
        /// May be null, even if the original file name was not null.</returns>
        public static string? SanitizeFileName(string? file_name)
        {
            if (string.IsNullOrWhiteSpace(file_name))
            {
                return null;
            }
            file_name = file_name.Trim();

            int idx = Math.Max(file_name.IndexOf("../"), file_name.IndexOf("..\\"));
            while (idx != -1)
            {
                file_name = file_name.Remove(idx, 3);
                idx = Math.Max(file_name.IndexOf("../"), file_name.IndexOf("..\\"));
            }
            idx = file_name.IndexOfAny(['/', '\\']);
            while (idx != -1)
            {
                file_name = file_name.Remove(idx, 1);
                idx = file_name.IndexOfAny(['/', '\\']);
            }
            if (file_name.Length == 0)
            {
                return null;
            }

            return file_name;
        }
    }
}
