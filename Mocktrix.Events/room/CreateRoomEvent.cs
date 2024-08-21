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
    /// Event for creation of a room.
    /// </summary>
    public class CreateRoomEvent : StateEventZeroLengthKey<CreateRoomEventContent>
    {
        /// <summary>
        /// The content object of the event. Type and available field differ
        /// depending on the concrete type.
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(IEvent.ContentPropertyOrder)]
        public CreateRoomEventContent Content { get; set; } = new();


        [JsonPropertyName("type")]
        [JsonPropertyOrder(-30)]
        public override string Type
        {
            get => "m.room.create";
            set
            {
                if (value != "m.room.create")
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be 'm.room.create'.");
                }
            }
        }
    }


    /// <summary>
    /// Event content for CreateRoomEvent.
    /// </summary>
    public class CreateRoomEventContent : IEventContent
    {
        /// <summary>
        /// The user id of the creator of the room, e.g. "@alice:matrix.example.org".
        /// </summary>
        [JsonPropertyName("creator")]
        public string Creator { get; set; } = null!;


        /// <summary>
        /// Whether users on other servers can join the room.
        /// </summary>
        [JsonPropertyName("m.federate")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Federate { get; set; } = null;


        /// <summary>
        /// A reference to the previous room, if the previous room was upgraded.
        /// </summary>
        [JsonPropertyName("predecessor")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PreviousRoom? Predecessor { get; set; }


        /// <summary>
        /// The version of the room. Defaults to "1", if not set.
        /// </summary>
        [JsonPropertyName("room_version")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Version { get; set; } = null;
    }


    /// <summary>
    /// Information about the previous room before the version upgrade.
    /// </summary>
    public class PreviousRoom
    {
        /// <summary>
        /// Id of the last known event in the old room.
        /// </summary>
        [JsonPropertyName("event_id")]
        public string EventId { get; set; } = null!;

        /// <summary>
        /// The id of the old room.
        /// </summary>
        [JsonPropertyName("room_id")]
        public string RoomId { get; set; } = null!;
    }
}
