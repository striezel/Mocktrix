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
    public class AccessTokensTests
    {
        [Fact]
        public void CreateToken_New()
        {
            const string dev_id = "SomeRandomId";
            const string user_id = "@alice:matrix.example.com";

            var token = AccessTokens.CreateToken(user_id, dev_id);

            Assert.NotNull(token);
            Assert.Equal(dev_id, token.device_id);
            Assert.Equal(user_id, token.user_id);
            Assert.NotNull(token.token);
            Assert.Matches("^[A-Za-z0-9]+$", token.token);
        }


        [Fact]
        public void CreateDevice_OldTokenRenewal()
        {
            const string dev_id = "SomeRandomDeviceId";
            const string user_id = "@charlie:matrix.example.com";
            var old_token = AccessTokens.CreateToken(user_id, dev_id);
            string old_token_value = old_token.token;

            var new_token = AccessTokens.CreateToken(user_id, dev_id);
            Assert.Equal(dev_id, new_token.device_id);
            Assert.Equal(user_id, new_token.user_id);
            Assert.Matches("^[A-Za-z0-9]+$", new_token.token);
            Assert.NotEqual(old_token_value, new_token.token);
        }


        [Fact]
        public void FindByUserAndDevice_NonExistentTokenNotFound()
        {
            const string dev_id = "NotFoundHere";
            const string user_id = "@mango-eating_mungo:matrix.example.com";
            var token = AccessTokens.FindByUserAndDevice(user_id, dev_id);

            // Token does not exist, function shall return null.
            Assert.Null(token);
        }


        [Fact]
        public void FindByUserAndDevice_ExistentToken()
        {
            const string dev_id = "TheIdIsReallyHere";
            const string user_id = "@bob:matrix.example.com";

            // Create a token.
            var some_token = AccessTokens.CreateToken(user_id, dev_id);
            // Query the created token.
            var token = AccessTokens.FindByUserAndDevice(user_id, dev_id);
            Assert.NotNull(token);
            // Values of created token and queried token must match.
            Assert.Equal(some_token.user_id, token.user_id);
            Assert.Equal(some_token.device_id, token.device_id);
            Assert.Equal(some_token.token, token.token);
            // That particular implementation even uses the same object instance
            // for both.
            Assert.True(ReferenceEquals(some_token, token));
        }


        [Fact]
        public void FindByUserAndDevice_OneOfSeveralExistentTokens()
        {
            const string user_id = "@bob:matrix.example.com";

            // Create first token.
            var first_token = AccessTokens.CreateToken(user_id, "SomeDeviceId");

            // Create second token.
            const string dev_id = "SomeOtherDevice";
            var second_token = AccessTokens.CreateToken(user_id, dev_id);
            // Query the created token.
            var token = AccessTokens.FindByUserAndDevice(user_id, dev_id);
            Assert.NotNull(token);
            // Values must not match old token.
            Assert.NotEqual(first_token.device_id, second_token.device_id);
            Assert.NotEqual(first_token.token, second_token.token);
            // Values of second device and queried device must match.
            Assert.Equal(second_token.user_id, token.user_id);
            Assert.Equal(second_token.device_id, token.device_id);
            Assert.Equal(second_token.token, token.token);
        }


        [Fact]
        public void FindByUserAndDevice_WithWrongDeviceId()
        {
            const string user_id = "@alice:matrix.example.com";
            const string dev_id = "JustTheDeviceId";

            // Create token.
            _ = AccessTokens.CreateToken(user_id, dev_id);

            // Query the created device with wrong device id.
            const string wrong_dev_id = "NotTheDeviceId";
            var wrong_device = AccessTokens.FindByUserAndDevice(user_id, wrong_dev_id);
            // Token must be null, because users may have several different
            // devices and different tokens on different devices.
            Assert.Null(wrong_device);
        }


        [Fact]
        public void FindByUserAndDevice_WithWrongUserId()
        {
            const string alice_id = "@alice:matrix.example.com";
            const string bob_id = "@bob:matrix.example.com";
            const string dev_id = "JustSomeDeviceId";

            // Create device.
            _ = AccessTokens.CreateToken(bob_id, dev_id);

            // Query the created token with wrong user id.
            var alice_token = AccessTokens.FindByUserAndDevice(alice_id, dev_id);
            // Token must be null, because other users may have the same device
            // id (device ids can be supplied by the user) by accident. However,
            // that is then a different device.
            Assert.Null(alice_token);
        }


        [Fact]
        public void Find_NonExistentTokenNotFound()
        {
            const string access_token = "not_an_existing_token_value";
            var token = AccessTokens.Find(access_token);

            // Token does not exist, function shall return null.
            Assert.Null(token);
        }


        [Fact]
        public void Find_ExistentToken()
        {
            const string dev_id = "TheDeviceIsReallyHere";
            const string user_id = "@charlie:matrix.example.com";

            // Create a token.
            var some_token = AccessTokens.CreateToken(user_id, dev_id);
            // Query the created token.
            var token = AccessTokens.Find(some_token.token);
            Assert.NotNull(token);
            // Values of created token and queried token must match.
            Assert.Equal(some_token.user_id, token.user_id);
            Assert.Equal(some_token.device_id, token.device_id);
            Assert.Equal(some_token.token, token.token);
            // That particular implementation even uses the same object instance
            // for both.
            Assert.True(ReferenceEquals(some_token, token));
        }


        [Fact]
        public void Revoke_NonExistentTokenNotRevoke()
        {
            const string access_token = "not_an_existing_token_value";
            // Token does not exist, function shall return false.
            Assert.False(AccessTokens.Revoke(access_token));
        }


        [Fact]
        public void Revoke_ExistentToken()
        {
            const string dev_id = "RevokableDeviceHere";
            const string user_id = "@alice:matrix.example.org";

            // Create a token.
            var original_token = AccessTokens.CreateToken(user_id, dev_id);
            // Query the created token.
            var find_token = AccessTokens.Find(original_token.token);
            Assert.NotNull(find_token);
            // Revoke the token.
            Assert.True(AccessTokens.Revoke(original_token.token));
            // Token shall not be found anymore.
            Assert.Null(AccessTokens.Find(original_token.token));
        }
    }
}