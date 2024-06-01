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

namespace Mocktrix.client.r0_6_1
{
    /// <summary>
    /// Contains implementation of account registration endpoints for version r0.6.1.
    /// </summary>
    public static class Registration
    {
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
        }
    }
}
