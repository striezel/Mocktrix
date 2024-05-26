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
    public class LoginTests
    {
        private readonly HttpClient client = new()
        {
            BaseAddress = Utilities.BaseAddress
        };

        internal record LoginFlow(string type);

        [Fact]
        public async Task TestAvailableLoginFlows()
        {
            var response = await client.GetAsync("/_matrix/client/r0/login");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var flows = new
            {
                flows = new List<LoginFlow>(1)
                    {
                        new("m.login.password")
                    }
            };

            var content = Utilities.GetContent(response, flows);
            Assert.NotNull(content);
            Assert.NotNull(content.flows);
            Assert.Equal(flows.flows.Count, content.flows.Count);
            Assert.Equal(flows.flows[0], content.flows[0]);
        }

        [Fact]
        public async Task TestLogin_NotPasswordBased()
        {
            var body = new
            {
                type = "m.login.token",
                identifier = new {
                    type = "m.id.user",
                    user = "@alice:matrix.example.org"
                },
                password = "foo",
                token = "very secret",
                initial_device_display_name = new string('a', 15000)
            };
            var response = await client.PostAsync("/_matrix/client/r0/login", JsonContent.Create(body));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected_error = new
            {
                errcode = "M_UNKNOWN",
                error = "Unsupported login type specified. This server only supports password-based login (\"m.login.password\")."
            };

            var content = Utilities.GetContent(response, expected_error);
            Assert.NotNull(content);
            Assert.Equal(expected_error.errcode, content.errcode);
            Assert.Equal(expected_error.error, content.error);
        }

        [Fact]
        public async Task TestLogin_NoUserIdGiven()
        {
            var body = new
            {
                type = "m.login.password",
                identifier = new
                {
                    type = "m.id.user",
                },
                password = "very secret",
                initial_device_display_name = "My device"
            };
            var response = await client.PostAsync("/_matrix/client/r0/login", JsonContent.Create(body));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected_error = new
            {
                errcode = "M_BAD_JSON",
                error = "User id or password is missing, or identifier.type is not \"m.id.user\"."
            };

            var content = Utilities.GetContent(response, expected_error);
            Assert.NotNull(content);
            Assert.Equal(expected_error.errcode, content.errcode);
            Assert.Equal(expected_error.error, content.error);
        }

        [Fact]
        public async Task TestLogin_MissingPassword()
        {
            var body = new
            {
                type = "m.login.password",
                identifier = new
                {
                    type = "m.id.user",
                    user = "@alice:matrix.example.org"
                },
                initial_device_display_name = "My device"
            };
            var response = await client.PostAsync("/_matrix/client/r0/login", JsonContent.Create(body));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected_error = new
            {
                errcode = "M_BAD_JSON",
                error = "User id or password is missing, or identifier.type is not \"m.id.user\"."
            };

            var content = Utilities.GetContent(response, expected_error);
            Assert.NotNull(content);
            Assert.Equal(expected_error.errcode, content.errcode);
            Assert.Equal(expected_error.error, content.error);
        }

        [Fact]
        public async Task TestLogin_WrongIdentifierType()
        {
            var body = new
            {
                type = "m.login.password",
                identifier = new
                {
                    type = "m.made_up_type",
                    user = "@alice:matrix.example.org"
                },
                password = "very secret",
                initial_device_display_name = "My device"
            };
            var response = await client.PostAsync("/_matrix/client/r0/login", JsonContent.Create(body));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected_error = new
            {
                errcode = "M_BAD_JSON",
                error = "User id or password is missing, or identifier.type is not \"m.id.user\"."
            };

            var content = Utilities.GetContent(response, expected_error);
            Assert.NotNull(content);
            Assert.Equal(expected_error.errcode, content.errcode);
            Assert.Equal(expected_error.error, content.error);
        }

        [Fact]
        public async Task TestLogin_NonExistentUser()
        {
            var body = new
            {
                type = "m.login.password",
                identifier = new
                {
                    type = "m.id.user",
                    user = "@does-not-exist:matrix.example.org"
                },
                password = "very secret",
                initial_device_display_name = "My device"
            };
            var response = await client.PostAsync("/_matrix/client/r0/login", JsonContent.Create(body));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected_error = new
            {
                errcode = "M_INVALID_USERNAME",
                error = "User id does not exist on this server."
            };

            var content = Utilities.GetContent(response, expected_error);
            Assert.NotNull(content);
            Assert.Equal(expected_error.errcode, content.errcode);
            Assert.Equal(expected_error.error, content.error);
        }

        [Fact]
        public async Task TestLogin_WrongPassword()
        {
            var body = new
            {
                type = "m.login.password",
                identifier = new
                {
                    type = "m.id.user",
                    user = "@alice:matrix.example.org"
                },
                password = "this is the wrong password",
                initial_device_display_name = "My device"
            };
            var response = await client.PostAsync("/_matrix/client/r0/login", JsonContent.Create(body));

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task TestLogin_CorrectCredentials()
        {
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
            var response = await client.PostAsync("/_matrix/client/r0/login", JsonContent.Create(body));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected_response = new
            {
                user_id = "@alice:matrix.example.org",
                access_token = "random ...",
                device_id = "also random ..."
            };

            var content = Utilities.GetContent(response, expected_response);
            Assert.NotNull(content);
            Assert.Equal(expected_response.user_id, content.user_id);
            Assert.NotNull(expected_response.access_token);
            Assert.Matches("^[A-Z]+$", content.device_id);
        }

        [Fact]
        public async Task TestLogin_CorrectCredentialsUsingDeprecatedJsonMembers()
        {
            var body = new
            {
                type = "m.login.password",
                // user was deprecated in favour of identifier.user + identifier.type,
                // but the server should still support this for older clients.
                user = "@alice:matrix.example.org",
                password = "secret password",
                initial_device_display_name = "Old device using older protocol version"
            };
            var response = await client.PostAsync("/_matrix/client/r0/login", JsonContent.Create(body));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected_response = new
            {
                user_id = "@alice:matrix.example.org",
                access_token = "random ...",
                device_id = "also random ..."
            };

            var content = Utilities.GetContent(response, expected_response);
            Assert.NotNull(content);
            Assert.Equal(expected_response.user_id, content.user_id);
            Assert.NotNull(expected_response.access_token);
            Assert.Matches("^[A-Z]+$", content.device_id);
        }

        [Fact]
        public async Task TestLogin_CorrectCredentialsWithExistingDeviceId()
        {
            var body = new
            {
                type = "m.login.password",
                identifier = new
                {
                    type = "m.id.user",
                    user = "@alice:matrix.example.org"
                },
                password = "secret password",
                device_id = "AliceDeviceId"
            };
            var response = await client.PostAsync("/_matrix/client/r0/login", JsonContent.Create(body));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected_response = new
            {
                user_id = "@alice:matrix.example.org",
                access_token = "random ...",
                device_id = "AliceDeviceId"
            };

            var content = Utilities.GetContent(response, expected_response);
            Assert.NotNull(content);
            Assert.Equal(expected_response.user_id, content.user_id);
            Assert.NotNull(expected_response.access_token);
            Assert.Equal(expected_response.device_id, content.device_id);
        }

        [Fact]
        public async Task TestLogin_CorrectCredentialsWithCustomDeviceId()
        {
            var body = new
            {
                type = "m.login.password",
                identifier = new
                {
                    type = "m.id.user",
                    user = "@alice:matrix.example.org"
                },
                password = "secret password",
                device_id = "OtherDeviceIdOfAlice",
                initial_display_name = "The other device"
            };
            var response = await client.PostAsync("/_matrix/client/r0/login", JsonContent.Create(body));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected_response = new
            {
                user_id = "@alice:matrix.example.org",
                access_token = "random ...",
                device_id = "OtherDeviceIdOfAlice"
            };

            var content = Utilities.GetContent(response, expected_response);
            Assert.NotNull(content);
            Assert.Equal(expected_response.user_id, content.user_id);
            Assert.NotNull(expected_response.access_token);
            Assert.Equal(expected_response.device_id, content.device_id);
        }
    }
}
