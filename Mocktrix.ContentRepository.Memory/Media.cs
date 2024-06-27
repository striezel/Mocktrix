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

using Mocktrix.Data;

namespace Mocktrix.ContentRepository.Memory
{
    /// <summary>
    /// In-memory implementation of Matrix content (MXC) repository.
    /// </summary>
    public static class Media
    {
        /// <summary>
        /// in-memory content list
        /// </summary>
        private static readonly List<Content> contents = [];


        /// <summary>
        /// Adds new uploaded content to the repository.
        /// </summary>
        /// <param name="bytes">the actual file content</param>
        /// <param name="content_type">the Content-Type of the uploaded file, if any</param>
        /// <param name="file_name">file name of the uploaded file</param>
        /// <returns>Returns the media id of the created content.</returns>
        public static string Create(byte[] bytes, string? content_type, string? file_name)
        {
            var id = Content.GenerateRandomId();
            while (contents.FindIndex(x => x.Id == id) != -1)
            {
                id = Content.GenerateRandomId();
            }

            Content content = new(id, content_type, Content.SanitizeFileName(file_name), bytes);
            contents.Add(content);
            return id;
        }


        /// <summary>
        /// Adds new uploaded content to the repository.
        /// </summary>
        /// <param name="bytes">the actual file content</param>
        /// <param name="content_type">the Content-Type of the uploaded file, if any</param>
        /// <param name="file_name">file name of the uploaded file</param>
        /// <returns>Returns the media id of the created content.</returns>
        public static string Create(ReadOnlySpan<byte> bytes, string? content_type, string? file_name)
        {
            return Create(bytes.ToArray(), content_type, file_name);
        }


        /// <summary>
        /// Gets an existing content.
        /// </summary>
        /// <param name="media_id">id of the content</param>
        /// <returns>Returns the content with the matching id, if it exists.
        /// Returns null, if no match was found.</returns>
        public static Content? GetContent(string media_id)
        {
            return contents.Find(element => element.Id == media_id);
        }
    }
}
