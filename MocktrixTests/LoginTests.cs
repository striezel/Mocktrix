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
    public class LoginTests
    {
        private readonly HttpClient client = new()
        {
            BaseAddress = Utilities.BaseAddress
        };

        internal record LoginFlow(string type);

        [Fact]
        public async Task TestAvailableLoginFlows()
        {
            var response = await client.GetAsync("/_matrix/client/r0/login");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var flows = new
            {
                flows = new List<LoginFlow>(1)
                    {
                        new("m.login.password")
                    }
            };

            var content = Utilities.GetContent(response, flows);
            Assert.NotNull(content);
            Assert.NotNull(content.flows);
            Assert.Equal(flows.flows.Count, content.flows.Count);
            Assert.Equal(flows.flows[0], content.flows[0]);
        }
    }
}
