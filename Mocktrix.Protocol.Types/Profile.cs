﻿/*
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

namespace Mocktrix.Protocol.Types.Profile
{
    /// <summary>
    /// Data sent by a client via PUT request to change the display name of a
    /// user account.
    /// </summary>
    public class DisplayNameChangeData
    {
        /// <summary>
        /// The new display name of the user.
        /// </summary>
        [JsonPropertyName("displayname")]
        public string? DisplayName { get; set; } = null;
    }


    /// <summary>
    /// Data sent by a client via PUT request to change the avatar URL of a
    /// user account.
    /// </summary>
    public class AvatarUrlChangeData
    {
        /// <summary>
        /// The new avatar URL of the user as Matrix Content (MXC) URI.
        /// </summary>
        [JsonPropertyName("avatar_url")]
        public string? AvatarUrl { get; set; } = null;
    }
}
