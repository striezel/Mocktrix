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
    /// Holds information about a user's membership in a Matrix room.
    /// </summary>
    /// <param name="room_id">id of the room, e.g. "!foo:matrix.example.org"</param>
    /// <param name="user_id">id of the user, e.g. "@alice:matrix.example.org"</param>
    /// <param name="membership">membership state of the user for that room</param>
    public class RoomMembership(string room_id, string user_id, Enums.Membership membership)
    {
        /// <summary>
        /// Id of the room, e.g. "!foo:matrix.example.org".
        /// </summary>
        public string RoomId { get; set; } = room_id;


        /// <summary>
        /// Id of the user whose membership this tracks, e.g. "@alice:matrix.example.org".
        /// </summary>
        public string UserId { get; set; } = user_id;


        /// <summary>
        /// The membership state of the user.
        /// </summary>
        public Enums.Membership Membership { get; set; } = membership;
    }
}
