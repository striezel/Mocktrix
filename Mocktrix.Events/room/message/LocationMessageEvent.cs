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
    /// Event for location messages in a room.
    /// </summary>
    public class LocationMessageEvent: RoomMessageEvent<LocationMessageEventContent>
    {
    }


    /// <summary>
    /// Event content for LocationMessageEvent.
    /// </summary>
    public class LocationMessageEventContent : RoomMessageEventContent
    {
        /// <summary>
        /// A geo URI representing this location.
        /// </summary>
        [JsonPropertyName("geo_uri")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string GeoUri { get; set; } = null!;


        /// <summary>
        /// Metadata about the image referred to in Url.
        /// </summary>
        [JsonPropertyName("info")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public LocationInfo? Info { get; set; } = null;


        /// <summary>
        /// The type of message, e.g. "m.text", "m.file", ...
        /// </summary>
        [JsonPropertyName("msgtype")]
        public override string MessageType
        {
            get => "m.location";
            set
            {
                if (value != "m.location")
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be 'm.location'.");
                }
            }
        }
    }
}
