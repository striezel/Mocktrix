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
    public class ProfileTests
    {
        private readonly HttpClient client = new()
        {
            BaseAddress = Utilities.BaseAddress
        };

        [Fact]
        public async Task TestRetrieveDisplayName_NonExistentUser()
        {
            var response = await client.GetAsync("/_matrix/client/r0/profile/@does-not-exist:" + client.BaseAddress?.Host + "/displayname");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_NOT_FOUND",
                error = "The user profile was not found."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestRetrieveDisplayName_ExistingUserWithoutDisplayName()
        {
            var response = await client.GetAsync("/_matrix/client/r0/profile/@unnamed_user:" + client.BaseAddress?.Host + "/displayname");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("{}", content);
        }

        [Fact]
        public async Task TestRetrieveDisplayName_ExistingUserWithDisplayName()
        {
            var response = await client.GetAsync("/_matrix/client/r0/profile/@named_user:" + client.BaseAddress?.Host + "/displayname");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                displayname = "Nomen Nominandum"
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.displayname, content.displayname);
        }

        [Fact]
        public async Task TestChangeDisplayName_NoAuthorization()
        {
            var data = new { displayname = "Alice" };
            var response = await client.PutAsync("/_matrix/client/r0/profile/@alice:" + client.BaseAddress?.Host + "/displayname", JsonContent.Create(data));
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
        public async Task TestChangeDisplayName_InvalidAccessToken()
        {
            HttpClient unauthenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            unauthenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer foobar");

            var data = new { displayname = "Alice" };
            var response = await unauthenticated_client.PutAsync("/_matrix/client/r0/profile/@alice:" + client.BaseAddress?.Host + "/displayname", JsonContent.Create(data));
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
        public async Task TestChangeDisplayName_WrongUser()
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

            var data = new { displayname = "Alice" };
            var response = await authenticated_client.PutAsync("/_matrix/client/r0/profile/@all_alice:matrix.example.org/displayname", JsonContent.Create(data));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_FORBIDDEN",
                error = "Changing someone else's profile is not allowed."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestChangeDisplayName_Success()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var body = new
            {
                type = "m.login.password",
                identifier = new
                {
                    type = "m.id.user",
                    user = "name_change_user"
                },
                password = "some password"
            };
            var login_response = await client.PostAsync("/_matrix/client/r0/login", JsonContent.Create(body));
            var login_data = new
            {
                user_id = "@...",
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

            var data = new { displayname = "A. N. Other" };
            var response = await authenticated_client.PutAsync("/_matrix/client/r0/profile/@name_change_user:" + authenticated_client.BaseAddress.Host + "/displayname", JsonContent.Create(data));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("{}", content);

            response = await client.GetAsync("/_matrix/client/r0/profile/@name_change_user:" + client.BaseAddress?.Host + "/displayname");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                displayname = "A. N. Other"
            };
            var retrieval_content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.displayname, retrieval_content.displayname);
        }
    }
}
