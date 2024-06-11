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

using System.Text.Json.Serialization;

namespace Mocktrix.Protocol.Types.Media
{
    /// <summary>
    /// Contains configuration information for the media repository.
    /// </summary>
    public class MediaConfiguration
    {
        /// <summary>
        /// The maximum size an upload can be in bytes.
        /// </summary>
        [JsonPropertyName("m.upload.size")]
        public required ulong? UploadSize { get; set; } = null;
    }
}
