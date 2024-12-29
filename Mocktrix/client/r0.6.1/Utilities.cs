/*
    This file is part of Mocktrix.
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

using System.Text;

namespace Mocktrix.client.r0_6_1
{
    /// <summary>
    /// Small utilities to ease handling of client API requests.
    /// </summary>
    internal static class Utilities
    {
        /// <summary>
        /// Retrieves the client API access token from a request's context.
        /// </summary>
        /// <param name="context">context of the incoming request</param>
        /// <returns>Returns the access token, if one is given.
        /// Returns null, if no token exists.</returns>
        public static string? GetAccessToken(HttpContext context)
        {
            if (context.Request.Headers.Authorization.Count > 0)
            {
                var auth = context.Request.Headers.Authorization.First();
                if (string.IsNullOrWhiteSpace(auth) || !auth.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }
                return auth.Remove(0, "Bearer ".Length);
            }

            // No Authorization header present, so no access token is there.
            return null;
        }

        /// <summary>
        /// Reads the whole body of a request into a string.
        /// </summary>
        /// <param name="context">context of the incoming request</param>
        /// <returns>Returns the content of the request body as string.</returns>
        public static async Task<string> GetRequestBodyAsString(HttpContext context)
        {
            if (!context.Request.Body.CanSeek)
            {
                context.Request.EnableBuffering();
            }
            context.Request.Body.Position = 0;

            var reader = new StreamReader(context.Request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();

            // Rewind, in case somebody else uses it later.
            context.Request.Body.Position = 0;

            return body;
        }
    }
}
