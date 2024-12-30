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
using System.Text.Json.Nodes;

namespace Mocktrix.client.r0_6_1
{
    /// <summary>
    /// Contains implementation for client config endpoints of protocol version r0.6.1.
    /// </summary>
    public static class ClientConfig
    {
        /// <summary>
        /// Implements https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-user-userid-account-data-type,
        /// i. e. the endpoint to get account data for the client.
        /// </summary>
        /// <param name="userId">id of the user to get account data for</param>
        /// <param name="type">event type of the account data to get</param>
        private static IResult GetAccountDataByType(HttpContext context, string userId, string type)
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
            if (userId != token.user_id)
            {
                var error = new ErrorResponse
                {
                    errcode = "M_FORBIDDEN",
                    error = "You cannot get account data of another user."
                };
                return Results.Json(error, statusCode: StatusCodes.Status403Forbidden);
            }

            var entry = Database.Memory.ConfigData.GetDatum(userId, type);
            if (entry == null)
            {
                var error = new ErrorResponse
                {
                    errcode = "M_NOT_FOUND",
                    error = "Account data of the requested type was not found."
                };
                return Results.NotFound(error);
            }

            return Results.Json(entry.Data);
        }


        /// <summary>
        /// Checks whether a data type is managed by the server.
        /// </summary>
        /// <param name="type">the type to check</param>
        /// <returns>Returns true, if the type is managed by the server.
        /// Returns false otherwise.</returns>
        private static bool IsServerManagedType(string type)
        {
            // In version r.0.6.1 of the client-server specification of the
            // Matrix protocol, the only type that is managed by the server is
            // "m.fully_read".
            return type == "m.fully_read";
        }


        /// <summary>
        /// Implements https://spec.matrix.org/historical/client_server/r0.6.1.html#put-matrix-client-r0-user-userid-account-data-type,
        /// i. e. the endpoint to set account data for the client.
        /// </summary>
        /// <param name="userId">id of the user to get account data for</param>
        /// <param name="type">event type of the account data to get</param>
        private static async Task<IResult> SetAccountDataByType(HttpContext context, string userId, string type)
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
            if (userId != token.user_id)
            {
                var error = new ErrorResponse
                {
                    errcode = "M_FORBIDDEN",
                    error = "You cannot set account data of another user."
                };
                return Results.Json(error, statusCode: StatusCodes.Status403Forbidden);
            }

            // Server-managed types must be rejected, as per specification.
            if (IsServerManagedType(type))
            {
                var error = new ErrorResponse
                {
                    errcode = "M_UNKNOWN",
                    error = "The m.fully_read type cannot be set via this API."
                };
                return Results.BadRequest(error);
            }

            JsonNode? node;
            try
            {
                node = JsonNode.Parse(await Utilities.GetRequestBodyAsString(context));
            }
            catch (Exception)
            {
                var error = new ErrorResponse
                {
                    errcode = "M_NOT_JSON",
                    error = "The content is not valid JSON."
                };
                return Results.BadRequest(error);
            }
            if (node == null)
            {
                var error = new ErrorResponse
                {
                    errcode = "M_NOT_JSON",
                    error = "The content is not valid JSON."
                };
                return Results.BadRequest(error);
            }

            var entry = Database.Memory.ConfigData.GetDatum(userId, type);
            if (entry != null)
            {
                entry.Data = node;
            }
            else
            {
                _ = Database.Memory.ConfigData.Create(userId, type, node);
            }

            return Results.Ok(new { });
        }


        /// <summary>
        /// Implements https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-user-userid-rooms-roomid-account-data-type,
        /// i. e. the endpoint to get account data for the client for a specific room.
        /// </summary>
        /// <param name="userId">id of the user to get account data for</param>
        /// /// <param name="roomId">id of the room to which the account data belongs</param>
        /// <param name="type">event type of the account data to get</param>
        private static IResult GetRoomAccountDataByType(HttpContext context, string userId, string roomId, string type)
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
            if (userId != token.user_id)
            {
                var error = new ErrorResponse
                {
                    errcode = "M_FORBIDDEN",
                    error = "You cannot get account data of another user."
                };
                return Results.Json(error, statusCode: StatusCodes.Status403Forbidden);
            }

            var entry = Database.Memory.RoomConfigData.GetDatum(userId, roomId, type);
            if (entry == null)
            {
                var error = new ErrorResponse
                {
                    errcode = "M_NOT_FOUND",
                    error = "Account data of the requested type for the requested room was not found."
                };
                return Results.NotFound(error);
            }

            return Results.Json(entry.Data);
        }


        /// <summary>
        /// Adds endpoints for client configuration data to the web application.
        /// </summary>
        /// <param name="app">the app to which the endpoints shall be added</param>
        public static void AddEndpoints(WebApplication app)
        {
            // Add https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-user-userid-account-data-type,
            // i. e. the endpoint to get account data for the client.
            app.MapGet("/_matrix/client/r0/user/{userId}/account_data/{type}", GetAccountDataByType);

            // Add https://spec.matrix.org/historical/client_server/r0.6.1.html#put-matrix-client-r0-user-userid-account-data-type,
            // i. e. the endpoint to get account data for the client.
            app.MapPut("/_matrix/client/r0/user/{userId}/account_data/{type}", SetAccountDataByType);

            // Add https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-user-userid-rooms-roomid-account-data-type,
            // i. e. the endpoint to get account data for the client.
            app.MapGet("/_matrix/client/r0/user/{userId}/rooms/{roomId}/account_data/{type}", GetRoomAccountDataByType);
        }
    }
}
