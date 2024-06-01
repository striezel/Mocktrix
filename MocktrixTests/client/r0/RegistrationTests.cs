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
    public class RegistrationTests
    {
        private readonly HttpClient client = new()
        {
            BaseAddress = Utilities.BaseAddress
        };


        [Fact]
        public async Task TestMailRegisterToken()
        {
            var postData = new
            { 
                client_secret = "not so secret",
                email = "alice@example.org",
                send_attempt = 1,
                next_link = "https://example.org/congratulations.html",
                id_server = "id.example.com",
                id_access_token = "some_string"
            };

            var response = await client.PostAsync("/_matrix/client/r0/register/email/requestToken", JsonContent.Create(postData));

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_THREEPID_DENIED",
                error = "Third party identifier is not allowed."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }


        [Fact]
        public async Task TestPhoneRegisterToken()
        {
            var postData = new
            {
                client_secret = "not so secret",
                country = "GB",
                phone_number = "07700900001",
                send_attempt = 1,
                next_link = "https://example.org/congratulations.html",
                id_server = "id.example.com",
                id_access_token = "some_string"
            };

            var response = await client.PostAsync("/_matrix/client/r0/register/msisdn/requestToken", JsonContent.Create(postData));

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_THREEPID_DENIED",
                error = "Third party identifier is not allowed."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }
    }
}
