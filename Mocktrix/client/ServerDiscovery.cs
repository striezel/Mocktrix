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

namespace Mocktrix.client
{
    /// <summary>
    /// Implements the server discovery endpoint.
    /// </summary>
    public static class ServerDiscovery
    {
        /// <summary>
        /// Adds server discovery endpoint to the web application.
        /// </summary>
        /// <param name="app">the app to which the endpoint shall be added</param>
        public static void AddEndpoints(WebApplication app)
        {
            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#get-well-known-matrix-client.
            app.MapGet("/.well-known/matrix/client", (HttpContext httpContext) =>
            {
                var result = new DiscoveryInformation()
                {
                    Homeserver = new HomeserverInformation()
                    {
                        BaseUrl = app.Urls.First()
                    }
                };
                return Results.Json(result);
            });
        }
    }
}
