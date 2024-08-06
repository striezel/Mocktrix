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

using Mocktrix.Data;

namespace Mocktrix.Database.Memory
{
    using State = Dictionary<StateDictionaryKey, string>;


    /// <summary>
    /// In-memory implementation of room state database.
    /// </summary>
    public static class RoomStates
    {
        /// <summary>
        /// in-memory room states
        /// </summary>
        private static readonly List<RoomState> states = [];


        /// <summary>
        /// Creates and adds a new room state.
        /// </summary>
        /// <param name="room_id">the room id, e.g. "!myRoom:example.com"</param>
        /// <param name="state">the state for the room</param>
        /// <returns>Returns the created room state.</returns>
        public static RoomState Create(string room_id, State state)
        {
            RoomState the_state = new(room_id, state ?? []);
            states.Add(the_state);
            return the_state;
        }


        /// <summary>
        /// Gets an existing room state.
        /// </summary>
        /// <param name="room_id">the room id, e.g. "!myRoom:example.com"</param>
        /// <returns>Returns a state for the room, if it exists.
        /// Returns null, if no match was found.</returns>
        public static RoomState? Get(string room_id)
        {
            return states.Find(s => s.RoomId == room_id);
        }
    }
}
