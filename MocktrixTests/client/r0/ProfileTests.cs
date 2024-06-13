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
    public class ProfileTests
    {
        private readonly HttpClient client = new()
        {
            BaseAddress = Utilities.BaseAddress
        };

        [Fact]
        public async Task TestRetrieveDisplayName_NonExistentUser()
        {
            var response = await client.GetAsync("/_matrix/client/r0/profile/@does-not-exist:" + client.BaseAddress?.Host + "/displayname");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_NOT_FOUND",
                error = "The user profile was not found."
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestRetrieveDisplayName_ExistingUserWithoutDisplayName()
        {
            var response = await client.GetAsync("/_matrix/client/r0/profile/@unnamed_user:" + client.BaseAddress?.Host + "/displayname");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("{}", content);
        }

        [Fact]
        public async Task TestRetrieveDisplayName_ExistingUserWithDisplayName()
        {
            var response = await client.GetAsync("/_matrix/client/r0/profile/@named_user:" + client.BaseAddress?.Host + "/displayname");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                displayname = "Nomen Nominandum"
            };
            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.displayname, content.displayname);
        }
    }
}
