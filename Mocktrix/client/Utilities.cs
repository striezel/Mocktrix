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

namespace Mocktrix.client
{
    /// <summary>
    /// Small utilities to ease handling of client API requests.
    /// </summary>
    internal class Utilities
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
    }
}
