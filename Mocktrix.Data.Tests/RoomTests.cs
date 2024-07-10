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
    public class RoomTests
    {
        [Fact]
        public void Constructor()
        {
            Room room = new("!roomyRoom:matrix.example.org", "@alice:matrix.example.com", "1", true);

            Assert.NotNull(room);
            Assert.Equal("!roomyRoom:matrix.example.org", room.RoomId);
            Assert.Equal("@alice:matrix.example.com", room.Creator);
            Assert.Equal("1", room.Version);
            Assert.True(room.Public);
            Assert.Null(room.Name);
            Assert.Null(room.Topic);
        }
    }
}