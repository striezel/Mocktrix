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
    /// Contains implementation for room-related endpoints of protocol version r0.6.1
    /// </summary>
    public static class Rooms
    {
        // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-joined-rooms,
        // i.e. the endpoint to get information about joined rooms of a
        // user.
        private static IResult GetJoinedRooms(HttpContext context)
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

            // Token was found, so get joined rooms.
            var joined_rooms = Database.Memory.RoomMemberships
                              .GetAllMembershipsOfUser(token.user_id)
                              .FindAll(e => e.Membership == Enums.Membership.Join);
            var result = new
            {
                joined_rooms = new List<string>(joined_rooms.Count)
            };
            foreach (var element in joined_rooms)
            {
                result.joined_rooms.Add(element.RoomId);
            }
            return Results.Ok(result);
        }


        /// <summary>
        /// Adds room-related endpoints to the web application.
        /// </summary>
        /// <param name="app">the app to which the endpoint shall be added</param>
        public static void AddEndpoints(WebApplication app)
        {
            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-joined-rooms,
            // i.e. the endpoint to get information about joined rooms of a
            // user.
            app.MapGet("/_matrix/client/r0/joined_rooms", GetJoinedRooms);
        }
    }
}
