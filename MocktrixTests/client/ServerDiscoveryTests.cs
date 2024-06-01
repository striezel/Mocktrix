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

using Mocktrix.Protocol.Types;
using System.Net;

namespace MocktrixTests
{
    public class ServerDiscoveryTests
    {
        private readonly HttpClient client = new()
        {
            BaseAddress = Utilities.BaseAddress
        };

        [Fact]
        public async Task TestDisoveryInformation()
        {
            var response = await client.GetAsync("/.well-known/matrix/client");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var expected = new DiscoveryInformation
            {
                Homeserver = new HomeserverInformation()
                {
                   BaseUrl = "http://localhost:5289"
                }
            };

            var content = Utilities.GetContent(response, expected);
            
            Assert.Equal(expected.Homeserver.BaseUrl, content.Homeserver.BaseUrl);
        }
    }
}