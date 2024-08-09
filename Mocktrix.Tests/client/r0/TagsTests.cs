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

using Mocktrix.Events;
using System.Net;

namespace MocktrixTests
{
    public class TagsTests
    {
        private readonly HttpClient client = new()
        {
            BaseAddress = Utilities.BaseAddress
        };

        [Fact]
        public async Task TestGetTags_NoAuthorization()
        {
            var response = await client.GetAsync("/_matrix/client/r0/user/@alice:matrix.example.org/rooms/!someRoom:matrix.example.org/tags");

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
        public async Task TestGetTags_InvalidAccessToken()
        {
            HttpClient unauthenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            unauthenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer foobar");
            var response = await unauthenticated_client.GetAsync("/_matrix/client/r0/user/@alice:matrix.example.org/rooms/!someRoom:matrix.example.org/tags");

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
        public async Task TestGetTags_WithAuthorization_OtherUser()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            string user_id = "@tag_user:" + Utilities.BaseAddress.Host;
            var access_token = await Utilities.PerformLogin(client, user_id);

            // Use access token in next request.
            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var response = await authenticated_client.GetAsync("/_matrix/client/r0/user/@alice:matrix.example.org/rooms/!room_without_tags:matrix.example.org/tags");
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected = new
            {
                errcode = "M_FORBIDDEN",
                error = "You cannot query tags of other users."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestGetTags_WithAuthorization_RoomWithoutTags()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            string user_id = "@tag_user:" + Utilities.BaseAddress.Host;
            var access_token = await Utilities.PerformLogin(client, user_id);

            // Use access token in next request.
            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var response = await authenticated_client.GetAsync("/_matrix/client/r0/user/"+user_id+"/rooms/!room_without_tags:matrix.example.org/tags");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            string content = await response.Content.ReadAsStringAsync();
            Assert.Equal("{\"tags\":{}}", content);
        }

        [Fact]
        public async Task TestGetTags_WithAuthorization_RoomWithTags()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            string user_id = "@tag_user:" + Utilities.BaseAddress.Host;
            var access_token = await Utilities.PerformLogin(client, user_id);

            // Use access token in next request.
            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var response = await authenticated_client.GetAsync("/_matrix/client/r0/user/" + user_id + "/rooms/!room_with_some_tags:matrix.example.org/tags");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected = new TagEventContent()
            {
                Tags = new()
                {
                    { "m.favourite", new() { Order = 0.25 } },
                    { "u.null", new() },
                    { "u.some_tag", new() { Order = 1.0 } }
                }
            };
            var content = Utilities.GetContent(response, expected);
            Assert.NotNull(content.Tags);
            Assert.Equal(expected.Tags.Count, content.Tags.Count);
            Assert.True(content.Tags.ContainsKey("m.favourite"));
            Assert.Equal(0.25, content.Tags["m.favourite"].Order);
            Assert.True(content.Tags.ContainsKey("u.null"));
            Assert.Null(content.Tags["u.null"].Order);
            Assert.True(content.Tags.ContainsKey("u.some_tag"));
            Assert.Equal(1.0, content.Tags["u.some_tag"].Order);
        }
    }
}
