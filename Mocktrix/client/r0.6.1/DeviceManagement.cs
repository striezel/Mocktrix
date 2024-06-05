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

using Mocktrix.Protocol.Types.DeviceManagement;

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
            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-devices.
            app.MapGet("/_matrix/client/r0/devices", (HttpContext context) =>
            {
                var access_token = Utilities.GetAccessToken(context);
                if (string.IsNullOrWhiteSpace(access_token))
                {
                    var error = new
                    {
                        errcode = "M_MISSING_TOKEN",
                        error = "Missing access token."
                    };
                    return Results.Json(error, statusCode: StatusCodes.Status401Unauthorized);
                }
                var token = Database.Memory.AccessTokens.Find(access_token);
                if (token == null)
                {
                    var error = new
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


            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-devices-deviceid.
            app.MapGet("/_matrix/client/r0/devices/{deviceId}", (string deviceId, HttpContext context) =>
            {
                var access_token = Utilities.GetAccessToken(context);
                if (string.IsNullOrWhiteSpace(access_token))
                {
                    var error = new
                    {
                        errcode = "M_MISSING_TOKEN",
                        error = "Missing access token."
                    };
                    return Results.Json(error, statusCode: StatusCodes.Status401Unauthorized);
                }
                var token = Database.Memory.AccessTokens.Find(access_token);
                if (token == null)
                {
                    var error = new
                    {
                        errcode = "M_UNKNOWN_TOKEN",
                        error = "Unrecognized access token."
                    };
                    return Results.Json(error, statusCode: StatusCodes.Status401Unauthorized);
                }

                Data.Device? dev = Database.Memory.Devices.GetDevice(deviceId, token.user_id);
                if (dev == null)
                {
                    return Results.NotFound(new
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
        }
    }
}
