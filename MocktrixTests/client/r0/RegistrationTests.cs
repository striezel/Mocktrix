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


        [Fact]
        public async Task TestAvailable_NoUserNameGiven()
        {
            var response = await client.GetAsync("/_matrix/client/r0/register/available");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_MISSING_PARAM",
                error = "Query parameter 'username' is missing."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }


        [Fact]
        public async Task TestAvailable_EmptyUserName()
        {
            var response = await client.GetAsync("/_matrix/client/r0/register/available?username=");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_INVALID_USERNAME",
                error = "User name cannot be empty."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }


        [Fact]
        public async Task TestAvailable_UserNameWithNonAsciiCharacters()
        {
            var response = await client.GetAsync("/_matrix/client/r0/register/available?username=smørrebrød");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_INVALID_USERNAME",
                error = "User ID can only contain the characters a-z, 0-9, '.', '-' and '_'."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }


        [Fact]
        public async Task TestAvailable_UserNameWithDisallowedCharacters()
        {
            var response = await client.GetAsync("/_matrix/client/r0/register/available?username=no spaces allowed");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_INVALID_USERNAME",
                error = "User ID can only contain the characters a-z, 0-9, '.', '-' and '_'."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }


        [Fact]
        public async Task TestAvailable_UserNameAlreadyTaken()
        {
            var response = await client.GetAsync("/_matrix/client/r0/register/available?username=alice");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_USER_IN_USE",
                error = "User ID is already used by someone else."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }


        [Fact]
        public async Task TestAvailable_UserNameStillAvailable()
        {
            var response = await client.GetAsync("/_matrix/client/r0/register/available?username=not_alice_but_bob");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                available = true
            };
            var content = Utilities.GetContent(response, expected);
            Assert.True(content.available);
        }


        [Fact]
        public async Task TestRegister_UnrecognizedMembershipKind()
        {
            var data = new
            {
                username = "ray_gister",
                password = "everybody loves account registration"
            };
            var response = await client.PostAsync("/_matrix/client/r0/register?kind=foo", JsonContent.Create(data));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_UNRECOGNIZED",
                error = "Membership kind must be either 'user' or 'guest'."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }


        [Fact]
        public async Task TestRegister_GuestAccountNotAllowed()
        {
            var data = new
            {
                username = "ray_guest",
                password = "everybody loves account registration"
            };
            var response = await client.PostAsync("/_matrix/client/r0/register?kind=guest", JsonContent.Create(data));

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_FORBIDDEN",
                error = "Registration of guest accounts is not allowed."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }


        [Fact]
        public async Task TestRegister_ContentNotJSON()
        {
            var response = await client.PostAsync("/_matrix/client/r0/register?kind=user", new StringContent("foo"));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_NOT_JSON",
                error = "The request does not contain JSON or contains invalid JSON."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }


        [Fact]
        public async Task TestRegister_InvalidUserName()
        {
            var data = new
            {
                username = "in:valid üser näme",
                password = "everybody loves account registration"
            };
            var response = await client.PostAsync("/_matrix/client/r0/register?kind=user", JsonContent.Create(data));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_INVALID_USERNAME",
                error = "User ID can only contain the characters a-z, 0-9, '.', '-' and '_'."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }


        [Fact]
        public async Task TestRegister_UserNameAlreadyExists()
        {
            var data = new
            {
                username = "alice",
                password = "everybody loves account registration"
            };
            var response = await client.PostAsync("/_matrix/client/r0/register?kind=user", JsonContent.Create(data));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_USER_IN_USE",
                error = "User ID is already used by someone else."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }


        [Theory]
        [InlineData("password")]
        [InlineData("1234")]
        [InlineData("      ")]
        [InlineData("bad")]
        [InlineData("quite weak")]
        public async Task TestRegister_WeakPassword(string inline_password)
        {
            var data = new
            {
                username = "ray_weak_pass",
                password = inline_password
            };
            var response = await client.PostAsync("/_matrix/client/r0/register?kind=user", JsonContent.Create(data));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_WEAK_PASSWORD",
                error = "No password was specified, or the provided password is too weak."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }


        [Fact]
        public async Task TestRegister_NoPassword()
        {
            var data = new
            {
                username = "ray_weak_pass"
            };
            var response = await client.PostAsync("/_matrix/client/r0/register?kind=user", JsonContent.Create(data));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_WEAK_PASSWORD",
                error = "No password was specified, or the provided password is too weak."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }


        [Fact]
        public async Task TestRegister_InhibitLogin()
        {
            var data = new
            {
                username = "ray_gister_inhibit",
                password = "everybody loves account registration",
                inhibit_login = true
            };
            var response = await client.PostAsync("/_matrix/client/r0/register?kind=user", JsonContent.Create(data));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                user_id = "@ray_gister_inhibit:localhost"
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.user_id, content.user_id);

            // Login should now be possible, so test it.
            var login_body = new
            {
                type = "m.login.password",
                identifier = new
                {
                    type = "m.id.user",
                    user = data.username
                },
                password = data.password,
                initial_device_display_name = "My registered device (i)"
            };
            response = await client.PostAsync("/_matrix/client/r0/login", JsonContent.Create(login_body));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected_response = new
            {
                user_id = "@ray_gister_inhibit:localhost",
                access_token = "random ...",
                device_id = "also random ..."
            };

            var login_content = Utilities.GetContent(response, expected_response);
            Assert.NotNull(content);
            Assert.Equal(expected_response.user_id, login_content.user_id);
            Assert.NotNull(login_content.access_token);
            Assert.Matches("^[A-Z]+$", login_content.device_id);
        }


        [Fact]
        public async Task TestRegister_Success()
        {
            var data = new
            {
                username = "ray_gister",
                password = "everybody loves account registration",
                initial_device_display_name = "Yay"
            };
            var response = await client.PostAsync("/_matrix/client/r0/register?kind=user", JsonContent.Create(data));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                user_id = "@ray_gister:localhost",
                access_token = "random",
                device_id = "random"
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.user_id, content.user_id);
            Assert.NotNull(content.access_token);
            Assert.Matches("^[A-Z]+$", content.device_id);

            // Login should now be possible, so test it.
            var login_body = new
            {
                type = "m.login.password",
                identifier = new
                {
                    type = "m.id.user",
                    user = data.username
                },
                password = data.password,
                initial_device_display_name = "My registered device (i)"
            };
            response = await client.PostAsync("/_matrix/client/r0/login", JsonContent.Create(login_body));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected_response = new
            {
                user_id = "@ray_gister:localhost",
                access_token = "random ...",
                device_id = "also random ..."
            };

            var login_content = Utilities.GetContent(response, expected_response);
            Assert.NotNull(content);
            Assert.Equal(expected_response.user_id, login_content.user_id);
            Assert.NotNull(login_content.access_token);
            Assert.Matches("^[A-Z]+$", login_content.device_id);
        }
    }
}
