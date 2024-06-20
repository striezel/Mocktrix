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

using Mocktrix.Protocol.Types.Capabilities;
using System.Net;
using System.Net.Http.Json;

namespace MocktrixTests
{
    public class CapabilitiesTests
    {
        private readonly HttpClient client = new()
        {
            BaseAddress = Utilities.BaseAddress
        };

        [Fact]
        public async Task TestCapabilities_NoAuthorization()
        {
            var response = await client.GetAsync("/_matrix/client/r0/capabilities");

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
        public async Task TestCapabilities_InvalidAccessToken()
        {
            HttpClient unauthenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            unauthenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer foobar");
            var response = await unauthenticated_client.GetAsync("/_matrix/client/r0/capabilities");

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
        public async Task TestCapabilities_WithAuthorization()
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

            var response = await authenticated_client.GetAsync("/_matrix/client/r0/capabilities");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected_response = new
            {
                capabilities = new ServerCapabilities
                {
                    ChangePassword = new ChangePasswordCapability() { Enabled = true },
                    RoomVersions = new RoomVersionsCapability()
                    {
                        DefaultVersion = "1",
                        Available = new Dictionary<string, string>(3)
                        {
                            { "1", "stable" },
                            { "2", "unstable" },
                            { "3", "unstable" },
                        }
                    }
                }
            };
            var content = Utilities.GetContent(response, expected_response);
            var text = await response.Content.ReadAsStringAsync();
            Assert.NotNull(text);
            string expected_text = "{\"capabilities\":{\"m.change_password\":{\"enabled\":true},\"m.room_versions\":{\"default\":\"1\",\"available\":{\"1\":\"stable\",\"2\":\"unstable\",\"3\":\"unstable\"}}}}";
            Assert.Equal(expected_text, text);
            Assert.NotNull(content.capabilities);
            Assert.NotNull(content.capabilities.ChangePassword);
            Assert.Equal(expected_response.capabilities.ChangePassword.Enabled, content.capabilities.ChangePassword.Enabled);
            Assert.NotNull(content.capabilities.RoomVersions);
            Assert.Equal(expected_response.capabilities.RoomVersions.DefaultVersion, content.capabilities.RoomVersions.DefaultVersion);
            Assert.Equal(3, content.capabilities.RoomVersions.Available.Count);
            Assert.True(content.capabilities.RoomVersions.Available.ContainsKey("1"));
            Assert.Equal(expected_response.capabilities.RoomVersions.Available["1"], content.capabilities.RoomVersions.Available["1"]);
            Assert.True(content.capabilities.RoomVersions.Available.ContainsKey("2"));
            Assert.Equal(expected_response.capabilities.RoomVersions.Available["2"], content.capabilities.RoomVersions.Available["2"]);
            Assert.True(content.capabilities.RoomVersions.Available.ContainsKey("3"));
            Assert.Equal(expected_response.capabilities.RoomVersions.Available["3"], content.capabilities.RoomVersions.Available["3"]);
        }
    }
}
