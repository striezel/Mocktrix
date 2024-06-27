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

        [Fact]
        public async Task TestUpload_NoAuthorization()
        {
            byte[] file_data = "Hello there."u8.ToArray();
            var response = await client.PostAsync("/_matrix/media/r0/upload", new ByteArrayContent(file_data));

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
        public async Task TestUpload_InvalidAccessToken()
        {
            HttpClient unauthenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            unauthenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer foobar");

            byte[] file_data = "Hello there."u8.ToArray();
            var response = await unauthenticated_client.PostAsync("/_matrix/media/r0/upload", new ByteArrayContent(file_data));

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
        public async Task TestUpload_FileTooLarge()
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

            long size = 20 * 1024 * 1024;
            byte[] file_data = new byte[size];
            Array.Fill(file_data, Convert.ToByte('A'));
            var response = await authenticated_client.PostAsync("/_matrix/media/r0/upload", new ByteArrayContent(file_data));
            Assert.Equal(HttpStatusCode.RequestEntityTooLarge, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected_response = new
            {
                errcode = "M_TOO_LARGE",
                error = "Uploaded file exceeds the allowed size limit."
            };
            var content = Utilities.GetContent(response, expected_response);

            Assert.Equal(expected_response.errcode, content.errcode);
            Assert.Equal(expected_response.error, content.error);
        }

        [Fact]
        public async Task TestUpload_Success()
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

            byte[] file_data = "Hello there."u8.ToArray();
            var response = await authenticated_client.PostAsync("/_matrix/media/r0/upload", new ByteArrayContent(file_data));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected_response = new
            {
                content_uri = "mxc://..."
            };
            var content = Utilities.GetContent(response, expected_response);

            Assert.NotNull(content.content_uri);
            Assert.StartsWith("mxc://", content.content_uri);
            Assert.Contains(Utilities.BaseAddress.Host, content.content_uri);
            Assert.Matches("[A-Za-z]{24}$", content.content_uri);

            // Download should also succeed.
            string server_name = Utilities.BaseAddress.Host;
            string media_id = content.content_uri[(content.content_uri.LastIndexOf('/') + 1)..];
            var download_response = await client.GetAsync("/_matrix/media/r0/download/" + server_name + "/" + media_id);

            Assert.Equal(HttpStatusCode.OK, download_response.StatusCode);
            Assert.Equal("sandbox; default-src 'none'; script-src 'none'; plugin-types application/pdf; style-src 'unsafe-inline'; object-src 'self';",
                download_response.Headers.GetValues("Content-Security-Policy").First());

            string dl_content = await download_response.Content.ReadAsStringAsync();
            Assert.Equal("Hello there.", dl_content);
        }

        [Fact]
        public async Task TestUpload_SuccessWithContentType()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client);

            // Use access token in next request.
            HttpClient uploading_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            uploading_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);

            byte[] file_data = "Hello there?"u8.ToArray();
            var uploaded_content = new ByteArrayContent(file_data);
            uploaded_content.Headers.Add("Content-Type", "text/plain");
            var response = await uploading_client.PostAsync("/_matrix/media/r0/upload", uploaded_content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected_response = new
            {
                content_uri = "mxc://..."
            };
            var content = Utilities.GetContent(response, expected_response);

            Assert.NotNull(content.content_uri);
            Assert.StartsWith("mxc://", content.content_uri);
            Assert.Contains(Utilities.BaseAddress.Host, content.content_uri);
            Assert.Matches("[A-Za-z]{24}$", content.content_uri);

            // Download should also succeed.
            string server_name = Utilities.BaseAddress.Host;
            string media_id = content.content_uri[(content.content_uri.LastIndexOf('/') + 1)..];
            var download_response = await client.GetAsync("/_matrix/media/r0/download/" + server_name + "/" + media_id);

            Assert.Equal(HttpStatusCode.OK, download_response.StatusCode);
            Assert.Equal("sandbox; default-src 'none'; script-src 'none'; plugin-types application/pdf; style-src 'unsafe-inline'; object-src 'self';",
                download_response.Headers.GetValues("Content-Security-Policy").First());
            Assert.Equal("text/plain", download_response.Content.Headers.ContentType?.MediaType);

            string dl_content = await download_response.Content.ReadAsStringAsync();
            Assert.Equal("Hello there?", dl_content);
        }

        [Fact]
        public async Task TestUpload_SuccessWithFileName()
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

            byte[] file_data = "Hello there!"u8.ToArray();
            var response = await authenticated_client.PostAsync("/_matrix/media/r0/upload?filename=the_hello.txt", new ByteArrayContent(file_data));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected_response = new
            {
                content_uri = "mxc://..."
            };
            var content = Utilities.GetContent(response, expected_response);

            Assert.NotNull(content.content_uri);
            Assert.StartsWith("mxc://", content.content_uri);
            Assert.Contains(Utilities.BaseAddress.Host, content.content_uri);
            Assert.Matches("[A-Za-z]{24}$", content.content_uri);

            // Download should also succeed.
            string server_name = Utilities.BaseAddress.Host;
            string media_id = content.content_uri[(content.content_uri.LastIndexOf('/') + 1)..];
            var download_response = await client.GetAsync("/_matrix/media/r0/download/" + server_name + "/" + media_id);

            Assert.Equal(HttpStatusCode.OK, download_response.StatusCode);
            Assert.Equal("sandbox; default-src 'none'; script-src 'none'; plugin-types application/pdf; style-src 'unsafe-inline'; object-src 'self';",
                download_response.Headers.GetValues("Content-Security-Policy").First());
            Assert.Equal("the_hello.txt", download_response.Content.Headers.ContentDisposition?.FileName);

            string dl_content = await download_response.Content.ReadAsStringAsync();
            Assert.Equal("Hello there!", dl_content);
        }

        [Fact]
        public async Task TestDownload_InvalidParameter()
        {
            string server_name = Utilities.BaseAddress.Host;
            var response = await client.GetAsync("/_matrix/media/r0/download/" + server_name + "/foo?allow_remote=blah");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_INVALID_PARAM",
                error = "Query parameter 'allow_remote' must be either 'true' or 'false'."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestDownload_RemoteWhenRemoteIsNotAllowed()
        {
            var response = await client.GetAsync("/_matrix/media/r0/download/matrix.example.org/foo?allow_remote=false");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_NOT_FOUND",
                error = "Content was not found, because fetching from remote was not allowed."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestDownload_RemoteWhenRemoteIsActuallyAllowed()
        {
            var response = await client.GetAsync("/_matrix/media/r0/download/matrix.example.org/foo?allow_remote=true");

            Assert.Equal(HttpStatusCode.BadGateway, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_TOO_LARGE",
                error = "Content fetching from other servers is not implemented."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestDownload_NonExistentContent()
        {
            string server_name = Utilities.BaseAddress.Host;
            var response = await client.GetAsync("/_matrix/media/r0/download/" + server_name + "/fooNotHere");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_NOT_FOUND",
                error = "Content was not found on the server."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestDownload_ExistingContent()
        {
            string server_name = Utilities.BaseAddress.Host;
            var response = await client.GetAsync("/_matrix/media/r0/download/" + server_name + "/testDownload");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("text/plain", response.Content.Headers.ContentType?.MediaType);
            Assert.Equal("sandbox; default-src 'none'; script-src 'none'; plugin-types application/pdf; style-src 'unsafe-inline'; object-src 'self';",
                response.Headers.GetValues("Content-Security-Policy").First());

            string content = await response.Content.ReadAsStringAsync();
            Assert.Equal("Hello, test code. :)", content);
        }
    }
}
