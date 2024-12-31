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
using Mocktrix.Protocol.Types.Sync;

namespace Mocktrix.client.r0_6_1
{
    /// <summary>
    /// Contains implementation of event synchronization endpoints for version r0.6.1.
    /// </summary>
    public static class Syncing
    {
        /// <summary>
        /// Adds user-specific account data to the sync response, if any such
        /// data is set.
        /// </summary>
        /// <param name="user_id">id of the Matrix user, e. g. "@alice:matrix.example.org"</param>
        /// <param name="response">the response object to fill with data</param>
        private static void PrepareAccountData(string user_id, SyncResponse response)
        {
            var config_data = Database.Memory.ConfigData.GetAllConfigData(user_id);
            if (config_data.Count > 0)
            {
                response.AccountData = new AccountData()
                {
                    Events = new List<ConfigDataEvent>(config_data.Count)
                };
                foreach (var entry in config_data)
                {
                    var ev = new ConfigDataEvent()
                    {
                        Content = entry.Data,
                        Type = entry.Type
                    };
                    response.AccountData.Events.Add(ev);
                }
            }
        }


        /// <summary>
        /// Mock https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-sync,
        /// i.e. the endpoint to sync events.
        /// </summary>
        private static IResult Sync(HttpContext context)
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

            // Return empty event list.
            var response = new SyncResponse()
            {
                AccountData = null,

                // Pagination by using next_batch values in the since parameter
                // to subsequent requests is not implemented yet.
                NextBatch = "not_implemented"
            };

            // Prepare "account_data", if any such data is set.
            PrepareAccountData(token.user_id, response);

            return Results.Ok(response);
        }

        /// <summary>
        /// Adds event synchronization endpoints to the web application.
        /// </summary>
        /// <param name="app">the app to which the endpoint shall be added</param>
        public static void AddEndpoints(WebApplication app)
        {
            // Mock https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-sync,
            // i.e. the endpoint to sync events.
            app.MapGet("/_matrix/client/r0/sync", Sync);
        }
    }
}
