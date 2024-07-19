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
    public class RoomMembershipTests
    {
        [Fact]
        public void Constructor()
        {
            RoomMembership membership = new("!roomyRoom:matrix.example.org", "@alice:matrix.example.com", Enums.Membership.Invite);

            Assert.NotNull(membership);
            Assert.Equal("!roomyRoom:matrix.example.org", membership.RoomId);
            Assert.Equal("@alice:matrix.example.com", membership.UserId);
            Assert.Equal(Enums.Membership.Invite, membership.Membership);
        }
    }
}
