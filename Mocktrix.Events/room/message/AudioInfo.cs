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

using System.Text.Json.Serialization;

namespace Mocktrix.Events
{
    /// <summary>
    /// Contains metadata for an audio clip.
    /// </summary>
    public class AudioInfo
    {
        /// <summary>
        /// The duration of the audio in milliseconds.
        /// </summary>
        [JsonPropertyName("duration")]
        [JsonPropertyOrder(-30)]
        public int? Duration { get; set; } = null;


        /// <summary>
        /// The mimetype of the image, e.g. "image/jpeg".
        /// </summary>
        [JsonPropertyName("mimetype")]
        [JsonPropertyOrder(-20)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? MimeType { get; set; } = null;


        /// <summary>
        /// Size of the image in bytes.
        /// </summary>
        [JsonPropertyName("size")]
        [JsonPropertyOrder(-10)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Size { get; set; } = null;
    }
}
