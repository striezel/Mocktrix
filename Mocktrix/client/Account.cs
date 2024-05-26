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
    public class Account
    {
        /// <summary>
        /// Adds account management endpoint to the web application.
        /// </summary>
        /// <param name="app">the app to which the endpoint shall be added</param>
        public static void AddEndpoints(WebApplication app)
        {
            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-account-whoami.
            app.MapGet("/_matrix/client/r0/account/whoami", (HttpContext context) =>
            {
                var auth = context.Request.Headers.Authorization.Count > 0 ? context.Request.Headers.Authorization.First() : "";
                if (string.IsNullOrWhiteSpace(auth) || !auth.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Results.Unauthorized();
                }
                auth = auth.Remove(0, "Bearer ".Length);
                var token = Database.Memory.AccessTokens.Find(auth);
                if (token == null)
                {
                    return Results.Unauthorized();
                }

                // Token was found.
                return Results.Ok(new { token.user_id });
            });
        }
    }
}
