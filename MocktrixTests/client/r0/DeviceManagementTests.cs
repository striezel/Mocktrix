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

using Mocktrix.Protocol.Types.DeviceManagement;
using System.Net;
using System.Net.Http.Json;

namespace MocktrixTests
{
    public class DeviceManagementTests
    {
        private readonly HttpClient client = new()
        {
            BaseAddress = Utilities.BaseAddress
        };

        [Fact]
        public async Task TestDevices_NoAuthorization()
        {
            var response = await client.GetAsync("/_matrix/client/r0/devices");

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
        public async Task TestDevices_InvalidAccessToken()
        {
            HttpClient unauthenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            unauthenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer foobar");
            var response = await unauthenticated_client.GetAsync("/_matrix/client/r0/devices");

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
        public async Task TestDevices_WithAuthorization()
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
                device_id = "test_dev_mgmt_id_2",
                initial_device_display_name = "My device mgmt. dev #2"
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
            Assert.Equal(body.device_id, login_content.device_id);

            // Use access token in next request.
            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var response = await authenticated_client.GetAsync("/_matrix/client/r0/devices");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var expected_response = new
            {
                devices = new List<DeviceData>()
                {
                    new DeviceData()
                    {
                        DeviceId = body.device_id,
                        DisplayName = "My device mgmt. dev #2",
                        LastSeenIP = null,
                        LastSeenTimestamp = null
                    }
                }
            };
            
            var content = Utilities.GetContent(response, expected_response);
            Assert.NotNull(content.devices);
            // Find device created as part of login.
            var item = content.devices.Find(element => element.DeviceId == body.device_id);
            Assert.NotNull(item);
            Assert.Equal(body.device_id, item.DeviceId);
            Assert.Equal(body.initial_device_display_name, item.DisplayName);
            Assert.Null(item.LastSeenIP);
            Assert.Null(item.LastSeenTimestamp);
            // Find device from mock data.
            item = content.devices.Find(element => element.DeviceId == "AliceDeviceId");
            Assert.NotNull(item);
            Assert.Equal("AliceDeviceId", item.DeviceId);
            Assert.Equal("Alice's Matrix-enabled comm badge", item.DisplayName);
            Assert.Null(item.LastSeenIP);
            Assert.Null(item.LastSeenTimestamp);
        }

        [Fact]
        public async Task TestDeviceWithId_NoAuthorization()
        {
            var response = await client.GetAsync("/_matrix/client/r0/devices/foobar");

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
        public async Task TestDeviceWithId_InvalidAccessToken()
        {
            HttpClient unauthenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            unauthenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer foobar");
            var response = await unauthenticated_client.GetAsync("/_matrix/client/r0/devices/foobar");

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
        public async Task TestDeviceWithId_IdNotFound()
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
                device_id = "test_dev_mgmt_id_1",
                initial_device_display_name = "My device mgmt. dev #1"
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
            Assert.Equal(body.device_id, login_content.device_id);

            // Use access token in next request.
            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var response = await authenticated_client.GetAsync("/_matrix/client/r0/devices/NonExistentDeviceId1");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var expected_response = new
            {
                errcode = "M_NOT_FOUND",
                error = "Device not found"
            };
            var content = Utilities.GetContent(response, expected_response);
            Assert.Equal(expected_response.errcode, content.errcode);
            Assert.Equal(expected_response.error, content.error);
        }

        [Fact]
        public async Task TestDeviceWithId_WithAuthorization()
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
                device_id = "test_dev_mgmt_id_1",
                initial_device_display_name = "My device mgmt. dev #1"
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
            Assert.Equal(body.device_id, login_content.device_id);

            // Use access token in next request.
            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            var response = await authenticated_client.GetAsync("/_matrix/client/r0/devices/" + body.device_id);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var expected_response = new DeviceData
            {
                DeviceId = body.device_id,
                DisplayName = "My device mgmt. dev #1",
                LastSeenIP = null,
                LastSeenTimestamp = null
            };
            var content = Utilities.GetContent(response, expected_response);
            Assert.Equal(expected_response.DeviceId, content.DeviceId);
            Assert.Equal(expected_response.DisplayName, content.DisplayName);
            Assert.Null(content.LastSeenIP);
            Assert.Null(content.LastSeenTimestamp);
        }
    }
}
