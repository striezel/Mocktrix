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
    /// Holds user data.
    /// </summary>
    /// <param name="id">user id, e.g. "@alice:matrix.domain.tld"</param>
    /// <param name="hashed_pass">hashed password</param>
    /// <param name="salty">the salt for password hashing</param>
    public class User(string id, string hashed_pass, byte[] salty)
    {
        /// <summary>
        /// id of the user, including homeserver domain, e.g. "@foo:matrix.example.org"
        /// </summary>
        public string user_id = id;


        /// <summary>
        /// hashed password
        /// </summary>
        public string password_hash = hashed_pass;


        /// <summary>
        /// the salt used in password hashing
        /// </summary>
        public byte[] salt = salty;


        /// <summary>
        /// the user's display name
        /// </summary>
        public string? display_name = default;


        /// <summary>
        /// the user's avatar URL as Matrix Content (MXC) URI
        /// </summary>
        public string? avatar_url = default;
    }
}
