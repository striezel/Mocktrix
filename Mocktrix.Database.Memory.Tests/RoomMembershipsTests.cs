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
    public class RoomsMembershipsTests
    {
        [Fact]
        public void CreateRoomMembership()
        {
            const string room_id = "!testRoom:matrix.example.com";
            const string user_id = "@alice:matrix.example.com";
            var room = RoomMemberships.Create(room_id, user_id, Enums.Membership.Join);

            Assert.NotNull(room);
            Assert.Equal(room_id, room.RoomId);
            Assert.Equal(user_id, room.UserId);
            Assert.Equal(Enums.Membership.Join, room.Membership);
        }


        [Fact]
        public void GetRoomMembership_NonExistentMembershipNotFound()
        {
            const string room_id = "!not_here:matrix.example.com";
            const string user_id = "@alice:matrix.example.com";
            var room = RoomMemberships.GetRoomMembership(room_id, user_id);

            // Room membership does not exist, function shall return null.
            Assert.Null(room);
        }


        [Fact]
        public void GetRoomMembership_ExistentMembership()
        {
            const string room_id = "!existing_room:matrix.example.com";
            const string user_id = "@bob:matrix.example.com";
            // Create a room membership.
            var membership_of_bob = RoomMemberships.Create(room_id, user_id, Enums.Membership.Leave);
            // Query the created membership by ids.
            var membership = RoomMemberships.GetRoomMembership(room_id, user_id);
            Assert.NotNull(membership);
            // Values of created membership and queried membership must match.
            Assert.Equal(membership_of_bob.RoomId, membership.RoomId);
            Assert.Equal(membership_of_bob.UserId, membership.UserId);
            Assert.Equal(membership_of_bob.Membership, membership.Membership);
            // As a special property of this implementation, both objects refer
            // to the same instance.
            Assert.True(ReferenceEquals(membership, membership_of_bob));
        }


        [Fact]
        public void GetAllRoomMembers_NonExistentMembershipNotFound()
        {
            const string room_id = "!not_here:matrix.example.com";
            var data = RoomMemberships.GetAllRoomMembers(room_id);

            // Room (and thus room membership) does not exist, function shall return empty list.
            Assert.NotNull(data);
            Assert.Empty(data);
        }

        [Fact]
        public void GetAllRoomMembers_ExistingMembershipsFound()
        {
            const string room_id = "!test_room_01:matrix.example.com";
            const string alice_id = "@alice:matrix.example.com";
            const string bob_id = "@bob:matrix.example.com";

            RoomMemberships.Create(room_id, alice_id, Enums.Membership.Join);
            RoomMemberships.Create(room_id, bob_id, Enums.Membership.Invite);

            var data = RoomMemberships.GetAllRoomMembers(room_id);

            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
            var alice = data.Find(x => x.RoomId == room_id && x.UserId == alice_id);
            Assert.NotNull(alice);
            Assert.Equal(Enums.Membership.Join, alice.Membership);
            var bob = data.Find(x => x.RoomId == room_id && x.UserId == bob_id);
            Assert.NotNull(bob);
            Assert.Equal(Enums.Membership.Invite, bob.Membership);
        }

        [Fact]
        public void GetAllRoomMembers_OnlyMembershipsOfGivenRoomAreFound()
        {
            const string room_one_id = "!test_room_m_01:matrix.example.com";
            const string room_two_id = "!test_room_m_02:matrix.example.com";
            const string alice_id = "@alice:matrix.example.com";
            const string bob_id = "@bob:matrix.example.com";

            RoomMemberships.Create(room_one_id, alice_id, Enums.Membership.Join);
            RoomMemberships.Create(room_one_id, bob_id, Enums.Membership.Invite);
            RoomMemberships.Create(room_two_id, alice_id, Enums.Membership.Leave);
            RoomMemberships.Create(room_two_id, bob_id, Enums.Membership.Join);

            // room 1
            {
                var data = RoomMemberships.GetAllRoomMembers(room_one_id);

                Assert.NotNull(data);
                Assert.Equal(2, data.Count);
                var alice = data.Find(x => x.RoomId == room_one_id && x.UserId == alice_id);
                Assert.NotNull(alice);
                Assert.Equal(Enums.Membership.Join, alice.Membership);
                var bob = data.Find(x => x.RoomId == room_one_id && x.UserId == bob_id);
                Assert.NotNull(bob);
                Assert.Equal(Enums.Membership.Invite, bob.Membership);
            }

            // room 2
            {
                var data = RoomMemberships.GetAllRoomMembers(room_two_id);

                Assert.NotNull(data);
                Assert.Equal(2, data.Count);
                var alice = data.Find(x => x.RoomId == room_two_id && x.UserId == alice_id);
                Assert.NotNull(alice);
                Assert.Equal(Enums.Membership.Leave, alice.Membership);
                var bob = data.Find(x => x.RoomId == room_two_id && x.UserId == bob_id);
                Assert.NotNull(bob);
                Assert.Equal(Enums.Membership.Join, bob.Membership);
            }
        }

        [Fact]
        public void GetAllMembershipsOfUser_NonExistentMembershipNotFound()
        {
            const string user_id = "@non-existent_user:matrix.example.com";
            var data = RoomMemberships.GetAllMembershipsOfUser(user_id);

            // Room (and thus room membership) does not exist, function shall return empty list.
            Assert.NotNull(data);
            Assert.Empty(data);
        }

        [Fact]
        public void GetAllMembershipsOfUser_ExistingMembershipsFound()
        {
            const string room_one_id = "!test_room_m_03:matrix.example.com";
            const string room_two_id = "!test_room_m_04:matrix.example.com";
            const string user_id = "@charlie:matrix.example.com";

            RoomMemberships.Create(room_one_id, user_id, Enums.Membership.Join);
            RoomMemberships.Create(room_two_id, user_id, Enums.Membership.Invite);

            var data = RoomMemberships.GetAllMembershipsOfUser(user_id);

            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
            var one = data.Find(x => x.RoomId == room_one_id && x.UserId == user_id);
            Assert.NotNull(one);
            Assert.Equal(Enums.Membership.Join, one.Membership);
            var two = data.Find(x => x.RoomId == room_two_id && x.UserId == user_id);
            Assert.NotNull(two);
            Assert.Equal(Enums.Membership.Invite, two.Membership);
        }

        [Fact]
        public void GetAllMembershipsOfUser_OnlyMembershipsOfGivenUserAreFound()
        {
            const string room_one_id = "!test_room_m_05:matrix.example.com";
            const string room_two_id = "!test_room_m_06:matrix.example.com";
            const string dora_id = "@dora:matrix.example.com";
            const string eli_id = "@elias:matrix.example.com";

            RoomMemberships.Create(room_one_id, dora_id, Enums.Membership.Join);
            RoomMemberships.Create(room_one_id, eli_id, Enums.Membership.Invite);
            RoomMemberships.Create(room_two_id, dora_id, Enums.Membership.Leave);
            RoomMemberships.Create(room_two_id, eli_id, Enums.Membership.Join);

            // Dora
            {
                var data = RoomMemberships.GetAllMembershipsOfUser(dora_id);

                Assert.NotNull(data);
                Assert.Equal(2, data.Count);
                var one = data.Find(x => x.RoomId == room_one_id && x.UserId == dora_id);
                Assert.NotNull(one);
                Assert.Equal(Enums.Membership.Join, one.Membership);
                var two = data.Find(x => x.RoomId == room_two_id && x.UserId == dora_id);
                Assert.NotNull(two);
                Assert.Equal(Enums.Membership.Leave, two.Membership);
            }

            // Elias
            {
                var data = RoomMemberships.GetAllMembershipsOfUser(eli_id);

                Assert.NotNull(data);
                Assert.Equal(2, data.Count);
                var one = data.Find(x => x.RoomId == room_one_id && x.UserId == eli_id);
                Assert.NotNull(one);
                Assert.Equal(Enums.Membership.Invite, one.Membership);
                var two = data.Find(x => x.RoomId == room_two_id && x.UserId == eli_id);
                Assert.NotNull(two);
                Assert.Equal(Enums.Membership.Join, two.Membership);
            }
        }
    }
}
