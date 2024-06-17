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
using Mocktrix.Protocol.Types.Profile;
using System.Text.Json.Serialization;
using System.Text.Json;

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

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#put-matrix-client-r0-profile-userid-displayname,
            // i. e. the possibility to change the own display name.
            app.MapPut("/_matrix/client/r0/profile/{userId}/displayname", async (string userId, HttpContext context) =>
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

                // User id in URL and user id of token must match. Otherwise
                // somebody is trying to change someone else's profile data.
                if (token.user_id != userId)
                {
                    return Results.BadRequest(new ErrorResponse
                    {
                        errcode = "M_FORBIDDEN",
                        error = "Changing someone else's profile is not allowed."
                    });
                }

                DisplayNameChangeData? data = null;
                try
                {
                    data = await context.Request.ReadFromJsonAsync<DisplayNameChangeData>();
                }
                catch (Exception)
                {
                    data = null;
                }
                if (data == null)
                {
                    return Results.BadRequest(new ErrorResponse
                    {
                        errcode = "M_NOT_JSON",
                        error = "The request does not contain JSON or contains invalid JSON."
                    });
                }

                if (string.IsNullOrWhiteSpace(data.DisplayName))
                {
                    data.DisplayName = null;
                }

                var user = Database.Memory.Users.GetUser(token.user_id);
                if (user != null)
                {
                    user.display_name = data.DisplayName;
                }
                return Results.Ok(new { });
            });

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-profile-userid-avatar-url,
            // i. e. the possibility to query a user's avatar URL.
            app.MapGet("/_matrix/client/r0/profile/{userId}/avatar_url", (string userId, HttpContext context) =>
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
                if (string.IsNullOrWhiteSpace(user.avatar_url))
                {
                    return Results.Ok(new { });
                }

                var data = new
                {
                    avatar_url = user.avatar_url
                };

                // Return the display name.
                return Results.Ok(data);
            });

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#put-matrix-client-r0-profile-userid-avatar-url,
            // i. e. the possibility to change the own avatar URL.
            app.MapPut("/_matrix/client/r0/profile/{userId}/avatar_url", async (string userId, HttpContext context) =>
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

                // User id in URL and user id of token must match. Otherwise
                // somebody is trying to change someone else's profile data.
                if (token.user_id != userId)
                {
                    return Results.BadRequest(new ErrorResponse
                    {
                        errcode = "M_FORBIDDEN",
                        error = "Changing someone else's profile is not allowed."
                    });
                }

                AvatarUrlChangeData? data = null;
                try
                {
                    data = await context.Request.ReadFromJsonAsync<AvatarUrlChangeData>();
                }
                catch (Exception)
                {
                    data = null;
                }
                if (data == null)
                {
                    return Results.BadRequest(new ErrorResponse
                    {
                        errcode = "M_NOT_JSON",
                        error = "The request does not contain JSON or contains invalid JSON."
                    });
                }

                if (string.IsNullOrWhiteSpace(data.AvatarUrl))
                {
                    data.AvatarUrl = null;
                }

                var user = Database.Memory.Users.GetUser(token.user_id);
                if (user != null)
                {
                    user.avatar_url = data.AvatarUrl;
                }
                return Results.Ok(new { });
            });

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-profile-userid,
            // i. e. the endpoint to get a user's profile information.
            app.MapGet("/_matrix/client/r0/profile/{userId}", (string userId, HttpContext context) =>
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

                var data = new
                {
                    avatar_url = user.avatar_url,
                    displayname = user.display_name
                };
                var options = new JsonSerializerOptions(JsonSerializerOptions.Default)
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                // Return the profile data.
                return Results.Json(data, options);
            });
        }
    }
}
