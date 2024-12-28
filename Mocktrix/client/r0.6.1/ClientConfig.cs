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
        /// Adds endpoints for client configuration data to the web application.
        /// </summary>
        /// <param name="app">the app to which the endpoints shall be added</param>
        public static void AddEndpoints(WebApplication app)
        {
            // Add https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-user-userid-account-data-type,
            // i. e. the endpoint to get account data for the client.
            app.MapGet("/_matrix/client/r0/user/{userId}/account_data/{type}", GetAccountDataByType);
        }
    }
}
