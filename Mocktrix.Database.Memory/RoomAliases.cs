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
    /// In-memory implementation of room alias database.
    /// </summary>
    public static class RoomAliases
    {
        /// <summary>
        /// in-memory room alias list
        /// </summary>
        private static readonly List<RoomAlias> aliases = [];


        /// <summary>
        /// Creates and adds a new room alias.
        /// </summary>
        /// <param name="room_id">the room id, e.g. "!myRoom:example.com"</param>
        /// <param name="alias">the alias for the room, e.g. "#my_alias:example.com"</param>
        /// <param name="creator_user_id">user id of the creator, e.g. "@alice:example.com"</param>
        /// <returns>Returns the created room alias.</returns>
        public static RoomAlias Create(string room_id, string alias, string creator_user_id)
        {
            RoomAlias the_alias = new(room_id, alias, creator_user_id);
            aliases.Add(the_alias);
            return the_alias;
        }


        /// <summary>
        /// Gets an existing room alias.
        /// </summary>
        /// <param name="alias">alias of the room, e.g. "#myAlias:example.com"</param>
        /// <returns>Returns a room alias with the matching data, if it exists.
        /// Returns null, if no match was found.</returns>
        public static RoomAlias? GetAlias(string alias)
        {
            return aliases.Find(a => a.Alias == alias);
        }


        /// <summary>
        /// Gets all alias information for a given room.
        /// </summary>
        /// <param name="room_id">id of the room, e.g. "!myRoom:example.com"</param>
        /// <returns>Returns a list of aliases for the room, if they exist.
        /// Returns an empty list otherwise.</returns>
        public static List<RoomAlias> GetAllRoomAliases(string room_id)
        {
            return aliases.FindAll(m => m.RoomId == room_id);
        }
    }
}
