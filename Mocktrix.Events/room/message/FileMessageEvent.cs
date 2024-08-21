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
    /// Event for file messages in a room.
    /// </summary>
    public class FileMessageEvent : RoomMessageEvent<FileMessageEventContent>
    {
    }


    /// <summary>
    /// Event content for FileMessageEvent.
    /// </summary>
    public class FileMessageEventContent : RoomMessageEventContent
    {
        // TODO: Add File when encryption module is supported.
        // public EncryptedFile? File { get; set; } = null;


        /// <summary>
        /// The original filename of the uploaded file.
        /// </summary>
        [JsonPropertyName("filename")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FileName { get; set; } = null;


        /// <summary>
        /// Metadata about the image referred to in Url.
        /// </summary>
        [JsonPropertyName("info")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public FileInfo? Info { get; set; } = null;


        /// <summary>
        /// The type of message, e.g. "m.text", "m.file", ...
        /// </summary>
        [JsonPropertyName("msgtype")]
        public override string MessageType
        {
            get => "m.file";
            set
            {
                if (value != "m.file")
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be 'm.file'.");
                }
            }
        }


        /// <summary>
        /// Required if the file is unencrypted.
        /// The URL (typically MXC URI) to the file.
        /// </summary>
        [JsonPropertyName("url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Url { get; set; } = null;
    }
}
