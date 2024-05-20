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
    public class VersionsTests
    {
        private readonly HttpClient client = new()
        {
            BaseAddress = Utilities.BaseAddress
        };

        [Fact]
        public async Task TestSupportedSpecVersions()
        {
            var response = await client.GetAsync("/_matrix/client/versions");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var versions = new
            {
                versions = new List<string>(1)
                {
                   "r0.6.1"
                }
            };

            var content = Utilities.GetContent(response, versions);
            Assert.NotNull(content);
            Assert.NotNull(content.versions);
            Assert.Equal(versions.versions.Count, content.versions.Count);
            Assert.Equal(versions.versions[0], content.versions[0]);
        }
    }
}