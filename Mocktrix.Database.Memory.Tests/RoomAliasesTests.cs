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
    public class RoomsAliasesTests
    {
        [Fact]
        public void CreateRoomAlias()
        {
            const string room_id = "!testRoom:matrix.example.com";
            const string alias_id = "#nice_room:matrix.example.com";
            const string creator_id = "@alice:matrix.example.com";
            var alias = RoomAliases.Create(room_id, alias_id, creator_id);

            Assert.NotNull(alias);
            Assert.Equal(room_id, alias.RoomId);
            Assert.Equal(alias_id, alias.Alias);
            Assert.Equal(creator_id, alias.Creator);
        }


        [Fact]
        public void GetAlias_NonExistentAliasNotFound()
        {
            const string alias_id = "#not_here:matrix.example.com";
            var alias = RoomAliases.GetAlias(alias_id);

            // Alias does not exist, function shall return null.
            Assert.Null(alias);
        }


        [Fact]
        public void GetAlias_ExistentAlias()
        {
            const string room_id = "!existing_room:matrix.example.com";
            const string alias_id = "#test_alias:matrix.example.com";
            const string user_id = "@bob:matrix.example.com";
            // Create a room alias.
            var alias_of_bob = RoomAliases.Create(room_id, alias_id, user_id);
            // Query the created alias.
            var alias = RoomAliases.GetAlias(alias_id);
            Assert.NotNull(alias);
            // Values of created alias and queried alias must match.
            Assert.Equal(alias_of_bob.RoomId, alias.RoomId);
            Assert.Equal(alias_of_bob.Alias, alias.Alias);
            Assert.Equal(alias_of_bob.Creator, alias.Creator);
            // As a special property of this implementation, both objects refer
            // to the same instance.
            Assert.True(ReferenceEquals(alias, alias_of_bob));
        }


        [Fact]
        public void GetAllRoomAliases_NonExistentAliasNotFound()
        {
            const string room_id = "!not_here:matrix.example.com";
            var data = RoomAliases.GetAllRoomAliases(room_id);

            // Room (and thus room alias) does not exist, function shall return empty list.
            Assert.NotNull(data);
            Assert.Empty(data);
        }

        [Fact]
        public void GetAllRoomAliases_ExistingAliasesFound()
        {
            const string room_id = "!test_room_01:matrix.example.com";
            const string alias_one_id = "#alice_was_here:matrix.example.com";
            const string alias_two_id = "#bob_was_here:matrix.example.com";
            const string user_id = "@alice:matrix.example.com";

            RoomAliases.Create(room_id, alias_one_id, user_id);
            RoomAliases.Create(room_id, alias_two_id, user_id);

            var data = RoomAliases.GetAllRoomAliases(room_id);

            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
            var alice = data.Find(x => x.Alias == alias_one_id);
            Assert.NotNull(alice);
            Assert.Equal(room_id, alice.RoomId);
            Assert.Equal(user_id, alice.Creator);
            var bob = data.Find(x => x.Alias == alias_two_id);
            Assert.NotNull(bob);
            Assert.Equal(room_id, bob.RoomId);
            Assert.Equal(user_id, bob.Creator);
        }

        [Fact]
        public void GetAllRoomAliases_OnlyAliasesOfGivenRoomAreFound()
        {
            const string room_one_id = "!test_room_a_01:matrix.example.com";
            const string alias_one_id = "#test_alias_01:matrix.example.com";
            const string alias_two_id = "#test_alias_02:matrix.example.com";
            const string room_two_id = "!test_room_a_02:matrix.example.com";
            const string alias_three_id = "#test_alias_03:matrix.example.com";
            const string alias_four_id = "#test_alias_04:matrix.example.com";
            const string user_id = "@alice:matrix.example.com";

            RoomAliases.Create(room_one_id, alias_one_id, user_id);
            RoomAliases.Create(room_one_id, alias_two_id, user_id);
            RoomAliases.Create(room_two_id, alias_three_id, user_id);
            RoomAliases.Create(room_two_id, alias_four_id, user_id);

            // room 1
            {
                var data = RoomAliases.GetAllRoomAliases(room_one_id);

                Assert.NotNull(data);
                Assert.Equal(2, data.Count);
                var one = data.Find(x => x.Alias == alias_one_id);
                Assert.NotNull(one);
                Assert.Equal(room_one_id, one.RoomId);
                Assert.Equal(user_id, one.Creator);
                var two = data.Find(x => x.Alias == alias_two_id);
                Assert.NotNull(two);
                Assert.Equal(room_one_id, two.RoomId);
                Assert.Equal(user_id, two.Creator);
            }

            // room 2
            {
                var data = RoomAliases.GetAllRoomAliases(room_two_id);

                Assert.NotNull(data);
                Assert.Equal(2, data.Count);
                var one = data.Find(x => x.Alias == alias_three_id);
                Assert.NotNull(one);
                Assert.Equal(room_two_id, one.RoomId);
                Assert.Equal(user_id, one.Creator);
                var two = data.Find(x => x.Alias == alias_four_id);
                Assert.NotNull(two);
                Assert.Equal(room_two_id, two.RoomId);
                Assert.Equal(user_id, two.Creator);
            }
        }
    }
}
