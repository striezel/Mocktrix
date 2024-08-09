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
    /// Holds information about a room tag set by a user.
    /// </summary>
    /// <param name="user_id">id of the user that created the tag, e.g. "@alice:matrix.example.org"</param>
    /// <param name="room_id">id of the room to tag, e.g. "!foo:matrix.example.org"</param>
    /// <param name="tag">the name of the tag, e.g. "m.favourite"</param>
    /// <param name="order">number in a range [0;1] describing a relative position of the room under the given tag</param>
    public class Tag(string user_id, string room_id, string tag, double? order)
    {
        /// <summary>
        /// Id of the user that created the tag, e.g. "@alice:matrix.example.org".
        /// </summary>
        public string UserId { get; set; } = user_id;


        /// <summary>
        /// Id of the room, e.g. "!foo:matrix.example.org".
        /// </summary>
        public string RoomId { get; set; } = room_id;


        /// <summary>
        /// the name of the tag, e.g. "m.favourite"
        /// </summary>
        public string Name { get; set; } = tag;


        /// <summary>
        /// number in a range [0;1] describing a relative position of the room
        /// under the given tag
        /// </summary>
        public double? Order { get; set; } = order;
    }
}
