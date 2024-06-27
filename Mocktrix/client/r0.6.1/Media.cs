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
using Mocktrix.Protocol.Types.Media;

namespace Mocktrix.client.r0_6_1
{
    /// <summary>
    /// Contains implementation of media-related endpoints for version r0.6.1.
    /// </summary>
    public static class Media
    {
        /// <summary>
        /// Adds media-related endpoints to the web application.
        /// </summary>
        /// <param name="app">the app to which the endpoints shall be added</param>
        public static void AddEndpoints(WebApplication app)
        {
            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-media-r0-upload,
            // i.e. the endpoint to upload a new file.
            app.MapPost("/_matrix/media/r0/upload", async (HttpContext context) =>
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

                var limit = Convert.ToInt64(Configuration.ConfigurationManager.Current.UploadLimit);
                byte[] buffer = new byte[limit + 1];
                long bytesRead = 0;
                int currentlyRead = 1;
                while ((bytesRead < limit + 1) && (currentlyRead > 0))
                {
                    currentlyRead = await context.Request.Body.ReadAsync(buffer, Convert.ToInt32(bytesRead), Convert.ToInt32(limit + 1 - bytesRead));
                    bytesRead += currentlyRead;
                }

                if (bytesRead > limit)
                {
                    var error = new ErrorResponse
                    {
                        errcode = "M_TOO_LARGE",
                        error = "Uploaded file exceeds the allowed size limit."
                    };
                    return Results.Json(error, statusCode: StatusCodes.Status413PayloadTooLarge);
                }
                  
                string? contentType = context.Request.ContentType;
                string? fileName = context.Request.Query["filename"].FirstOrDefault("");

                string id = ContentRepository.Memory.Media.Create(new ReadOnlySpan<byte>(buffer, 0, Convert.ToInt32(bytesRead)), contentType, fileName);

                var server_address = new Uri(app.Urls.FirstOrDefault("http://localhost/"));
                var data = new 
                {
                    content_uri = "mxc://" + server_address.Host + "/" + id
                };

                return Results.Ok(data);
            });

            // Implements https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-media-r0-config,
            // i.e. the endpoint to query media-related configuration settings.
            app.MapGet("/_matrix/media/r0/config", (HttpContext context) =>
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

                var config = new MediaConfiguration()
                {
                    UploadSize = Configuration.ConfigurationManager.Current.UploadLimit
                };
                // Return configuration values.
                return Results.Ok(config);
            });
        }
    }
}
