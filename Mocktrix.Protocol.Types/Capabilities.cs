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

namespace Mocktrix.Protocol.Types.Capabilities
{
    /// <summary>
    /// Holds information about the capability to change a user's password.
    /// </summary>
    public class ChangePasswordCapability
    {
        /// <summary>
        /// Indicates whether users can change their passwords.
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = false;
    }

    /// <summary>
    /// Holds information about available room versions and the default room version.
    /// </summary>
    public class RoomVersionsCapability
    {
        /// <summary>
        /// The default room version used for new rooms.
        /// </summary>
        [JsonPropertyName("default")]
        public string DefaultVersion { get; set; } = "1";


        /// <summary>
        /// Available room versions (=keys) and their stability levels (=values).
        /// Stability level must be one of ["stable", "unstable"].
        /// </summary>
        [JsonPropertyName("available")]
        public Dictionary<string, string> Available { get; set; } = [];
    }

    /// <summary>
    /// Holds information about a server's available capabilities.
    /// </summary>
    public class ServerCapabilities
    {
        public ServerCapabilities()
        {
            ChangePassword = null;
            RoomVersions = null;
        }

        [JsonPropertyName("m.change_password")]
        public ChangePasswordCapability? ChangePassword { get; set; }

        [JsonPropertyName("m.room_versions")]
        public RoomVersionsCapability? RoomVersions { get; set; }
    }
}
