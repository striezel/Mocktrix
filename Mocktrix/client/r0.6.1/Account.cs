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
            var DoNotAllowThreePID = (HttpContext context) =>
            {
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

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-account-3pid,
            // i. e. gets list of associated third-party ids for the user id
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

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-account-3pid-email-requesttoken
            // by not allowing it.
            app.MapPost("/_matrix/client/r0/account/3pid/email/requestToken", DoNotAllowThreePID);

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-account-3pid-msisdn-requesttoken
            // by not allowing it.
            app.MapPost("/_matrix/client/r0/account/3pid/msisdn/requestToken", DoNotAllowThreePID);

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-account-whoami,
            // i. e. gets the user id associated with an access token.
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
