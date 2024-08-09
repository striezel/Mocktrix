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
    public class TagTests
    {
        [Fact]
        public void Constructor()
        {
            Tag tag = new("@alice:matrix.example.org", "!roomyRoom:matrix.example.org", "u.yeah", 0.5);

            Assert.NotNull(tag);
            Assert.Equal("@alice:matrix.example.org", tag.UserId);
            Assert.Equal("!roomyRoom:matrix.example.org", tag.RoomId);
            Assert.Equal("u.yeah", tag.Name);
            Assert.Equal(0.5, tag.Order);
        }

        [Fact]
        public void Constructor_NullOrder()
        {
            Tag tag = new("@alice:matrix.example.org", "!roomyRoom:matrix.example.org", "u.yeah", null);

            Assert.NotNull(tag);
            Assert.Equal("@alice:matrix.example.org", tag.UserId);
            Assert.Equal("!roomyRoom:matrix.example.org", tag.RoomId);
            Assert.Equal("u.yeah", tag.Name);
            Assert.Null(tag.Order);
        }
    }
}
