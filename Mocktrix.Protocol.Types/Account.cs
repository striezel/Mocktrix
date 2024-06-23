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

namespace Mocktrix.Protocol.Types.Account
{
    /// <summary>
    /// Holds information about user-interactive authentication.
    /// </summary>
    public class AuthenticationData
    {
        /// <summary>
        /// The login type that the client attempts to complete.
        /// </summary>
        [JsonPropertyName("type")]
        public required string Type { get; set; } = string.Empty;


        /// <summary>
        /// The session key which was provided by the homeserver.
        /// </summary>
        [JsonPropertyName("session")]
        public string? Session { get; set; } = string.Empty;


        /// <summary>
        /// The current password of the account.
        /// </summary>
        [JsonPropertyName("password")]
        public string? Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Data sent by a client via POST request to change the password of an
    /// account.
    /// </summary>
    public class PasswordChangeData
    {
        /// <summary>
        /// The new password of the account.
        /// </summary>
        [JsonPropertyName("new_password")]
        public string NewPassword { get; set; } = string.Empty;


        /// <summary>
        /// Whether the other access tokens and associated devices of the user
        /// should be logged out. Defaults to true, if not given.
        /// </summary>
        [JsonPropertyName("logout_devices")]
        public bool? LogoutDevices { get; set; } = null;


        /// <summary>
        /// Information for the user-interactive authentication.
        /// </summary>
        [JsonPropertyName("auth")]
        public AuthenticationData? Auth { get; set; } = null;
    }
}
