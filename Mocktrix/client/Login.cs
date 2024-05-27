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

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mocktrix.client
{
#pragma warning disable IDE1006
    internal record LoginFlow(string type);
#pragma warning restore IDE1006


    public class UserIdentifier
    {
        /// <summary>
        /// The type of identification. Must be "m.id.user" for Matrix user IDs.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// User id or user localpart.
        /// </summary>
        [JsonPropertyName("user")]
        public string User { get; set; } = string.Empty;
    }

    public class LoginPostData
    {
        /// <summary>
        /// The login type being used. One of: ["m.login.password", "m.login.token"].
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("identifier")]
        public UserIdentifier? Identifier { get; set; } = null;

        /// <summary>
        /// The fully qualified user ID or just local part of the user ID, to log in. Deprecated in favour of Identifier.
        /// </summary>
        [JsonPropertyName("user")]
        public string? User { get; set; } = null;

        /// <summary>
        /// Password of the user. Required for type "m.login.password".
        /// </summary>
        [JsonPropertyName("password")]
        public string? Password { get; set; } = null;

        /// <summary>
        /// Used in token-based login. Required when type is "m.login.token".
        /// </summary>
        [JsonPropertyName("token")]
        public string? Token { get; set; } = null;

        /// <summary>
        /// ID of the client device. If this does not correspond to a known
        /// client device, a new device will be created. The server will auto-
        /// generate a device_id if this is not specified.
        /// </summary>
        [JsonPropertyName("device_id")]
        public string? DeviceId { get; set; } = null;

        /// <summary>
        /// A display name to assign to the newly-created device.
        /// Ignored if DeviceId corresponds to a known device.
        /// </summary>
        [JsonPropertyName("initial_device_display_name")]
        public string? InitialDeviceDisplayName { get; set; } = null;
    }

    public struct HomeserverInformation
    {
        /// <summary>
        /// The base URL for the homeserver for client-server connections, e. g.
        /// "https://matrix.example.org".
        /// </summary>
        [JsonPropertyName("base_url")]
        public string BaseUrl { get; set; }
    }

    public struct DiscoveryInformation
    {
        [JsonPropertyName("m.homeserver")]
        public HomeserverInformation Homeserver { get; set; }
    }


    public class Login
    {
        /// <summary>
        /// Adds login-related endpoints to the web application.
        /// </summary>
        /// <param name="app">the app to which the endpoints shall be added</param>
        public static void AddEndpoints(WebApplication app)
        {
            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-login.
            app.MapGet("/_matrix/client/r0/login", (HttpContext httpContext) =>
            {
                var flows = new
                {
                    flows = new List<LoginFlow>(1)
                    {
                        new("m.login.password")
                    }
                };
                return flows;
            });

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-login.
            app.MapPost("/_matrix/client/r0/login", async (HttpContext context) =>
            {
                const int maxReadSize = 10000;
                if (context.Request.ContentLength is not null && context.Request.ContentLength > maxReadSize)
                {
                    return Results.BadRequest(new {
                        errcode = "M_TOO_LARGE",
                        error = "Request body is too large."
                    });
                }
                if (context.Request.ContentType is null || !context.Request.ContentType.StartsWith("application/json"))
                {
                    return Results.BadRequest(new
                    {
                        errcode = "M_UNKNOWN",
                        error = "Content type of the request was not set to application/json."
                    });
                }

                var options = new JsonSerializerOptions(JsonSerializerOptions.Default)
                {
                    AllowTrailingCommas = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                };
                var data = await context.Request.ReadFromJsonAsync<LoginPostData>(options);
                if (data == null)
                {
                    return Results.BadRequest(new
                    {
                        errcode = "M_NOT_JSON",
                        error = "The request does not contain JSON or contains invalid JSON."
                    });
                }

                // Only password-based login is currently supported.
                if (data.Type != "m.login.password")
                {
                    return Results.BadRequest(new
                    {
                        errcode = "M_UNKNOWN",
                        error = "Unsupported login type specified. This server only supports password-based login (\"m.login.password\")."
                    });
                }

                string? user_id = data.Identifier?.User ?? data.User;
                if (string.IsNullOrWhiteSpace(user_id)
                    || string.IsNullOrWhiteSpace(data.Password)
                    || ((data.Identifier!= null) && (data.Identifier.Type != "m.id.user")))
                {
                    return Results.BadRequest(new
                    {
                        errcode = "M_BAD_JSON",
                        error = "User id or password is missing, or identifier.type is not \"m.id.user\"."
                    });
                }

                // Find user.
                var user = Database.Memory.Users.GetUser(user_id);
                if (user == null)
                {
                    return Results.BadRequest(new
                    {
                        errcode = "M_INVALID_USERNAME",
                        error = "User id does not exist on this server."
                    });
                }

                // Verify login data.
                if (string.IsNullOrWhiteSpace(data.Password) ||
                    utilities.Hashing.HashPassword(data.Password, user.salt) != user.password_hash)
                {
                    return Results.StatusCode(403);
                    // TODO: Add JSON output, like:
                    //  {
                    //    errcode: "M_FORBIDDEN"
                    //  }
                }

                // Handle device.
                Data.Device? device;
                if (!string.IsNullOrWhiteSpace(data.DeviceId))
                {
                    device = Database.Memory.Devices.GetDevice(data.DeviceId, user_id);
                    if (device == null)
                    {
                        device = Database.Memory.Devices.CreateDevice(
                            data.DeviceId, user_id, data.InitialDeviceDisplayName);
                    }
                }
                else
                {
                    device = Database.Memory.Devices.CreateDevice(
                            Data.Device.GenerateRandomId(), user_id, data.InitialDeviceDisplayName);
                }

                // Generate access token.
                var token = Database.Memory.AccessTokens.CreateToken(user_id, device.device_id);

                var response = new {
                    user_id = user_id,
                    access_token = token.token,
                    device_id = device.device_id,
                    well_known = new DiscoveryInformation()
                    {
                        Homeserver = new HomeserverInformation()
                        {
                            BaseUrl = app.Urls.First()
                        }
                    }
                };

                return Results.Ok(response);
            });

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-logout.
            app.MapPost("/_matrix/client/r0/logout", (HttpContext context) =>
            {
                var auth = Utilities.GetAccessToken(context);
                if (string.IsNullOrWhiteSpace(auth))
                {
                    string json = "{\"errcode\":\"M_MISSING_TOKEN\",\"error\":\"Missing access token.\"}";
                    return Results.Content(json, "application/json",
                        System.Text.Encoding.UTF8, StatusCodes.Status401Unauthorized);
                }
                var token = Database.Memory.AccessTokens.Find(auth);
                if (token == null)
                {
                    string json = "{\"errcode\":\"M_UNKNOWN_TOKEN\",\"error\":\"Unknown access token.\"}";
                    return Results.Content(json, "application/json",
                        System.Text.Encoding.UTF8, StatusCodes.Status401Unauthorized);
                }

                // Revoke access token.
                _ = Database.Memory.AccessTokens.Revoke(token.token);
                // Delete the associated device.
                _ = Database.Memory.Devices.Remove(token.device_id, token.user_id);

                return Results.Ok(new { });
            });

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-logout-all.
            app.MapPost("/_matrix/client/r0/logout/all", (HttpContext context) =>
            {
                var auth = Utilities.GetAccessToken(context);
                if (string.IsNullOrWhiteSpace(auth))
                {
                    string json = "{\"errcode\":\"M_MISSING_TOKEN\",\"error\":\"Missing access token.\"}";
                    return Results.Content(json, "application/json",
                        System.Text.Encoding.UTF8, StatusCodes.Status401Unauthorized);
                }
                var access_token = Database.Memory.AccessTokens.Find(auth);
                if (access_token == null)
                {
                    string json = "{\"errcode\":\"M_UNKNOWN_TOKEN\",\"error\":\"Unknown access token.\"}";
                    return Results.Content(json, "application/json",
                        System.Text.Encoding.UTF8, StatusCodes.Status401Unauthorized);
                }

                var all_tokens_of_user = Database.Memory.AccessTokens.FindByUser(access_token.user_id);
                foreach (var token in all_tokens_of_user)
                {
                    // Revoke access token.
                    _ = Database.Memory.AccessTokens.Revoke(token.token);
                    // Delete the associated device.
                    _ = Database.Memory.Devices.Remove(token.device_id, token.user_id);
                }

                return Results.Ok(new { });
            });
        }
    }
}
