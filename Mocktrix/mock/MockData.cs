/*
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
        }
    }
}
