﻿/*
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
    /// Event for video messages in a room.
    /// </summary>
    public class VideoMessageEvent : RoomMessageEvent<VideoMessageEventContent>
    {
    }


    /// <summary>
    /// Event content for VideoMessageEvent.
    /// </summary>
    public class VideoMessageEventContent : RoomMessageEventContent
    {
        // TODO: Add File when encryption module is supported.
        // public EncryptedFile? File { get; set; } = null;


        /// <summary>
        /// Metadata for the video clip referred to in Url.
        /// </summary>
        [JsonPropertyName("info")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public VideoInfo? Info { get; set; } = null;


        /// <summary>
        /// The type of message, e.g. "m.text", "m.file", ...
        /// </summary>
        [JsonPropertyName("msgtype")]
        public override string MessageType
        {
            get => "m.video";
            set
            {
                if (value != "m.video")
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be 'm.video'.");
                }
            }
        }


        /// <summary>
        /// Required if the file is unencrypted.
        /// The URL (typically MXC URI) to the video clip.
        /// </summary>
        [JsonPropertyName("url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Url { get; set; } = null;
    }
}
