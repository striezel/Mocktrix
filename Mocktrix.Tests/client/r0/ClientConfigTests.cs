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
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MocktrixTests.client.r0
{
    public class ClientConfigTests
    {
        private readonly HttpClient client = new()
        {
            BaseAddress = Utilities.BaseAddress
        };

        [Fact]
        public async Task TestGetClientData_NoAuthorization()
        {
            var response = await client.GetAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/account_data/x.mas.song", TestContext.Current.CancellationToken);

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
        public async Task TestGetClientData_InvalidAccessToken()
        {
            HttpClient unauthenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            unauthenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer foobar");
            var response = await unauthenticated_client.GetAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/account_data/x.mas.song", TestContext.Current.CancellationToken);

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
        public async Task TestGetClientData_WrongUser()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client);

            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);
            var response = await authenticated_client.GetAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/account_data/x.mas.song", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_FORBIDDEN",
                error = "You cannot get account data of another user."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestGetClientData_NotFound()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client, "account_data_user");

            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);
            var response = await authenticated_client.GetAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/account_data/does.not.exist", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_NOT_FOUND",
                error = "Account data of the requested type was not found."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestGetClientData_Success()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client, "account_data_user");

            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);
            var response = await authenticated_client.GetAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/account_data/x.mas.song", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                snow = "glistening",
                sleigh_bells = "ring, ring",
                building_snowman = true
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.snow, content.snow);
            Assert.Equal(expected.sleigh_bells, content.sleigh_bells);
            Assert.Equal(expected.building_snowman, content.building_snowman);
        }

        [Fact]
        public async Task TestSetClientData_NoAuthorization()
        {
            var data = new { foo = "bar", baz = false };
            var response = await client.PutAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/account_data/com.winter.song",
                JsonContent.Create(data), TestContext.Current.CancellationToken);

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
        public async Task TestSetClientData_InvalidAccessToken()
        {
            HttpClient unauthenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            unauthenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer foobar");

            var data = new { foo = "bar", baz = false };
            var response = await unauthenticated_client.PutAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/account_data/com.winter.song",
                JsonContent.Create(data), TestContext.Current.CancellationToken);

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
        public async Task TestSetClientData_WrongUser()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client);

            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);
            var data = new { foo = "bar", baz = false };
            var response = await authenticated_client.PutAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/account_data/com.winter.song",
                JsonContent.Create(data), TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_FORBIDDEN",
                error = "You cannot set account data of another user."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestSetClientData_NoJSON()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client, "account_data_user");

            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var response = await authenticated_client.PutAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/account_data/com.winter.song",
                new StringContent("{ \"blah\": blah blah", MediaTypeHeaderValue.Parse("application/json")), TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_NOT_JSON",
                error = "The content is not valid JSON."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestSetClientData_Success()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client, "account_data_user");

            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var data = new { oh = "Tannenbaum", en = "Oh, christmas tree" };
            var response = await authenticated_client.PutAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/account_data/com.winter.song",
                JsonContent.Create(data), TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var plain_json = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Equal("{}", plain_json);

            // Get value back from server, should be the same data that was sent.
            response = await authenticated_client.GetAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/account_data/com.winter.song", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                oh = "Tannenbaum",
                en = "Oh, christmas tree"
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.oh, content.oh);
            Assert.Equal(expected.en, content.en);

            plain_json = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Equal("{\"oh\":\"Tannenbaum\",\"en\":\"Oh, christmas tree\"}", plain_json);
        }

        [Fact]
        public async Task TestSetClientData_Twice()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client, "account_data_user");

            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var data = new { oh = "Tannenbaum", en = "Oh, christmas tree" };
            var response = await authenticated_client.PutAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/account_data/com.winter.song.twice",
                JsonContent.Create(data), TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var plain_json = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Equal("{}", plain_json);

            // Set it again, should work and update existing value.
            var data2 = new
            {
                oh = "Tannenbaum",
                oh2 = "Tannenbaum",
                en = "Oh, christmas tree"
            };
            response = await authenticated_client.PutAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/account_data/com.winter.song.twice",
                JsonContent.Create(data2), TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            plain_json = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Equal("{}", plain_json);

            // Get value back from server, should be the same data that was sent the second time.
            response = await authenticated_client.GetAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/account_data/com.winter.song.twice", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                oh = "Tannenbaum",
                oh2 = "Tannenbaum",
                en = "Oh, christmas tree"
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.oh, content.oh);
            Assert.Equal(expected.en, content.en);

            plain_json = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Equal("{\"oh\":\"Tannenbaum\",\"oh2\":\"Tannenbaum\",\"en\":\"Oh, christmas tree\"}", plain_json);
        }

        [Fact]
        public async Task TestSetClientData_RejectServerManagedType()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client, "account_data_user");

            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var data = new { event_id = "$what_ever:matrix.example.org" };
            var response = await authenticated_client.PutAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/account_data/m.fully_read",
                JsonContent.Create(data), TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_UNKNOWN",
                error = "The m.fully_read type cannot be set via this API."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestGetRoomClientData_NoAuthorization()
        {
            var response = await client.GetAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/rooms/%21account_data_room%3Amatrix.example.org/account_data/org.snow.data", TestContext.Current.CancellationToken);

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
        public async Task TestGetRoomClientData_InvalidAccessToken()
        {
            HttpClient unauthenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            unauthenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer foobar");
            var response = await unauthenticated_client.GetAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/rooms/%21account_data_room%3Amatrix.example.org/account_data/org.snow.data", TestContext.Current.CancellationToken);

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
        public async Task TestGetRoomClientData_WrongUser()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client);

            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);
            var response = await authenticated_client.GetAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/rooms/%21account_data_room%3Amatrix.example.org/account_data/org.snow.data", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_FORBIDDEN",
                error = "You cannot get account data of another user."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestGetRoomClientData_NotFound()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client, "account_data_user");

            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);
            var response = await authenticated_client.GetAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/rooms/%21account_data_room%3Amatrix.example.org/account_data/does.not.exist", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_NOT_FOUND",
                error = "Account data of the requested type for the requested room was not found."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestRoomGetClientData_Success()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client, "account_data_user");

            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);
            var response = await authenticated_client.GetAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/rooms/%21account_data_room%3Amatrix.example.org/account_data/org.snow.data", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                snow = "glistening",
                sleigh_bells = "ring, ring, ring",
                building_snowman = true
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.snow, content.snow);
            Assert.Equal(expected.sleigh_bells, content.sleigh_bells);
            Assert.Equal(expected.building_snowman, content.building_snowman);
        }

        [Fact]
        public async Task TestSetRoomClientData_NoAuthorization()
        {
            var data = new { foo = "bar", baz = false };
            var response = await client.PutAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/rooms/%21account_data_room%3Amatrix.example.org/account_data/com.winter.song",
                JsonContent.Create(data), TestContext.Current.CancellationToken);

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
        public async Task TestSetRoomClientData_InvalidAccessToken()
        {
            HttpClient unauthenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            unauthenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer foobar");

            var data = new { foo = "bar", baz = false };
            var response = await unauthenticated_client.PutAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/rooms/%21account_data_room%3Amatrix.example.org/account_data/com.winter.song",
                JsonContent.Create(data), TestContext.Current.CancellationToken);

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
        public async Task TestSetRoomClientData_WrongUser()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client);

            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);
            var data = new { foo = "bar", baz = false };
            var response = await authenticated_client.PutAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/rooms/%21account_data_room%3Amatrix.example.org/account_data/com.winter.song",
                JsonContent.Create(data), TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_FORBIDDEN",
                error = "You cannot set account data of another user."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestSetRoomClientData_NoJSON()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client, "account_data_user");

            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var response = await authenticated_client.PutAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/rooms/%21account_data_room%3Amatrix.example.org/account_data/com.winter.song",
                new StringContent("{ \"blah\": blah blah", MediaTypeHeaderValue.Parse("application/json")), TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_NOT_JSON",
                error = "The content is not valid JSON."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestSetRoomClientData_Success()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client, "account_data_user");

            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var data = new { oh = "Tannenbaum", english = "Oh, christmas tree", room = true };
            var response = await authenticated_client.PutAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/rooms/%21account_data_room%3Amatrix.example.org/account_data/com.winter.song",
                JsonContent.Create(data), TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var plain_json = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Equal("{}", plain_json);

            // Get value back from server, should be the same data that was sent.
            response = await authenticated_client.GetAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/rooms/%21account_data_room%3Amatrix.example.org/account_data/com.winter.song", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                oh = "Tannenbaum",
                english = "Oh, christmas tree",
                room = true
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.oh, content.oh);
            Assert.Equal(expected.english, content.english);
            Assert.Equal(expected.room, content.room);

            plain_json = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Equal("{\"oh\":\"Tannenbaum\",\"english\":\"Oh, christmas tree\",\"room\":true}", plain_json);
        }

        [Fact]
        public async Task TestSetRoomClientData_Twice()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client, "account_data_user");

            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var data = new { oh = "Tannenbaum", en = "Oh, christmas tree", room = true };
            var response = await authenticated_client.PutAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/rooms/%21account_data_room%3Amatrix.example.org/account_data/com.winter.song.twice",
                JsonContent.Create(data), TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var plain_json = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Equal("{}", plain_json);

            // Set it again, should work and update existing value.
            var data2 = new
            {
                oh = "Tannenbaum",
                oh2 = "Tannenbaum",
                eng = "Oh, christmas tree",
                room = true
            };
            response = await authenticated_client.PutAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/rooms/%21account_data_room%3Amatrix.example.org/account_data/com.winter.song.twice",
                JsonContent.Create(data2), TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            plain_json = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Equal("{}", plain_json);

            // Get value back from server, should be the same data that was sent the second time.
            response = await authenticated_client.GetAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/rooms/%21account_data_room%3Amatrix.example.org/account_data/com.winter.song.twice", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                oh = "Tannenbaum",
                oh2 = "Tannenbaum",
                eng = "Oh, christmas tree",
                room = true
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.oh, content.oh);
            Assert.Equal(expected.oh2, content.oh2);
            Assert.Equal(expected.eng, content.eng);
            Assert.Equal(expected.room, content.room);

            plain_json = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Equal("{\"oh\":\"Tannenbaum\",\"oh2\":\"Tannenbaum\",\"eng\":\"Oh, christmas tree\",\"room\":true}", plain_json);
        }

        [Fact]
        public async Task TestSetRoomClientData_RejectServerManagedType()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client, "account_data_user");

            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var data = new { event_id = "$what_ever:matrix.example.org" };
            var response = await authenticated_client.PutAsync("/_matrix/client/r0/user/%40account_data_user%3A" + Utilities.BaseAddress.Host + "/rooms/%21account_data_room%3Amatrix.example.org/account_data/m.fully_read",
                JsonContent.Create(data), TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_UNKNOWN",
                error = "The m.fully_read type cannot be set via this API."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }
    }
}
