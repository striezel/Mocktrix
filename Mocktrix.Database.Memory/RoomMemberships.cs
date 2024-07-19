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
    /// In-memory implementation of room membership database.
    /// </summary>
    public static class RoomMemberships
    {
        /// <summary>
        /// in-memory room membership list
        /// </summary>
        private static readonly List<RoomMembership> memberships = [];


        /// <summary>
        /// Creates and adds a new room membership.
        /// </summary>
        /// <param name="room_id">the room id, e.g. "!myRoom:example.com"</param>
        /// <param name="user_id">user id, e.g. "@alice:example.com"</param>
        /// <param name="state">the user's membership state for the room</param>
        /// <returns>Returns the created room.</returns>
        public static RoomMembership Create(string room_id, string user_id, Enums.Membership state)
        {
            RoomMembership membership = new(room_id, user_id, state);
            memberships.Add(membership);
            return membership;
        }


        /// <summary>
        /// Gets an existing room membership.
        /// </summary>
        /// <param name="room_id">id of the room, e.g. "!myRoom:example.com"</param>
        /// <param name="user_id">user id, e.g. "@alice:example.com"</param>
        /// <returns>Returns a room membership with the matching ids, if it exists.
        /// Returns null, if no match was found.</returns>
        public static RoomMembership? GetRoomMembership(string room_id, string user_id)
        {
            return memberships.Find(m => m.RoomId == room_id && m.UserId == user_id);
        }

        /// <summary>
        /// Gets all membership information for a single room.
        /// </summary>
        /// <param name="room_id">id of the room, e.g. "!myRoom:example.com"</param>
        /// <returns>Returns a list of memberships for the room, if they exist.
        /// Returns an empty list otherwise.</returns>
        public static List<RoomMembership> GetAllRoomMembers(string room_id)
        {
            return memberships.FindAll(m => m.RoomId == room_id);
        }

        /// <summary>
        /// Gets all membership information for a single user.
        /// </summary>
        /// <param name="user_id">user id to search for, e.g. "@alice:example.com"</param>
        /// <returns>Returns a list of memberships for the user, if any exist.
        /// Returns an empty list otherwise.</returns>
        public static List<RoomMembership> GetAllMembershipsOfUser(string user_id)
        {
            return memberships.FindAll(m => m.UserId == user_id);
        }
    }
}
