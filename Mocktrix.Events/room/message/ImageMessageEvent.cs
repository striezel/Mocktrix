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
    /// Event for image messages in a room.
    /// </summary>
    public class ImageMessageEvent: RoomMessageEvent<ImageMessageEventContent>
    {
    }


    /// <summary>
    /// Event content for ImageMessageEvent.
    /// </summary>
    public class ImageMessageEventContent : RoomMessageEventContent
    {
        // TODO: Add File when encryption module is supported.
        // public EncryptedFile? File { get; set; } = null;


        /// <summary>
        /// Metadata about the image referred to in Url.
        /// </summary>
        [JsonPropertyName("info")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ImageInfo? Info { get; set; } = null;


        /// <summary>
        /// The type of message, e.g. "m.text", "m.file", ...
        /// </summary>
        [JsonPropertyName("msgtype")]
        public override string MessageType
        {
            get => "m.image";
            set
            {
                if (value != "m.image")
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be 'm.image'.");
                }
            }
        }


        /// <summary>
        /// Required if the file is unencrypted.
        /// The URL (typically MXC URI) to the image.
        /// </summary>
        [JsonPropertyName("url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Url { get; set; } = null;
    }

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

    public class ThumbnailInfo
    {
        /// <summary>
        /// The intended display height of the image in pixels.
        /// This may differ from the intrinsic dimensions of the image file.
        /// </summary>
        [JsonPropertyName("h")]
        [JsonPropertyOrder(-80)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Height { get; set; } = null;


        /// <summary>
        /// The mimetype of the image, e.g. "image/jpeg".
        /// </summary>
        [JsonPropertyName("mimetype")]
        [JsonPropertyOrder(-70)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? MimeType { get; set; } = null;


        /// <summary>
        /// Size of the image in bytes.
        /// </summary>
        [JsonPropertyName("size")]
        [JsonPropertyOrder(-60)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Size { get; set; } = null;


        /// <summary>
        /// The intended display width of the image in pixels.
        /// This may differ from the intrinsic dimensions of the image file.
        /// </summary>
        [JsonPropertyName("w")]
        [JsonPropertyOrder(-20)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Width { get; set; } = null;
    }
}
