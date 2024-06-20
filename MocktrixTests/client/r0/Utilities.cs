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

using System.Net.Http.Json;
using System.Text.Json;

namespace MocktrixTests
{
    /// <summary>
    /// Contains utilities to ease testing.
    /// </summary>
    internal class Utilities
    {
        /// <summary>
        /// Gets the base address for the server to test against.
        /// </summary>
        public static Uri BaseAddress
        {
            get
            {
                return new Uri("http://localhost:5289");
            }
        }


        /// <summary>
        /// Returns the deserialized JSON content from a HTTP response.
        /// </summary>
        /// <typeparam name="T">type of the returned object</typeparam>
        /// <param name="response">the actual (successful) HTTP response</param>
        /// <param name="_">dummy parameter to allow type deduction</param>
        /// <returns>Returns the deserialized response.</returns>
        public static T GetContent<T>(HttpResponseMessage response, T _)
        {
            return JsonSerializer.Deserialize<T?>(response.Content.ReadAsStream());
        }


        /// <summary>
        /// Performs a login with the given credentials.
        /// </summary>
        /// <param name="client">the HTTPClient instance with properly set base address</param>
        /// <param name="userId">Matrix user id of the user to log in</param>
        /// <param name="password">the user's password</param>
        /// <returns>Returns the access token retrieved from the server.</returns>
        public static async Task<string> PerformLogin(HttpClient client, string userId = "@alice:matrix.example.org", string password = "secret password")
        {
            var body = new
            {
                type = "m.login.password",
                identifier = new
                {
                    type = "m.id.user",
                    user = userId
                },
                password = password,
                initial_device_display_name = "My device"
            };
            var login_response = await client.PostAsync("/_matrix/client/r0/login", JsonContent.Create(body));
            var login_data = new
            {
                user_id = "@alice:matrix.example.org",
                access_token = "random ...",
                device_id = "also random ..."
            };
            var login_content = GetContent(login_response, login_data);
            return login_content.access_token;
        }
    }
}
