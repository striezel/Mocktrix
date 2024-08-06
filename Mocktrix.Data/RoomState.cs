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

using System.Collections.Generic;

namespace Mocktrix.Data
{
    using State = Dictionary<StateDictionaryKey, string>;


    /// <summary>
    /// Holds information about a room's state.
    /// </summary>
    /// <param name="room_id">id of the room, e.g. "!foo:matrix.example.org"</param>
    /// <param name="state">the state of the room</param>
    public class RoomState(string room_id, State state)
    {
        /// <summary>
        /// Id of the room, e.g. "!foo:matrix.example.org".
        /// </summary>
        public string RoomId { get; set; } = room_id;

        /// <summary>
        /// The state for the room, i.e. a dictionary mapping (EventType, StateKey)
        /// pairs the the corresponding event id.
        /// </summary>
        public State State { get; set; } = state;
    }
}
