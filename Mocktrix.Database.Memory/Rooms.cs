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
    /// <summary>
    /// In-memory implementation of room database.
    /// </summary>
    public static class Rooms
    {
        /// <summary>
        /// in-memory room list
        /// </summary>
        private static readonly List<Room> rooms = [];


        /// <summary>
        /// Creates and adds a new room.
        /// </summary>
        /// <param name="id">the room id, e.g. "!myRoom:example.com"</param>
        /// <param name="creator">user id of the creator, e.g. "@alice:example.com"</param>
        /// <param name="version">the room version</param>
        /// <param name="is_public">whether the room is public</param>
        /// <returns>Returns the created room.</returns>
        public static Room Create(string id, string creator, string version, bool is_public)
        {
            Room room = new(id, creator, version, is_public);
            rooms.Add(room);
            return room;
        }


        /// <summary>
        /// Gets an existing room.
        /// </summary>
        /// <param name="room_id">id of the room</param>
        /// <returns>Returns a room with the matching id, if it exists.
        /// Returns null, if no match was found.</returns>
        public static Room? GetRoom(string room_id)
        {
            return rooms.Find(r => r.RoomId == room_id);
        }
    }
}
