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
    /// Holds information about a Matrix room.
    /// </summary>
    /// <param name="id">id of the room, e.g. "!foo:matrix.example.org"</param>
    /// <param name="creator">id of the user that created the room, e.g. "@alice:matrix.example.org"</param>
    /// <param name="v">version of the room</param>
    /// <param name="is_public">whether the room is public</param>
    public class Room(string id, string creator, string v, bool is_public)
    {
        /// <summary>
        /// Id of the room, e.g. "!foo:matrix.example.org".
        /// </summary>
        public string RoomId { get; set; } = id;


        /// <summary>
        /// Id of the user that created the room,  e.g. "@alice:matrix.example.org".
        /// </summary>
        public string Creator { get; set; } = creator;


        /// <summary>
        /// The version of the room.
        /// </summary>
        public string Version { get; set; } = v;


        /// <summary>
        /// Determines whether the room is public.
        /// </summary>
        public bool Public { get; set; } = is_public;


        /// <summary>
        /// The name of the room, if any.
        /// </summary>
        public string? Name { get; set; } = null;


        /// <summary>
        /// The topic of this room, if any.
        /// </summary>
        public string? Topic { get; set; } = null;


        /// <summary>
        /// The canonical alias of this room, if any
        /// (e.g. "#some-alias:matrix.example.org").
        /// </summary>
        public string? CanonicalAlias { get; set; } = null;


        /// <summary>
        /// The join rule of this room, if any.
        /// </summary>
        public Enums.JoinRule? JoinRule { get; set; } = null;


        /// <summary>
        /// Current history visibility of the room, if any.
        /// </summary>
        public Enums.HistoryVisibility? HistoryVisibility { get; set; } = null;


        /// <summary>
        /// Guest access setting of the room, if any.
        /// </summary>
        public Enums.GuestAccess? GuestAccess { get; set; } = null;
    }
}
