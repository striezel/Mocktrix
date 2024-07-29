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
    /// Contains metadata for an image.
    /// </summary>
    public class ImageInfo: ThumbnailInfo
    {
        // TODO: Add ThumbnailFile when encryption module is supported.
        // public ThumbnailFile? ThumbnailFile { get; set; } = null;


        /// <summary>
        /// Metadata about the image referred to in ThumbnailUrl.
        /// </summary>
        [JsonPropertyName("thumbnail_info")]
        [JsonPropertyOrder(-40)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ThumbnailInfo? ThumbnailInfo { get; set; } = null;


        /// <summary>
        /// The URL (typically MXC URI) to a thumbnail of the image.
        /// Only present if the thumbnail is unencrypted.
        /// </summary>
        [JsonPropertyName("thumbnail_url")]
        [JsonPropertyOrder(-30)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ThumbnailUrl { get; set; } = null;
    }
}
