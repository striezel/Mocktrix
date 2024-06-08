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
using Mocktrix.Protocol.Types.DeviceManagement;
using System.Text.Json;

namespace Mocktrix.client.r0_6_1
{
    /// <summary>
    /// Contains implementation of device management endpoints for version r0.6.1.
    /// </summary>
    public static class DeviceManagement
    {
        /// <summary>
        /// Adds device management endpoints to the web application.
        /// </summary>
        /// <param name="app">the app to which the endpoint shall be added</param>
        public static void AddEndpoints(WebApplication app)
        {
            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-devices,
            // i. e. the endpoint to list all devices of the user.
            app.MapGet("/_matrix/client/r0/devices", (HttpContext context) =>
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

                var devices = Database.Memory.Devices.GetDevicesOfUser(token.user_id);
                
                var result = new List<DeviceData>(devices.Count);
                foreach (var device in devices)
                {
                    result.Add(new DeviceData()
                    {
                        DeviceId = device.device_id,
                        DisplayName = device.display_name,
                        LastSeenIP = null,
                        LastSeenTimestamp = null
                    });
                }
                return Results.Ok(new { devices = result });
            });


            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-devices-deviceid,
            // i. e. the endpoint to get information about a specific device.
            app.MapGet("/_matrix/client/r0/devices/{deviceId}", (string deviceId, HttpContext context) =>
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

                Data.Device? dev = Database.Memory.Devices.GetDevice(deviceId, token.user_id);
                if (dev == null)
                {
                    return Results.NotFound(new ErrorResponse
                    {
                        errcode = "M_NOT_FOUND",
                        error = "Device not found"
                    });
                }

                // Device was found.
                return Results.Ok(new DeviceData
                {
                    DeviceId = dev.device_id,
                    DisplayName = dev.display_name,
                    LastSeenIP = null,
                    LastSeenTimestamp = null
                });
            });

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#put-matrix-client-r0-devices-deviceid,
            // i. e. the possibility to chance the display name of a device.
            app.MapPut("/_matrix/client/r0/devices/{deviceId}", async (string deviceId, HttpContext context) =>
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

                Data.Device? dev = Database.Memory.Devices.GetDevice(deviceId, token.user_id);
                if (dev == null)
                {
                    return Results.NotFound(new ErrorResponse
                    {
                        errcode = "M_NOT_FOUND",
                        error = "Device not found"
                    });
                }

                var options = new JsonSerializerOptions(JsonSerializerOptions.Default)
                {
                    AllowTrailingCommas = true,
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                };
                DeviceNameChangeData? data = null;
                try
                {
                    data = await context.Request.ReadFromJsonAsync<DeviceNameChangeData>(options);
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

                if (!string.IsNullOrWhiteSpace(data.DisplayName))
                {
                    dev.display_name = data.DisplayName;
                }

                // Update done.
                return Results.Ok(new { });
            });

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#delete-matrix-client-r0-devices-deviceid,
            // i. e. the endpoint to remove a specific device and the
            // corresponding access token.
            app.MapDelete("/_matrix/client/r0/devices/{deviceId}", (string deviceId, HttpContext context) =>
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

                // TODO: Device deletion should use the user-interactive
                // authentication API and require the user to re-submit the
                // current password for the account.
                //
                // A possible response could be HTTP 401 and then:
                // {
                //  "session": "random server-generated session ID here",
                //  "flows": [{
                //    "stages": ["m.login.password"]
                //  }],
                //  "params": {}
                // }
                //
                // Then the client has to resubmit the request, but with auth
                // data containing the current password, for example:
                // {
                //   "auth": {
                //     "type": "m.login.password",
                //     "session": "same session ID that the server gave",
                //     "password": "the actual secret password"
                //   }
                // }
                //
                // Then the actual deletion can be performed.

                Data.Device? dev = Database.Memory.Devices.GetDevice(deviceId, token.user_id);
                if (dev == null)
                {
                    // As per specification, status code 200 is also send for a
                    // device that cannot be found, because the assumption is
                    // that it was deleted earlier. Assumption is not that it
                    // never existed in the first place.
                    return Results.Ok(new { });
                }

                // Device was found. Now find associated access token and revoke it.
                var token_to_revoke = Database.Memory.AccessTokens.FindByUserAndDevice(dev.user_id, deviceId);
                if (token_to_revoke != null)
                {
                    Database.Memory.AccessTokens.Revoke(token_to_revoke.token);
                }
                // Delete device.
                _ = Database.Memory.Devices.Remove(dev.device_id, token.user_id);
                return Results.Ok(new { });
            });
        }
    }
}
