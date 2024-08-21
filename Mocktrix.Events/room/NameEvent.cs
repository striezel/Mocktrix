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
    /// Event for name of a room.
    /// </summary>
    public class NameEvent : StateEventZeroLengthKey<NameEventContent>
    {
        /// <summary>
        /// The content object of the event. Type and available field differ
        /// depending on the concrete type.
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(IEvent.ContentPropertyOrder)]
        public NameEventContent Content { get; set; } = new();


        [JsonPropertyName("type")]
        [JsonPropertyOrder(-30)]
        public override string Type
        {
            get => "m.room.name";
            set
            {
                if (value != "m.room.name")
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be 'm.room.name'.");
                }
            }
        }
    }


    /// <summary>
    /// Event content for NameEvent.
    /// </summary>
    public class NameEventContent : IEventContent
    {
        /// <summary>
        /// Name of the room.
        /// As per Matrix specification, this name must not exceed 255 characters.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;
    }
}
