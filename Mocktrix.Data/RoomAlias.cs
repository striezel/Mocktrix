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

namespace Mocktrix.Data
{
    /// <summary>
    /// Holds information about a room alias.
    /// </summary>
    /// <param name="room_id">id of the room, e.g. "!foo:matrix.example.org"</param>
    /// <param name="alias">the alias of the room, e.g. "#alias:matrix.example.org"</param>
    /// <param name="user_id">id of the user that created the alias, e.g. "@alice:matrix.example.org"</param>
    public class RoomAlias(string room_id, string alias, string user_id)
    {
        /// <summary>
        /// Id of the room, e.g. "!foo:matrix.example.org".
        /// </summary>
        public string RoomId { get; set; } = room_id;

        /// <summary>
        /// The alias for the room, e.g. "#alias:matrix.example.org".
        /// </summary>
        public string Alias { get; set; } = alias;


        /// <summary>
        /// Id of the user that created the alias, e.g. "@alice:matrix.example.org".
        /// </summary>
        public string Creator { get; set; } = user_id;
    }
}
