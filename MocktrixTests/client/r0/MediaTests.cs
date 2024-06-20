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
    public class MediaTests
    {
        private readonly HttpClient client = new()
        {
            BaseAddress = Utilities.BaseAddress
        };

        [Fact]
        public async Task TestConfig_NoAuthorization()
        {
            var response = await client.GetAsync("/_matrix/media/r0/config");

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
        public async Task TestConfig_InvalidAccessToken()
        {
            HttpClient unauthenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            unauthenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer foobar");
            var response = await unauthenticated_client.GetAsync("/_matrix/media/r0/config");

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
        public async Task TestConfig_WithAuthorization()
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

            var response = await authenticated_client.GetAsync("/_matrix/media/r0/config");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected_response = new Mocktrix.Protocol.Types.Media.MediaConfiguration
            {
                UploadSize = 10485760
            };
            var content = Utilities.GetContent(response, expected_response);
            Assert.Equal(expected_response.UploadSize, content.UploadSize);
        }
    }
}
