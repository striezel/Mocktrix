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
    public class TagsTests
    {
        [Fact]
        public void CreateTag()
        {
            const string user_id = "@alice:matrix.example.com";
            const string room_id = "!testRoom:matrix.example.com";
            const string tag_name = "u.nice_test";

            var tag = Tags.Create(user_id, room_id, tag_name, 0.75);

            Assert.NotNull(tag);
            Assert.Equal(user_id, tag.UserId);
            Assert.Equal(room_id, tag.RoomId);
            Assert.Equal(tag_name, tag.Name);
            Assert.Equal(0.75, tag.Order);
        }

        [Fact]
        public void CreateTag_NullOrder()
        {
            const string user_id = "@alice:matrix.example.com";
            const string room_id = "!testRoom:matrix.example.com";
            const string tag_name = "u.nice_test_2";

            var tag = Tags.Create(user_id, room_id, tag_name, null);

            Assert.NotNull(tag);
            Assert.Equal(user_id, tag.UserId);
            Assert.Equal(room_id, tag.RoomId);
            Assert.Equal(tag_name, tag.Name);
            Assert.Null(tag.Order);
        }


        [Fact]
        public void DeleteTag_NonExistentTagNotFound()
        {
            const string user_id = "@alice:matrix.example.com";
            const string room_id = "!some_test_room_for_tags:matrix.example.com";
            const string tag_name = "u.delete_test";

            _ = Tags.Create(user_id, room_id, tag_name, 0.234);

            {
                const string not_room_id = "!not_here:matrix.example.com";
                // Room does not exist, function shall return zero.
                Assert.Equal(0, Tags.DeleteTag(user_id, not_room_id, tag_name));
            }

            {
                const string not_tag_name = "u.not_delete_test";
                // Name does not exist, function shall return zero.
                Assert.Equal(0, Tags.DeleteTag(user_id, room_id, not_tag_name));
            }

            {
                const string not_user_id = "@not_alice:matrix.example.com";
                // User id does not exist, function shall return zero.
                Assert.Equal(0, Tags.DeleteTag(not_user_id, room_id, tag_name));
            }
        }


        [Fact]
        public void DeleteTag_ExistentTag()
        {
            const string user_id = "@bob:matrix.example.com";
            const string room_id = "!existing_room:matrix.example.com";
            const string tag_name = "u.yeah_delete";
            // Create a tag.
            _ = Tags.Create(user_id, room_id, tag_name, 0.234);
            // Tag exists, so one tag is deleted by the method.
            Assert.Equal(1, Tags.DeleteTag(user_id, room_id, tag_name));
        }


        [Fact]
        public void GetAllRoomTags_NonExistentTagNotFound()
        {
            const string user_id = "@alice:matrix.example.com";
            const string room_id = "!not_here:matrix.example.com";
            var data = Tags.GetAllRoomTags(user_id, room_id);

            // Room (and thus tag) does not exist, function shall return empty list.
            Assert.NotNull(data);
            Assert.Empty(data);
        }

        [Fact]
        public void GetAllRoomTags_ExistingTagsFound()
        {
            const string user_id = "@alice:matrix.example.com";
            const string room_id = "!test_room_42:matrix.example.com";
            const string name_one = "u.whatever";
            const string name_two = "u.test_more";

            Tags.Create(user_id, room_id, name_one, 0.05);
            Tags.Create(user_id, room_id, name_two, 0.25);

            var data = Tags.GetAllRoomTags(user_id, room_id);

            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
            var one = data.Find(x => x.Name == name_one);
            Assert.NotNull(one);
            Assert.Equal(user_id, one.UserId);
            Assert.Equal(room_id, one.RoomId);
            Assert.Equal(0.05, one.Order);
            var two = data.Find(x => x.Name == name_two);
            Assert.NotNull(two);
            Assert.Equal(user_id, two.UserId);
            Assert.Equal(room_id, two.RoomId);
            Assert.Equal(0.25, two.Order);
        }

        [Fact]
        public void GetAllRoomTags_OnlyTagsOfGivenUserAndRoomAreFound()
        {
            const string room_one_id = "!test_room_t_01:matrix.example.com";
            const string name_one = "u.test_01";
            const string name_two = "u.test_02";
            const string room_two_id = "!test_room_t_02:matrix.example.com";
            const string name_three = "u.test_03";
            const string name_four = "u.test_04";
            const string user_id = "@alice:matrix.example.com";
            const string other_user_id = "@not_alice:matrix.example.com";

            Tags.Create(user_id, room_one_id, name_one, 0.234);
            Tags.Create(user_id, room_one_id, name_two, 0.567);
            Tags.Create(other_user_id, room_one_id, name_one, 0.999);
            Tags.Create(user_id, room_two_id, name_three, 0.891);
            Tags.Create(user_id, room_two_id, name_four, 0.012);
            Tags.Create(other_user_id, room_two_id, name_three, 0.999);

            // room 1
            {
                var data = Tags.GetAllRoomTags(user_id, room_one_id);

                Assert.NotNull(data);
                Assert.Equal(2, data.Count);
                var one = data.Find(x => x.Name == name_one);
                Assert.NotNull(one);
                Assert.Equal(user_id, one.UserId);
                Assert.Equal(room_one_id, one.RoomId);
                Assert.Equal(0.234, one.Order);
                var two = data.Find(x => x.Name == name_two);
                Assert.NotNull(two);
                Assert.Equal(user_id, two.UserId);
                Assert.Equal(room_one_id, two.RoomId);
                Assert.Equal(0.567, two.Order);
            }

            // room 2
            {
                var data = Tags.GetAllRoomTags(user_id, room_two_id);

                Assert.NotNull(data);
                Assert.Equal(2, data.Count);
                var one = data.Find(x => x.Name == name_three);
                Assert.NotNull(one);
                Assert.Equal(user_id, one.UserId);
                Assert.Equal(room_two_id, one.RoomId);
                Assert.Equal(0.891, one.Order);
                var two = data.Find(x => x.Name == name_four);
                Assert.NotNull(two);
                Assert.Equal(user_id, two.UserId);
                Assert.Equal(room_two_id, two.RoomId);
                Assert.Equal(0.012, two.Order);
            }
        }
    }
}
