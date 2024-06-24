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

namespace Mocktrix.Data.Tests
{
    public class UserTests
    {
        [Fact]
        public void Constructor()
        {
            User user = new("@alice:matrix.example.com", "affeaffeaffe0123456789", "ABCDE"u8.ToArray());

            Assert.NotNull(user);
            Assert.Equal("@alice:matrix.example.com", user.user_id);
            Assert.Equal("affeaffeaffe0123456789", user.password_hash);
            Assert.Equal([65, 66, 67, 68, 69], user.salt);
            Assert.Null(user.display_name);
            Assert.Null(user.avatar_url);
            Assert.False(user.inactive);
        }
    }
}