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

using Mocktrix.Data;
using Mocktrix.Database.Memory;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mocktrix.client.r0_6_1
{
    /// <summary>
    /// Data posted during account registration.
    /// </summary>
    internal class RegistrationData
    {
        /// <summary>
        /// Basically the localpart of the Matrix ID.
        /// </summary>
        [JsonPropertyName("username")]
        public string? UserName { get; set; }

        /// <summary>
        /// Desired password for the account.
        /// </summary>
        [JsonPropertyName("password")]
        public string? Password { get; set; }

        /// <summary>
        /// ID of the client device.
        /// </summary>
        [JsonPropertyName("device_id")]
        public string? DeviceId { get; set; }

        /// <summary>
        /// Display name for the newly created device.
        /// </summary>
        [JsonPropertyName("initial_device_display_name")]
        public string? InitialDeviceDisplayName { get; set; }

        /// <summary>
        /// If set to true, no access token and device ID are returned from
        /// this call to prevent automatic login.
        /// </summary>
        [JsonPropertyName("inhibit_login")]
        public bool? InhibitLogin { get; set; }
    }


    /// <summary>
    /// Contains implementation of account registration endpoints for version r0.6.1.
    /// </summary>
    public static class Registration
    {
        /// <summary>
        /// Checks whether a username only contains valid characters.
        /// </summary>
        /// <param name="username">the username to validate</param>
        /// <returns>Returns true, if the username is valid.</returns>
        private static bool IsValidUserName(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            { 
                return false;
            }
            var valid_char = (char c) =>
            {
                return char.IsAsciiLetterOrDigit(c) || c == '_'
                || c == '-' || c == '.';
            };
            return username.All(valid_char);
        }

        /// <summary>
        /// Gets the full Matrix user ID from a given localpart.
        /// </summary>
        /// <param name="app">the app that runs the homeserver</param>
        /// <param name="localpart">the localpart of the user ID</param>
        /// <returns>Returns the full Matrix user ID for the current server.</returns>
        private static string ExtendLocalpartToUserId(WebApplication app, string localpart)
        {
            var server_address = new Uri(app.Urls.FirstOrDefault("http://localhost/"));
            return "@" + localpart + ":" + server_address.Host;
        }

        /// <summary>
        /// Checks whether a username is still available.
        /// </summary>
        /// <param name="app">the app that runs the homeserver</param>
        /// <param name="username">the username to check for availability</param>
        /// <returns>Returns true, if the name is still available.</returns>
        private static bool UsernameAvailable(WebApplication app, string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return false;
            }
            var user_id = ExtendLocalpartToUserId(app, username);
            return Database.Memory.Users.GetUser(user_id) == null;
        }

        /// <summary>
        /// Generates a new user id.
        /// </summary>
        /// <returns>Returns a new user id.</returns>
        private static string GenerateUserId()
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyz".AsSpan();
            return "user_" + RandomNumberGenerator.GetString(alphabet, 12);
        }

        /// <summary>
        /// Adds account registration endpoints to the web application.
        /// </summary>
        /// <param name="app">the app to which the endpoint shall be added</param>
        public static void AddEndpoints(WebApplication app)
        {
            var NotSupported = (HttpContext context) =>
            {
                // This registration method is not allowed / not implemented.
                var error = new
                {
                    errcode = "M_THREEPID_DENIED",
                    error = "Third party identifier is not allowed."
                };
                return Results.Json(error, statusCode: StatusCodes.Status403Forbidden);
            };

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-register-email-requesttoken.
            app.MapPost("/_matrix/client/r0/register/email/requestToken", NotSupported);

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-register-msisdn-requesttoken.
            app.MapPost("/_matrix/client/r0/register/msisdn/requestToken", NotSupported);

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-register-available.
            app.MapGet("/_matrix/client/r0/register/available", (HttpContext context) =>
            {
                if (!context.Request.Query.ContainsKey("username"))
                {
                    return Results.BadRequest(new
                    {
                        errcode = "M_MISSING_PARAM",
                        error = "Query parameter 'username' is missing."
                    });
                }

                string? username = context.Request.Query["username"].FirstOrDefault("")?.Trim();
                if (string.IsNullOrWhiteSpace(username))
                {
                    return Results.BadRequest(new
                    {
                        errcode = "M_INVALID_USERNAME",
                        error = "User name cannot be empty."
                    });
                }

                if (!IsValidUserName(username))
                {
                    return Results.BadRequest(new
                    {
                        errcode = "M_INVALID_USERNAME",
                        error = "User ID can only contain the characters a-z, 0-9, '.', '-' and '_'."
                    });
                }

                if (!UsernameAvailable(app, username))
                {
                    return Results.BadRequest(new
                    {
                        errcode = "M_USER_IN_USE",
                        error = "User ID is already used by someone else."
                    });
                }

                // User id is still available.
                return Results.Json(new { available = true });
            });

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-register.
            app.MapPost("/_matrix/client/r0/register", async (HttpContext context) =>
            {
                string kind;
                if (context.Request.Query.ContainsKey("kind"))
                {
                    kind = context.Request.Query["kind"].First() ?? "";
                }
                else
                {
                    kind = "user";
                }
                if (kind != "user" && kind != "guest")
                {
                    return Results.BadRequest(new
                    {
                        errcode = "M_UNRECOGNIZED",
                        error = "Membership kind must be either 'user' or 'guest'."
                    });
                }
                // Currently, guest accounts are not supported.
                if (kind == "guest")
                {
                    return Results.Json(new
                    {
                        errcode = "M_FORBIDDEN",
                        error = "Registration of guest accounts is not allowed."
                    },
                    statusCode: StatusCodes.Status403Forbidden);
                }
                var options = new JsonSerializerOptions(JsonSerializerOptions.Default)
                {
                    AllowTrailingCommas = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                };
                RegistrationData? data;
                try
                {
                    data = await context.Request.ReadFromJsonAsync<RegistrationData>(options);
                }
                catch (Exception)
                {
                    data = null;
                }
                if (data == null)
                {
                    return Results.BadRequest(new
                    {
                        errcode = "M_NOT_JSON",
                        error = "The request does not contain JSON or contains invalid JSON."
                    });
                }

                string username = data.UserName ?? GenerateUserId();
                if (!IsValidUserName(username))
                {
                    return Results.BadRequest(new
                    {
                        errcode = "M_INVALID_USERNAME",
                        error = "User ID can only contain the characters a-z, 0-9, '.', '-' and '_'."
                    });
                }

                if (!UsernameAvailable(app, username))
                {
                    return Results.BadRequest(new
                    {
                        errcode = "M_USER_IN_USE",
                        error = "User ID is already used by someone else."
                    });
                }

                string password = data.Password ?? string.Empty;
                if (string.IsNullOrWhiteSpace(password) || password.Length < 12)
                {
                    return Results.BadRequest(new
                    {
                        errcode = "M_WEAK_PASSWORD",
                        error = "No password was specified, or the provided password is too weak."
                    });
                }

                User user = Users.CreateUser(ExtendLocalpartToUserId(app, username), password);

                bool inhibit_login = data.InhibitLogin.GetValueOrDefault(false);
                if (inhibit_login)
                {
                    // We are done here, if login is inhibitted.
                    return Results.Ok(new { user_id = user.user_id });
                }

                string device_id = data.DeviceId ?? Device.GenerateRandomId();
                Device? device = Devices.GetDevice(device_id, user.user_id);
                if (device == null)
                {
                    device = Devices.CreateDevice(device_id, user.user_id, data.InitialDeviceDisplayName);
                }
                var token = AccessTokens.CreateToken(user.user_id, device_id);

                return Results.Ok(new
                {
                    user_id = user.user_id,
                    device_id = device_id,
                    access_token = token.token
                });
            });
        }
    }
}
