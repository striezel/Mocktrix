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
    /// In-memory implementation of user database.
    /// </summary>
    public static class Users
    {
        /// <summary>
        /// in-memory user list
        /// </summary>
        private static readonly List<User> users = [];


        /// <summary>
        /// Creates and adds a new user.
        /// </summary>
        /// <param name="id">user id, e.g. "@alice:example.com"</param>
        /// <param name="password">the user's password</param>
        /// <returns>Returns the created user.</returns>
        public static User CreateUser(string id, string password)
        {
            string hash = utilities.Hashing.CreateHashedSaltedPassword(password, out byte[] salt);
            User user = new(id, hash, salt);
            users.Add(user);
            return user;
        }


        /// <summary>
        /// Gets an existing user.
        /// </summary>
        /// <param name="user_id">id of the user</param>
        /// <returns>Returns a user with the matching id, if it exists.
        /// Returns null, if no match was found.</returns>
        public static User? GetUser(string user_id)
        {
            return users.Find(u => u.user_id == user_id);
        }
    }
}
