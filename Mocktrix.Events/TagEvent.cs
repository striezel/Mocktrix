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
    /// Event for end of a user-set tag of a room.
    /// </summary>
    public class TagEvent : IEvent
    {
        /// <summary>
        /// The content object of the event. Type and available field differ
        /// depending on the concrete type.
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(IEvent.ContentPropertyOrder)]
        public TagEventContent Content { get; set; } = new();


        [JsonPropertyName("type")]
        [JsonPropertyOrder(-30)]
        public string Type
        {
            get => "m.tag";
            set
            {
                if (value != "m.tag")
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be 'm.tag'.");
                }
            }
        }


        /// <summary>
        /// Indicates whether this event is a state event.
        /// </summary>
        /// <returns>Returns true, if the event is a state event.
        /// Returns false otherwise.</returns>
        public bool IsStateEvent()
        {
            return false;
        }
    }


    /// <summary>
    /// Event content for TagEvent.
    /// </summary>
    public class TagEventContent : IEventContent
    {
        /// <summary>
        /// Contains the tags on the room, where the key is the tag name and the
        /// value is the information about the tag order.
        /// </summary>
        [JsonPropertyName("tags")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SortedDictionary<string, OrderInfo>? Tags { get; set; } = null;
    }


    /// <summary>
    /// Contains information about the relative order of the room under the tag.
    /// </summary>
    public class OrderInfo
    {
        /// <summary>
        /// A number in a range [0;1] describing a relative position of the
        /// room under the given tag.
        /// </summary>
        [JsonPropertyName("order")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? Order { get; set; } = null;
    }
}
