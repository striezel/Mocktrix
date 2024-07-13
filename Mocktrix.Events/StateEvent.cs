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
    /// Abstract class that contains data members common to all state events.
    /// </summary>
    public abstract class StateEvent: RoomEvent
    {
        /// <summary>
        /// Previous content for that event, if any.
        /// </summary>
        [JsonPropertyName("prev_content")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(-70)]
        public IEventContent? PrevContent { get; set; }


        /// <summary>
        /// A unique key which defines the overwriting semantics for this piece
        /// of room state. This value is often a zero-length string. The
        /// presence of this key makes this event a State Event. State keys
        /// starting with an "@" are reserved for referencing user IDs, such as
        /// room members. With the exception of a few events, state events set
        /// with a given user's ID as the state key must only be set by that user.
        /// </summary>
        [JsonPropertyName("state_key")]
        [JsonPropertyOrder(-40)]
        public string StateKey { get; set; } = null!;
    }
}
