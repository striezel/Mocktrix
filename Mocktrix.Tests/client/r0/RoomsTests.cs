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
    public class RoomsTests
    {
        private readonly HttpClient client = new()
        {
            BaseAddress = Utilities.BaseAddress
        };

        [Fact]
        public async Task TestJoinedRooms_NoAuthorization()
        {
            var response = await client.GetAsync("/_matrix/client/r0/joined_rooms");
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
        public async Task TestJoinedRooms_InvalidAccessToken()
        {
            HttpClient unauthenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            unauthenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer foobar");

            var response = await unauthenticated_client.GetAsync("/_matrix/client/r0/joined_rooms");
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
        public async Task TestJoinedRooms_Success()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client, "joined_user", "the password");

            // Use access token in next request.
            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var response = await authenticated_client.GetAsync("/_matrix/client/r0/joined_rooms");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                joined_rooms = new List<string>(2)
                {
                    "!first_joined_room:matrix.example.org",
                    "!second_joined_room:matrix.example.org"
                }
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(2, content.joined_rooms.Count);
            Assert.Contains(expected.joined_rooms[0], content.joined_rooms);
            Assert.Contains(expected.joined_rooms[1], content.joined_rooms);
        }

        [Fact]
        public async Task TestJoinedRooms_Success_NoRooms()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client, "not_a_joined_user", "some password");

            // Use access token in next request.
            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var response = await authenticated_client.GetAsync("/_matrix/client/r0/joined_rooms");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                joined_rooms = new List<string>(0)
                {
                }
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Empty(content.joined_rooms);
        }

        [Fact]
        public async Task TestCreateRoom_NoAuthorization()
        {
            var data = new {
                preset = "public_chat"
            };
            var response = await client.PostAsync("/_matrix/client/r0/createRoom", JsonContent.Create(data));
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
        public async Task TestCreateRoom_InvalidAccessToken()
        {
            HttpClient unauthenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            unauthenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer foobar");

            var data = new
            {
                preset = "public_chat"
            };
            var response = await unauthenticated_client.PostAsync("/_matrix/client/r0/createRoom", JsonContent.Create(data));
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
        public async Task TestCreateRoom_UnsupportedRoomVersion()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client);

            // Use access token in next request.
            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var data = new
            {
                preset = "private_chat",
                room_version = "69"
            };
            var response = await authenticated_client.PostAsync("/_matrix/client/r0/createRoom", JsonContent.Create(data));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_UNSUPPORTED_ROOM_VERSION",
                error = "The given room version is not supported."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestCreateRoom_AliasContainsInvalidChar()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client);

            // Use access token in next request.
            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var data = new
            {
                preset = "private_chat",
                room_alias_name = "abcdef\r\nghijkl"
            };
            var response = await authenticated_client.PostAsync("/_matrix/client/r0/createRoom", JsonContent.Create(data));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_UNKNOWN",
                error = "The requested alias contains invalid characters."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestCreateRoom_AliasFullyQualified()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client);

            // Use access token in next request.
            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var data = new
            {
                preset = "private_chat",
                room_alias_name = "#abcdef:matrix.example.org"
            };
            var response = await authenticated_client.PostAsync("/_matrix/client/r0/createRoom", JsonContent.Create(data));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_UNKNOWN",
                error = "The ':' character is not allowed in the room alias."
                      + " This endpoint only expects the localpart "
                      + "of the alias and not the fully-qualified alias,"
                      + " e.g. 'foo' instead of '#foo:example.org'."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestCreateRoom_AliasTooLong()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client);

            // Use access token in next request.
            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var data = new
            {
                preset = "private_chat",
                room_alias_name = new string('a', 300)
            };
            var response = await authenticated_client.PostAsync("/_matrix/client/r0/createRoom", JsonContent.Create(data));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_UNKNOWN",
                error = "The requested alias is too long."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestCreateRoom_Success()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client);

            // Use access token in next request.
            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var data = new
            {
                preset = "private_chat",
                room_version = "1",
                name = "My first created room",
                topic = "Just testing ..."
            };
            var response = await authenticated_client.PostAsync("/_matrix/client/r0/createRoom", JsonContent.Create(data));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                room_id = "!some_id_here:server.domain"
            };
            var content = Utilities.GetContent(response, expected);
            Assert.NotNull(content.room_id);
            Assert.StartsWith("!", content.room_id);
            Assert.EndsWith(":" + Utilities.BaseAddress.Host, content.room_id);
            Assert.Matches("^![a-zA-Z0-9]+:", content.room_id);
        }
    }
}
