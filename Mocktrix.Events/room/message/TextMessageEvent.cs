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
    /// Event for text messages in a room.
    /// </summary>
    public class TextMessageEvent: RoomMessageEvent<TextMessageEventContent>
    {
    }


    /// <summary>
    /// Event content for TextMessageEvent.
    /// </summary>
    public class TextMessageEventContent : RoomMessageEventContent
    {
        /// <summary>
        /// The format used in the formatted_body.
        /// Currently only "org.matrix.custom.html" is supported.
        /// </summary>
        [JsonPropertyName("format")]
        [JsonPropertyOrder(-90)]
        public string? Format { get; set; } = null;


        /// <summary>
        /// The formatted version of the body. This is required if format is specified.
        /// </summary>
        [JsonPropertyName("formatted_body")]
        [JsonPropertyOrder(-80)]
        public string? FormattedBody { get; set; } = null;


        /// <summary>
        /// The type of message, e.g. "m.text", "m.file", ...
        /// </summary>
        [JsonPropertyName("msgtype")]
        public override string MessageType
        {
            get => "m.text";
            set
            {
                if (value != "m.text")
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be 'm.text'.");
                }
            }
        }
    }
}
