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
    public class RoomsStatesTests
    {
        [Fact]
        public void CreateRoomState()
        {
            const string room_id = "!testRoom:matrix.example.com";
            var state = RoomStates.Create(room_id, []);

            Assert.NotNull(state);
            Assert.Equal(room_id, state.RoomId);
            Assert.Empty(state.State);
        }


        [Fact]
        public void Get_NonExistentRoomNotFound()
        {
            const string room_id = "!not_here:matrix.example.com";
            var state = RoomStates.Get(room_id);

            // Room does not exist, function shall return null.
            Assert.Null(state);
        }


        [Fact]
        public void Get_ExistentRoom()
        {
            const string room_id = "!existing_room:matrix.example.com";
            // Create a room state object.
            var state = RoomStates.Create(room_id, []);
            // Query the created state.
            var queried_state = RoomStates.Get(room_id);
            Assert.NotNull(queried_state);
            // Values of created state and queried state must match.
            Assert.Equal(state.RoomId, queried_state.RoomId);
            Assert.Equal(state.State, queried_state.State);
            // As a special property of this implementation, both objects refer
            // to the same instance.
            Assert.True(ReferenceEquals(state, queried_state));
        }
    }
}
