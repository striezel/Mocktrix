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

using Mocktrix.Protocol.Types;

namespace Mocktrix.client.r0_6_1
{
    /// <summary>
    /// Contains implementation of profile-related endpoints for version r0.6.1.
    /// </summary>
    public static class Profile
    {
        /// <summary>
        /// Adds profile-related endpoints to the web application.
        /// </summary>
        /// <param name="app">the app to which the endpoints shall be added</param>
        public static void AddEndpoints(WebApplication app)
        {
            // Implements https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-profile-userid-displayname,
            // i. e. the endpoint to query a user's display name.
            app.MapGet("/_matrix/client/r0/profile/{userId}/displayname", (string userId, HttpContext context) =>
            {
                // TODO: Implement lookup for cases where user id is on a
                // different homeserver.

                var user = Database.Memory.Users.GetUser(userId);
                if (user == null)
                {
                    var response = new ErrorResponse
                    {
                        errcode = "M_NOT_FOUND",
                        error = "The user profile was not found."
                    };
                    return Results.NotFound(response);
                }
                if (string.IsNullOrWhiteSpace(user.display_name))
                {
                    return Results.Ok(new { });
                }

                var data = new 
                {
                    displayname = user.display_name
                };

                // Return the display name.
                return Results.Ok(data);
            });
        }
    }
}
