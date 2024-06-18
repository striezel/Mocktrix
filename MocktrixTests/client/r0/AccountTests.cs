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

using System.Net;
using System.Net.Http.Json;

namespace MocktrixTests
{
    public class AccountTests
    {
        private readonly HttpClient client = new()
        {
            BaseAddress = Utilities.BaseAddress
        };

        [Fact]
        public async Task TestMailAccountPasswordToken()
        {
            var postData = new
            {
                client_secret = "not so secret",
                email = "alice@example.org",
                send_attempt = 1,
                next_link = "https://example.org/congratulations.html",
                id_server = "id.example.com",
                id_access_token = "some_string"
            };

            var response = await client.PostAsync("/_matrix/client/r0/account/password/email/requestToken", JsonContent.Create(postData));

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_THREEPID_DENIED",
                error = "Third-party identifier is not allowed here."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestPhoneAccountPasswordToken()
        {
            var postData = new
            {
                client_secret = "not so secret",
                country = "GB",
                phone_number = "07700900001",
                send_attempt = 1,
                next_link = "https://example.org/congratulations.html",
                id_server = "id.example.com",
                id_access_token = "some_string"
            };

            var response = await client.PostAsync("/_matrix/client/r0/account/password/msisdn/requestToken", JsonContent.Create(postData));

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_THREEPID_DENIED",
                error = "Third-party identifier is not allowed here."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestAccountThreePid_NoAuthorization()
        {
            var response = await client.GetAsync("/_matrix/client/r0/account/3pid");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_MISSING_TOKEN",
                error = "Missing access token."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestAccountThreePid_InvalidAccessToken()
        {
            HttpClient unauthenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            unauthenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer foobar");
            var response = await unauthenticated_client.GetAsync("/_matrix/client/r0/account/3pid");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_UNKNOWN_TOKEN",
                error = "Unrecognized access token."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestAccountThreePid_WithAuthorization()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var body = new
            {
                type = "m.login.password",
                identifier = new
                {
                    type = "m.id.user",
                    user = "@alice:matrix.example.org"
                },
                password = "secret password",
                initial_device_display_name = "My device"
            };
            var login_response = await client.PostAsync("/_matrix/client/r0/login", JsonContent.Create(body));
            var login_data = new
            {
                user_id = "@alice:matrix.example.org",
                access_token = "random ...",
                device_id = "also random ..."
            };
            var login_content = Utilities.GetContent(login_response, login_data);
            var access_token = login_content.access_token;

            // Use access token in next request.
            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var response = await authenticated_client.GetAsync("/_matrix/client/r0/account/3pid");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected_response = new
            {
                threepid = new List<object>(0) { }
            };
            var content = Utilities.GetContent(response, expected_response);
            Assert.NotNull(content.threepid);
            Assert.Empty(content.threepid);
            var plain_content = await response.Content.ReadAsStringAsync();
            Assert.Equal("{\"threepid\":[]}", plain_content);
        }

        [Theory]
        [InlineData("add")]
        [InlineData("bind")]
        [InlineData("delete")]
        [InlineData("unbind")]
        public async Task TestAccountThreePidAddBindDeleteUnbind_NoAuthorization(string endpoint)
        {
            var post_data = new
            {
                sid = "abcdefghijklmnop1",
                client_secret = "secret"
            };
            var response = await client.PostAsync("/_matrix/client/r0/account/3pid/" + endpoint, JsonContent.Create(post_data));

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_MISSING_TOKEN",
                error = "Missing access token."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Theory]
        [InlineData("add")]
        [InlineData("bind")]
        [InlineData("delete")]
        [InlineData("unbind")]
        public async Task TestAccountThreePidAddBindDeleteUnbind_InvalidAccessToken(string endpoint)
        {
            HttpClient unauthenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            unauthenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer foobar");

            var post_data = new
            {
                sid = "abcdefghijklmnop1",
                client_secret = "secret"
            };
            var response = await unauthenticated_client.PostAsync("/_matrix/client/r0/account/3pid/" + endpoint, JsonContent.Create(post_data));

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_UNKNOWN_TOKEN",
                error = "Unrecognized access token."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Theory]
        [InlineData("add")]
        [InlineData("bind")]
        [InlineData("delete")]
        [InlineData("unbind")]
        public async Task TestAccountThreePidAddBindDeleteUnbind_WithAuthorization(string endpoint)
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var body = new
            {
                type = "m.login.password",
                identifier = new
                {
                    type = "m.id.user",
                    user = "@alice:matrix.example.org"
                },
                password = "secret password",
                initial_device_display_name = "My device"
            };
            var login_response = await client.PostAsync("/_matrix/client/r0/login", JsonContent.Create(body));
            var login_data = new
            {
                user_id = "@alice:matrix.example.org",
                access_token = "random ...",
                device_id = "also random ..."
            };
            var login_content = Utilities.GetContent(login_response, login_data);
            var access_token = login_content.access_token;

            // Use access token in next request.
            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var post_data = new
            {
                sid = "abcdefghijklmnop1",
                client_secret = "secret"
            };
            var response = await authenticated_client.PostAsync("/_matrix/client/r0/account/3pid/" + endpoint, JsonContent.Create(post_data));
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected_response = new
            {
                errcode = "M_THREEPID_DENIED",
                error = "Third-party identifier is not allowed here."
            };
            var content = Utilities.GetContent(response, expected_response);
            Assert.Equal(expected_response.errcode, content.errcode);
            Assert.Equal(expected_response.error, content.error);
        }

        [Fact]
        public async Task TestMailAccountThreePidToken()
        {
            var postData = new
            {
                client_secret = "not so secret",
                email = "alice@example.org",
                send_attempt = 1,
                next_link = "https://example.org/congratulations.html",
                id_server = "id.example.com",
                id_access_token = "some_string"
            };

            var response = await client.PostAsync("/_matrix/client/r0/account/3pid/email/requestToken", JsonContent.Create(postData));

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_THREEPID_DENIED",
                error = "Third-party identifier is not allowed here."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestPhoneAccountThreePidToken()
        {
            var postData = new
            {
                client_secret = "not so secret",
                country = "GB",
                phone_number = "07700900001",
                send_attempt = 1,
                next_link = "https://example.org/congratulations.html",
                id_server = "id.example.com",
                id_access_token = "some_string"
            };

            var response = await client.PostAsync("/_matrix/client/r0/account/3pid/msisdn/requestToken", JsonContent.Create(postData));

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_THREEPID_DENIED",
                error = "Third-party identifier is not allowed here."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestWhoami_NoAuthorization()
        {
            var response = await client.GetAsync("/_matrix/client/r0/account/whoami");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_MISSING_TOKEN",
                error = "Missing access token."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestWhoami_InvalidAccessToken()
        {
            HttpClient unauthenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            unauthenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer foobar");
            var response = await unauthenticated_client.GetAsync("/_matrix/client/r0/account/whoami");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_UNKNOWN_TOKEN",
                error = "Unrecognized access token."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestWhoami_WithAuthorization()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var body = new
            {
                type = "m.login.password",
                identifier = new
                {
                    type = "m.id.user",
                    user = "@alice:matrix.example.org"
                },
                password = "secret password",
                initial_device_display_name = "My device"
            };
            var login_response = await client.PostAsync("/_matrix/client/r0/login", JsonContent.Create(body));
            var login_data = new
            {
                user_id = "@alice:matrix.example.org",
                access_token = "random ...",
                device_id = "also random ..."
            };
            var login_content = Utilities.GetContent(login_response, login_data);
            var access_token = login_content.access_token;

            // Use access token in next request.
            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var response = await authenticated_client.GetAsync("/_matrix/client/r0/account/whoami");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected_response = new
            {
                user_id = "@alice:matrix.example.org"
            };
            var content = Utilities.GetContent(response, expected_response);
            Assert.Equal(expected_response.user_id, content.user_id);
        }
    }
}
