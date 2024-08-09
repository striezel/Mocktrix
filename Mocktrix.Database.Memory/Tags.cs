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
    /// In-memory implementation of tag database.
    /// </summary>
    public static class Tags
    {
        /// <summary>
        /// in-memory room tag list
        /// </summary>
        private static readonly List<Tag> tags = [];


        /// <summary>
        /// Creates and adds a new tag.
        /// </summary>
        /// <param name="user_id">id of the user that set the tag, e.g. "@alice:example.com"</param>
        /// <param name="room_id">id of the tagged room, e.g. "!myRoom:example.com"</param>
        /// <param name="name">the name of the tag, e.g. "m.favourite"</param>
        /// <param name="order">relative order of the room under the tag, must be in [0;1]</param>
        /// <returns>Returns the created room alias.</returns>
        public static Tag Create(string user_id, string room_id, string name, double? order)
        {
            Tag tag = new(user_id, room_id, name, order);
            tags.Add(tag);
            return tag;
        }


        /// <summary>
        /// Deletes an existing tag.
        /// </summary>
        /// <param name="user_id">id of the user that set the tag, e.g. "@alice:example.com"</param>
        /// <param name="room_id">id of the tagged room, e.g. "!myRoom:example.com"</param>
        /// <param name="name">the name of the tag, e.g. "m.favourite"</param>
        /// <returns>Returns the number of removed tags (usually one).</returns>
        public static int DeleteTag(string user_id, string room_id, string name)
        {
            return tags.RemoveAll(t => t.UserId == user_id && t.RoomId == room_id && t.Name == name);
        }


        /// <summary>
        /// Gets all tag set by a given user for a given room.
        /// </summary>
        /// <param name="user_id">id of the user that set the tag, e.g. "@alice:example.com"</param>
        /// <param name="room_id">id of the room, e.g. "!myRoom:example.com"</param>
        /// <returns>Returns a list of aliases for the room, if they exist.
        /// Returns an empty list otherwise.</returns>
        public static List<Tag> GetAllRoomTags(string user_id, string room_id)
        {
            return tags.FindAll(t => t.UserId == user_id && t.RoomId == room_id);
        }
    }
}
