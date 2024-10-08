﻿/*
    This file is part of Mocktrix.
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

namespace Mocktrix
{
    /// <summary>
    /// Handles mock data for test cases.
    /// </summary>
    public class MockData
    {
        /// <summary>
        /// Adds data for use in tests.
        /// </summary>
        public static void Add()
        {
            // User "alice" on homeserver at example domain.
            var alice = Database.Memory.Users.CreateUser("@alice:matrix.example.org", "secret password");
            _ = Database.Memory.Devices.CreateDevice("AliceDeviceId", alice.user_id, "Alice's Matrix-enabled comm badge");

            // User "alice" for domain name of the server.
            var base_address = new Uri("http://localhost:5289");
            var alice_local = Database.Memory.Users.CreateUser("@alice:" + base_address.Host, "Alice's secret password");
            _ = Database.Memory.Devices.CreateDevice("AliceDeviceId", alice_local.user_id, "Alice's Matrix-enabled comm badge");

            // User for test of logging out all access tokens of a user at once.
            _ = Database.Memory.Users.CreateUser("@all_alice:matrix.example.org", "my secret password");

            AddProfileTestData(base_address);

            // User for password change tests.
            _ = Database.Memory.Users.CreateUser("@password_change:" + base_address.Host, "the old password");

            // Users for account deactivation tests.
            var inactive_user = Database.Memory.Users.CreateUser("@inactive:" + base_address.Host, "some password");
            inactive_user.inactive = true;

            _ = Database.Memory.Users.CreateUser("@deactivatable:" + base_address.Host, "silly password");

            // Add content for use in repository tests.
            _ = ContentRepository.Memory.Media.Create("testDownload", "Hello, test code. :)"u8.ToArray(), "text/plain", "hello.txt");

            AddRoomData(base_address);
            AddTagData(base_address);
        }

        private static void AddProfileTestData(Uri base_address)
        {
            // Users for display name testing.
            var display_name_user = Database.Memory.Users.CreateUser("@unnamed_user:" + base_address.Host, "bad password");
            display_name_user.display_name = null;

            var user_with_name = Database.Memory.Users.CreateUser("@named_user:" + base_address.Host, "some password");
            user_with_name.display_name = "Nomen Nominandum";

            var name_change_user = Database.Memory.Users.CreateUser("@name_change_user:" + base_address.Host, "some password");
            name_change_user.display_name = "No Name";

            // Users for avatar URL testing.
            var no_avatar = Database.Memory.Users.CreateUser("@no_avatar:" + base_address.Host, "bad password");
            no_avatar.avatar_url = null;

            var user_with_avatar = Database.Memory.Users.CreateUser("@avatar_user:" + base_address.Host, "some password");
            user_with_avatar.avatar_url = "mxc://matrix.org/FooBar";

            var avatar_change_user = Database.Memory.Users.CreateUser("@avatar_change_user:" + base_address.Host, "some password");
            avatar_change_user.avatar_url = "mxc://matrix.org/SomeOpaqueIdentifier";

            // User for profile testing.
            var profile_user = Database.Memory.Users.CreateUser("@profile:" + base_address.Host, "don't use this password");
            profile_user.avatar_url = "mxc://matrix.org/DifferentMediaId";
            profile_user.display_name = "Profiler";
        }

        private static void AddRoomData(Uri base_address)
        {
            // User and room memberships for test of joined rooms.
            var joined_user = Database.Memory.Users.CreateUser("@joined_user:" + base_address.Host, "the password");
            Database.Memory.RoomMemberships.Create("!first_joined_room:matrix.example.org", joined_user.user_id, Enums.Membership.Join);
            Database.Memory.RoomMemberships.Create("!second_joined_room:matrix.example.org", joined_user.user_id, Enums.Membership.Join);
            Database.Memory.RoomMemberships.Create("!banned_room:matrix.example.org", joined_user.user_id, Enums.Membership.Ban);
            Database.Memory.RoomMemberships.Create("!invited_room:matrix.example.org", joined_user.user_id, Enums.Membership.Invite);
            Database.Memory.RoomMemberships.Create("!knock_room:matrix.example.org", joined_user.user_id, Enums.Membership.Knock);
            Database.Memory.RoomMemberships.Create("!left_room:matrix.example.org", joined_user.user_id, Enums.Membership.Leave);

            _ = Database.Memory.Users.CreateUser("@not_a_joined_user:" + base_address.Host, "some password");

            // Room visibility tests.
            _ = Database.Memory.Rooms.Create("!public_test_room:matrix.example.org", "@tester:matrix.example.org", "1", true);
            _ = Database.Memory.Rooms.Create("!private_test_room:matrix.example.org", "@tester:matrix.example.org", "1", false);
        }

        private static void AddTagData(Uri base_address)
        {
            // User and rooms and tags for test of tagged rooms.
            string tag_user_id = "@tag_user:" + base_address.Host;
            var tag_user = Database.Memory.Users.CreateUser(tag_user_id, "secret password");

            const string room_without_tags_id = "!room_without_tags:matrix.example.org";
            _ = Database.Memory.Rooms.Create(room_without_tags_id, tag_user_id, "1", false);
            Database.Memory.RoomMemberships.Create(room_without_tags_id, tag_user.user_id, Enums.Membership.Join);

            const string room_with_some_tags_id = "!room_with_some_tags:matrix.example.org";
            _ = Database.Memory.Rooms.Create(room_with_some_tags_id, tag_user_id, "1", false);
            Database.Memory.RoomMemberships.Create(room_with_some_tags_id, tag_user.user_id, Enums.Membership.Join);

            Database.Memory.Tags.Create(tag_user_id, room_with_some_tags_id, "m.favourite", 0.25);
            Database.Memory.Tags.Create(tag_user_id, room_with_some_tags_id, "u.null", null);
            Database.Memory.Tags.Create(tag_user_id, room_with_some_tags_id, "u.some_tag", 1.0);

            // Data for tag deletion tests.
            const string room_with_tag_to_delete_id = "!room_with_tag_to_delete:matrix.example.org";
            _ = Database.Memory.Rooms.Create(room_with_tag_to_delete_id, tag_user_id, "1", false);
            Database.Memory.RoomMemberships.Create(room_with_tag_to_delete_id, tag_user.user_id, Enums.Membership.Join);

            Database.Memory.Tags.Create(tag_user_id, room_with_tag_to_delete_id, "u.delete_me", 0.5);
            Database.Memory.Tags.Create(tag_user_id, room_with_tag_to_delete_id, "u.keep_me", 0.25);

            // Data for room where tags are added.
            const string room_to_add_tag_to_id = "!room_to_add_tag_to:matrix.example.org";
            _ = Database.Memory.Rooms.Create(room_to_add_tag_to_id, tag_user_id, "1", false);
            Database.Memory.RoomMemberships.Create(room_to_add_tag_to_id, tag_user.user_id, Enums.Membership.Join);

            const string room_to_change_tag_id = "!room_to_change_tag:matrix.example.org";
            _ = Database.Memory.Rooms.Create(room_to_change_tag_id, tag_user_id, "1", false);
            Database.Memory.RoomMemberships.Create(room_to_change_tag_id, tag_user.user_id, Enums.Membership.Join);
            Database.Memory.Tags.Create(tag_user_id, room_to_change_tag_id, "u.existing", 0.5);
        }
    }
}
