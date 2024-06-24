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
using Mocktrix.Protocol.Types.Account;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Mocktrix.client.r0_6_1
{
    /// <summary>
    /// Contains implementation of account management endpoints for version r0.6.1.
    /// </summary>
    public static class Account
    {
        /// <summary>
        /// Adds account management endpoint to the web application.
        /// </summary>
        /// <param name="app">the app to which the endpoint shall be added</param>
        public static void AddEndpoints(WebApplication app)
        {
            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-account-password,
            // i.e. the possibility to change the password.
            app.MapPost("/_matrix/client/r0/account/password", async (HttpContext context) =>
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

                var options = new JsonSerializerOptions(JsonSerializerOptions.Default)
                {
                    AllowTrailingCommas = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                };
                PasswordChangeData? data;
                try
                {
                    data = await context.Request.ReadFromJsonAsync<PasswordChangeData>(options);
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
                if (data.Auth == null || data.Auth.Type != "m.login.password")
                {
                    // Data for available flows looks like:
                    // {
                    //  "session": "random server-generated session ID here",
                    //  "flows": [{
                    //    "stages": ["m.login.password"]
                    //  }],
                    //  "params": {}
                    // }
                    var response = new
                    {
                        session = RandomNumberGenerator.GetString("abcdefghijklmnopqrstuvwxyz", 16),
                        flows = new[]
                        {
                          new
                          {
                              stages = new[] { "m.login.password" }
                          }
                        },
                        @params = new { }
                    };
                    return Results.Json(response, statusCode: StatusCodes.Status401Unauthorized);
                }

                string new_password = data.NewPassword ?? string.Empty;
                if (string.IsNullOrWhiteSpace(new_password) || new_password.Length < 12)
                {
                    return Results.BadRequest(new ErrorResponse
                    {
                        errcode = "M_WEAK_PASSWORD",
                        error = "New password was not specified, or the new password is too weak."
                    });
                }

                // Find user and check old password.
                var user = Database.Memory.Users.GetUser(token.user_id);
                if (user == null)
                {
                    // Should never happen. We either have a bug or memory corruption,
                    // if this branch is ever taken.
                    var response = new ErrorResponse
                    {
                        errcode = "M_UNKNOWN",
                        error = "User not found."
                    };
                    return Results.Json(response, statusCode: StatusCodes.Status500InternalServerError);
                }
                // Verify password.
                if (string.IsNullOrWhiteSpace(data.Auth.Password) ||
                    utilities.Hashing.HashPassword(data.Auth.Password, user.salt) != user.password_hash)
                {
                    return Results.Json(new ErrorResponse
                    {
                        errcode = "M_FORBIDDEN",
                        error = "Invalid password."
                    },
                    statusCode: StatusCodes.Status403Forbidden);
                }

                // Password is correct and new password is not weak, so new password can be set.
                user.password_hash = utilities.Hashing.CreateHashedSaltedPassword(new_password, out user.salt);

                // Log out other devices?
                if (data.LogoutDevices.GetValueOrDefault(true))
                {
                    var all_tokens_of_user = Database.Memory.AccessTokens.FindByUser(token.user_id);
                    foreach (var revokable_token in all_tokens_of_user)
                    {
                        if (revokable_token.token != token.token)
                        {
                            // Revoke access token.
                            _ = Database.Memory.AccessTokens.Revoke(revokable_token.token);
                            // Delete the associated device.
                            _ = Database.Memory.Devices.Remove(revokable_token.device_id, revokable_token.user_id);
                        }
                    }
                }

                // Return empty JSON object to indicate success.
                return Results.Ok(new { });
            });


            var DoNotAllowThreePID = (HttpContext context) =>
            {
                var error = new ErrorResponse
                {
                    errcode = "M_THREEPID_DENIED",
                    error = "Third-party identifier is not allowed here."
                };
                return Results.Json(error, statusCode: StatusCodes.Status403Forbidden);
            };

            var DoNotAllowThreePIDAuthRequired = (HttpContext context) =>
            {
                var access_token = Utilities.GetAccessToken(context);
                if (string.IsNullOrWhiteSpace(access_token))
                {
                    var response = new ErrorResponse
                    {
                        errcode = "M_MISSING_TOKEN",
                        error = "Missing access token."
                    };
                    return Results.Json(response, statusCode: StatusCodes.Status401Unauthorized);
                }
                var token = Database.Memory.AccessTokens.Find(access_token);
                if (token == null)
                {
                    var response = new ErrorResponse
                    {
                        errcode = "M_UNKNOWN_TOKEN",
                        error = "Unrecognized access token."
                    };
                    return Results.Json(response, statusCode: StatusCodes.Status401Unauthorized);
                }

                var error = new ErrorResponse
                {
                    errcode = "M_THREEPID_DENIED",
                    error = "Third-party identifier is not allowed here."
                };
                return Results.Json(error, statusCode: StatusCodes.Status403Forbidden);
            };

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-account-password-email-requesttoken
            // by not allowing it.
            app.MapPost("/_matrix/client/r0/account/password/email/requestToken", DoNotAllowThreePID);

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-account-password-msisdn-requesttoken
            // by not allowing it.
            app.MapPost("/_matrix/client/r0/account/password/msisdn/requestToken", DoNotAllowThreePID);

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-account-deactivate,
            // i.e. the possibility to deactivate an account.
            app.MapPost("/_matrix/client/r0/account/deactivate", async (HttpContext context) =>
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

                var options = new JsonSerializerOptions(JsonSerializerOptions.Default)
                {
                    AllowTrailingCommas = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                };
                AccountDeactivationData? data;
                try
                {
                    data = await context.Request.ReadFromJsonAsync<AccountDeactivationData>(options);
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
                if (data.Auth == null || data.Auth.Type != "m.login.password")
                {
                    // Data for available flows looks like:
                    // {
                    //  "session": "random server-generated session ID here",
                    //  "flows": [{
                    //    "stages": ["m.login.password"]
                    //  }],
                    //  "params": {}
                    // }
                    var response = new
                    {
                        session = RandomNumberGenerator.GetString("abcdefghijklmnopqrstuvwxyz", 16),
                        flows = new[]
                        {
                          new
                          {
                              stages = new[] { "m.login.password" }
                          }
                        },
                        @params = new { }
                    };
                    return Results.Json(response, statusCode: StatusCodes.Status401Unauthorized);
                }

                // Find user and check password.
                var user = Database.Memory.Users.GetUser(token.user_id);
                if (user == null)
                {
                    // Should never happen. We either have a bug or memory corruption,
                    // if this branch is ever taken.
                    var response = new ErrorResponse
                    {
                        errcode = "M_UNKNOWN",
                        error = "User not found."
                    };
                    return Results.Json(response, statusCode: StatusCodes.Status500InternalServerError);
                }
                // Verify password.
                if (string.IsNullOrWhiteSpace(data.Auth.Password) ||
                    utilities.Hashing.HashPassword(data.Auth.Password, user.salt) != user.password_hash)
                {
                    return Results.Json(new ErrorResponse
                    {
                        errcode = "M_FORBIDDEN",
                        error = "Invalid password."
                    },
                    statusCode: StatusCodes.Status403Forbidden);
                }

                // Password is correct, so the account can be deactivated.
                user.inactive = true;

                // Log out all devices.
                var all_tokens_of_user = Database.Memory.AccessTokens.FindByUser(token.user_id);
                foreach (var revokable_token in all_tokens_of_user)
                {

                    // Revoke access token.
                    _ = Database.Memory.AccessTokens.Revoke(revokable_token.token);
                    // Delete the associated device.
                    _ = Database.Memory.Devices.Remove(revokable_token.device_id, revokable_token.user_id);
                }

                // Return success.
                return Results.Ok(new { id_server_unbind_result = "success" });
            });

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-account-3pid,
            // i.e. gets list of associated third-party ids for the user id
            // associated with an access token.
            app.MapGet("/_matrix/client/r0/account/3pid", (HttpContext context) =>
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

                // Third-party ids are not implemented, so the list is always empty.
                return Results.Ok(new { threepid = new List<object>(0) { } });
            });

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-account-3pid-add
            // by not allowing it.
            app.MapPost("/_matrix/client/r0/account/3pid/add", DoNotAllowThreePIDAuthRequired);

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-account-3pid-bind
            // by not allowing it.
            app.MapPost("/_matrix/client/r0/account/3pid/bind", DoNotAllowThreePIDAuthRequired);

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-account-3pid-delete
            // by not allowing it.
            app.MapPost("/_matrix/client/r0/account/3pid/delete", DoNotAllowThreePIDAuthRequired);

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-account-3pid-unbind
            // by not allowing it.
            app.MapPost("/_matrix/client/r0/account/3pid/unbind", DoNotAllowThreePIDAuthRequired);

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-account-3pid-email-requesttoken
            // by not allowing it.
            app.MapPost("/_matrix/client/r0/account/3pid/email/requestToken", DoNotAllowThreePID);

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-account-3pid-msisdn-requesttoken
            // by not allowing it.
            app.MapPost("/_matrix/client/r0/account/3pid/msisdn/requestToken", DoNotAllowThreePID);

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-account-whoami,
            // i.e. gets the user id associated with an access token.
            app.MapGet("/_matrix/client/r0/account/whoami", (HttpContext context) =>
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

                // Token was found.
                return Results.Ok(new { token.user_id });
            });
        }
    }
}
