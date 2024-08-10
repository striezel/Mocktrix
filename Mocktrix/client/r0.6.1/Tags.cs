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

using Mocktrix.Events;
using Mocktrix.Protocol.Types;

namespace Mocktrix.client.r0_6_1
{
    /// <summary>
    /// Contains implementation of tag-related endpoints for version r0.6.1.
    /// </summary>
    public static class Tags
    {
        /// <summary>
        /// Implements https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-user-userid-rooms-roomid-tags,
        /// i.e. the endpoint to get available tags for a room.
        /// </summary>
        /// <param name="userId">The id of the user to get tags for. The access
        /// token must be authorized to make requests for this user ID.</param>
        /// <param name="roomId">The ID of the room to get tags for.</param>
        private static IResult GetRoomTags(string userId, string roomId, HttpContext context)
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
            if (token.user_id != userId)
            {
                var error = new ErrorResponse
                {
                    errcode = "M_FORBIDDEN",
                    error = "You cannot query tags of other users."
                };
                return Results.Json(error, statusCode: StatusCodes.Status403Forbidden);
            }

            var data = Database.Memory.Tags.GetAllRoomTags(userId, roomId);
            var content = new TagEventContent()
            {
                Tags = []
            };
            foreach (var tag in data)
            {
                content.Tags[tag.Name] = new() { Order = tag.Order };
            }

            return Results.Ok(content);
        }


        /// <summary>
        /// Implements https://spec.matrix.org/historical/client_server/r0.6.1.html#delete-matrix-client-r0-user-userid-rooms-roomid-tags-tag,
        /// i.e. the endpoint to delete a tag.
        /// </summary>
        /// <param name="userId">The id of the user to remove a tag for.</param>
        /// <param name="roomId">The ID of the room to remove a tag from.</param>
        /// <param name="tag">the tag to remove</param>
        /// <param name="context">request context</param>
        /// <returns>Returns a result for an HTTP(S) endpoint.</returns>
        private static IResult DeleteTag(string userId, string roomId, string tag, HttpContext context)
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
            if (token.user_id != userId)
            {
                var error = new ErrorResponse
                {
                    errcode = "M_FORBIDDEN",
                    error = "You cannot delete tags of other users."
                };
                return Results.Json(error, statusCode: StatusCodes.Status403Forbidden);
            }

            _ = Database.Memory.Tags.DeleteTag(token.user_id, roomId, tag);
            return Results.Ok(new { });
        }

        /// <summary>
        /// Adds tag-related endpoints to the web application.
        /// </summary>
        /// <param name="app">the app to which the endpoints shall be added</param>
        public static void AddEndpoints(WebApplication app)
        {
            // Add https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-user-userid-rooms-roomid-tags,
            // i.e. the endpoint to get available tags for a room.
            app.MapGet("/_matrix/client/r0/user/{userId}/rooms/{roomId}/tags", GetRoomTags);

            // Add https://spec.matrix.org/historical/client_server/r0.6.1.html#delete-matrix-client-r0-user-userid-rooms-roomid-tags-tag,
            // i.e. the endpoint to delete a tag.
            app.MapDelete("/_matrix/client/r0/user/{userId}/rooms/{roomId}/tags/{tag}", DeleteTag);
        }
    }
}
