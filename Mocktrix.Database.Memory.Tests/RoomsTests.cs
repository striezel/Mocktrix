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
    public class RoomsTests
    {
        [Fact]
        public void CreateRoom()
        {
            const string room_id = "!testRoom:matrix.example.com";
            const string user_id = "@alice:matrix.example.com";
            var room = Rooms.Create(room_id, user_id, "1", false);

            Assert.NotNull(room);
            Assert.Equal(room_id, room.RoomId);
            Assert.Equal(user_id, room.Creator);
            Assert.Equal("1", room.Version);
            Assert.False(room.Public);
            Assert.Null(room.Name);
            Assert.Null(room.Topic);
            Assert.Null(room.JoinRule);
            Assert.Null(room.HistoryVisibility);
            Assert.Null(room.GuestAccess);
        }


        [Fact]
        public void GetRoom_NonExistentRoomNotFound()
        {
            const string room_id = "!not_here:matrix.example.com";
            var room = Rooms.GetRoom(room_id);

            // Room does not exist, function shall return null.
            Assert.Null(room);
        }


        [Fact]
        public void GetRoom_ExistentRoom()
        {
            const string room_id = "!existing_room:matrix.example.com";
            const string user_id = "@bob:matrix.example.com";
            // Create a room.
            var room_of_bob = Rooms.Create(room_id, user_id, "1", false);
            // Query the created room by id.
            var room = Rooms.GetRoom(room_id);
            Assert.NotNull(room);
            // Values of created room and queried room must match.
            Assert.Equal(room_of_bob.RoomId, room.RoomId);
            Assert.Equal(room_of_bob.Creator, room.Creator);
            Assert.Equal(room_of_bob.Version, room.Version);
            Assert.False(room.Public);
            Assert.Equal(room_of_bob.Name, room.Name);
            Assert.Equal(room_of_bob.Topic, room.Topic);
            Assert.Equal(room_of_bob.JoinRule, room.JoinRule);
            Assert.Equal(room_of_bob.HistoryVisibility, room.HistoryVisibility);
            Assert.Equal(room_of_bob.GuestAccess, room.GuestAccess);
            // As a special property of this implementation, both rooms refer to
            // the same instance.
            Assert.True(ReferenceEquals(room, room_of_bob));
        }
    }
}