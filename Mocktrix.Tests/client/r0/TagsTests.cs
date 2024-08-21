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
using System.Net.Http.Json;

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

            var response = await authenticated_client.GetAsync("/_matrix/client/r0/user/" + user_id + "/rooms/!room_without_tags:matrix.example.org/tags");
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

        [Fact]
        public async Task TestDeleteTag_NoAuthorization()
        {
            string user_id = "@tag_user:" + Utilities.BaseAddress.Host;
            var response = await client.DeleteAsync("/_matrix/client/r0/user/" + user_id + "/rooms/!room_with_tag_to_delete:matrix.example.org/tags/u.delete_me");

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
        public async Task TestDeleteTag_InvalidAccessToken()
        {
            HttpClient unauthenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            string user_id = "@tag_user:" + Utilities.BaseAddress.Host;
            unauthenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer foobar");
            var response = await unauthenticated_client.DeleteAsync("/_matrix/client/r0/user/" + user_id + "/rooms/!room_with_tag_to_delete:matrix.example.org/tags/u.delete_me");

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
        public async Task TestDeleteTag_WithAuthorization_OtherUser()
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

            var response = await authenticated_client.DeleteAsync("/_matrix/client/r0/user/@alice:matrix.example.org/rooms/!room_with_tag_to_delete:matrix.example.org/tags/u.delete_me");
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected = new
            {
                errcode = "M_FORBIDDEN",
                error = "You cannot delete tags of other users."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestDeleteTag_WithAuthorization()
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

            // Tag should still be set before deletion.
            {
                var pre_delete_response = await authenticated_client.GetAsync("/_matrix/client/r0/user/" + user_id + "/rooms/!room_with_tag_to_delete:matrix.example.org/tags");
                Assert.Equal(HttpStatusCode.OK, pre_delete_response.StatusCode);
                Assert.Equal("application/json", pre_delete_response.Content.Headers.ContentType?.MediaType);

                string pre_delete_content = await pre_delete_response.Content.ReadAsStringAsync();
                Assert.Equal("{\"tags\":{\"u.delete_me\":{\"order\":0.5},\"u.keep_me\":{\"order\":0.25}}}", pre_delete_content);
            }

            // Perform deletion.
            {
                var response = await authenticated_client.DeleteAsync("/_matrix/client/r0/user/" + user_id + "/rooms/!room_with_tag_to_delete:matrix.example.org/tags/u.delete_me");
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

                var content = await response.Content.ReadAsStringAsync();
                Assert.Equal("{}", content);
            }

            // Check tags after deletion.
            {
                var post_delete_response = await authenticated_client.GetAsync("/_matrix/client/r0/user/" + user_id + "/rooms/!room_with_tag_to_delete:matrix.example.org/tags");
                Assert.Equal(HttpStatusCode.OK, post_delete_response.StatusCode);
                Assert.Equal("application/json", post_delete_response.Content.Headers.ContentType?.MediaType);

                string post_delete_content = await post_delete_response.Content.ReadAsStringAsync();
                Assert.Equal("{\"tags\":{\"u.keep_me\":{\"order\":0.25}}}", post_delete_content);
            }
        }

        [Fact]
        public async Task TestAddTag_NoAuthorization()
        {
            string user_id = "@tag_user:" + Utilities.BaseAddress.Host;
            var data = new { order = 0.25 };
            var response = await client.PutAsync("/_matrix/client/r0/user/" + user_id + "/rooms/!room_to_add_tag_to:matrix.example.org/tags/u.add_me", JsonContent.Create(data));

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
        public async Task TestAddTag_InvalidAccessToken()
        {
            HttpClient unauthenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            string user_id = "@tag_user:" + Utilities.BaseAddress.Host;
            unauthenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer foobar");
            var data = new { order = 0.25 };
            var response = await unauthenticated_client.PutAsync("/_matrix/client/r0/user/" + user_id + "/rooms/!room_to_add_tag_to:matrix.example.org/tags/u.add_me", JsonContent.Create(data));

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
        public async Task TestAddTag_WithAuthorization_OtherUser()
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

            var data = new { order = 0.25 };
            var response = await authenticated_client.PutAsync("/_matrix/client/r0/user/@alice:matrix.example.org/rooms/!room_to_add_tag_to:matrix.example.org/tags/u.add_me", JsonContent.Create(data));
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected = new
            {
                errcode = "M_FORBIDDEN",
                error = "You cannot add tags for other users."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestAddTag_WithAuthorization_Create()
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

            // No tag should be set before adding tag.
            {
                var pre_add_response = await authenticated_client.GetAsync("/_matrix/client/r0/user/" + user_id + "/rooms/!room_to_add_tag_to:matrix.example.org/tags");
                Assert.Equal(HttpStatusCode.OK, pre_add_response.StatusCode);
                Assert.Equal("application/json", pre_add_response.Content.Headers.ContentType?.MediaType);

                string pre_add_content = await pre_add_response.Content.ReadAsStringAsync();
                Assert.Equal("{\"tags\":{}}", pre_add_content);
            }

            // Perform tag creation.
            {
                var data = new { order = 0.25 };
                var response = await authenticated_client.PutAsync("/_matrix/client/r0/user/" + user_id + "/rooms/!room_to_add_tag_to:matrix.example.org/tags/u.add_me", JsonContent.Create(data));
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

                var content = await response.Content.ReadAsStringAsync();
                Assert.Equal("{}", content);
            }

            // Check tags after creation.
            {
                var post_add_response = await authenticated_client.GetAsync("/_matrix/client/r0/user/" + user_id + "/rooms/!room_to_add_tag_to:matrix.example.org/tags");
                Assert.Equal(HttpStatusCode.OK, post_add_response.StatusCode);
                Assert.Equal("application/json", post_add_response.Content.Headers.ContentType?.MediaType);

                string post_add_content = await post_add_response.Content.ReadAsStringAsync();
                Assert.Equal("{\"tags\":{\"u.add_me\":{\"order\":0.25}}}", post_add_content);
            }
        }

        [Fact]
        public async Task TestAddTag_WithAuthorization_Update()
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

            // Existing tag should be set before adding/updating tag.
            {
                var pre_add_response = await authenticated_client.GetAsync("/_matrix/client/r0/user/" + user_id + "/rooms/!room_to_change_tag:matrix.example.org/tags");
                Assert.Equal(HttpStatusCode.OK, pre_add_response.StatusCode);
                Assert.Equal("application/json", pre_add_response.Content.Headers.ContentType?.MediaType);

                string pre_add_content = await pre_add_response.Content.ReadAsStringAsync();
                Assert.Equal("{\"tags\":{\"u.existing\":{\"order\":0.5}}}", pre_add_content);
            }

            // Perform tag update.
            {
                var data = new { order = 0.75 };
                var response = await authenticated_client.PutAsync("/_matrix/client/r0/user/" + user_id + "/rooms/!room_to_change_tag:matrix.example.org/tags/u.existing", JsonContent.Create(data));
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

                var content = await response.Content.ReadAsStringAsync();
                Assert.Equal("{}", content);
            }

            // Check tags after update.
            {
                var post_add_response = await authenticated_client.GetAsync("/_matrix/client/r0/user/" + user_id + "/rooms/!room_to_change_tag:matrix.example.org/tags");
                Assert.Equal(HttpStatusCode.OK, post_add_response.StatusCode);
                Assert.Equal("application/json", post_add_response.Content.Headers.ContentType?.MediaType);

                string post_add_content = await post_add_response.Content.ReadAsStringAsync();
                Assert.Equal("{\"tags\":{\"u.existing\":{\"order\":0.75}}}", post_add_content);
            }
        }
    }
}
