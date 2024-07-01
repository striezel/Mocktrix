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
using Mocktrix.Protocol.Types.Capabilities;

namespace Mocktrix.client.r0_6_1
{
    /// <summary>
    /// Contains implementation of capabilities endpoints for version r0.6.1.
    /// </summary>
    public static class Capabilities
    {
        /// <summary>
        /// Adds capabilities negotiation endpoint to the web application.
        /// </summary>
        /// <param name="app">the app to which the endpoint shall be added</param>
        public static void AddEndpoints(WebApplication app)
        {
            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-capabilities,
            // i.e. the endpoint to get information about the server's
            // capabilities and features.
            app.MapGet("/_matrix/client/r0/capabilities", (HttpContext context) =>
            {
                var access_token = Utilities.GetAccessToken(context);
                if (string.IsNullOrWhiteSpace(access_token))
                {
                    var error = new ErrorResponse
                    {
                        errcode = "M_MISSING_TOKEN",
                        error = "Missing access token."
                    };
                    return Results.Json(error, statusCode: StatusCodes.Status401Unauthorized);
                }
                var token = Database.Memory.AccessTokens.Find(access_token);
                if (token == null)
                {
                    var error = new ErrorResponse
                    {
                        errcode = "M_UNKNOWN_TOKEN",
                        error = "Unrecognized access token."
                    };
                    return Results.Json(error, statusCode: StatusCodes.Status401Unauthorized);
                }

                // Token was found, so get capabilities.
                var result = new
                {
                    capabilities = new ServerCapabilities
                    {
                        ChangePassword = new ChangePasswordCapability { Enabled = true },
                        RoomVersions = new RoomVersionsCapability
                        {
                            DefaultVersion = "1",
                            Available = new Dictionary<string, string>(3)
                            {
                                { "1", "stable" },
                                { "2", "unstable" },
                                { "3", "unstable" },
                            }
                        }
                    }
                };
                return Results.Ok(result);
            });
        }
    }
}
