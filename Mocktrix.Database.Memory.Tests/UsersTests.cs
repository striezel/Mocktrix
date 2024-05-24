/*
    This file is part of test suite for Mocktrix.
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

namespace Mocktrix.Database.Memory.Tests
{
    public class UsersTests
    {
        [Fact]
        public void CreateUser()
        {
            const string user_id = "@alice:matrix.example.com";
            const string password = "very secret";
            var user = Users.CreateUser(user_id, password);

            Assert.NotNull(user);
            Assert.Equal(user_id, user.user_id);
            Assert.False(string.IsNullOrWhiteSpace(user.password_hash));
            Assert.NotNull(user.salt);
            Assert.True(user.salt.Length > 20);
        }


        [Fact]
        public void GetUser_NonExistentUserNotFound()
        {
            const string user_id = "@mango-eating_mungo:matrix.example.com";
            var user = Users.GetUser(user_id);

            // User does not exist, function shall return null.
            Assert.Null(user);
        }


        [Fact]
        public void GetUser_ExistentUser()
        {
            const string user_id = "@bob:matrix.example.com";
            const string password = "secret password";
            // Create a user.
            var bob = Users.CreateUser(user_id, password);
            // Query the created user by id.
            var user = Users.GetUser(user_id);
            Assert.NotNull(user);
            // Values of created user and queried user must match.
            Assert.Equal(bob.user_id, user.user_id);
            Assert.Equal(bob.password_hash, user.password_hash);
            Assert.Equal(bob.salt, user.salt);
        }
    }
}