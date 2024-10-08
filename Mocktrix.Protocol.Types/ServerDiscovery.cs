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

namespace Mocktrix.Protocol.Types
{
    /// <summary>
    /// Holds information about the Matrix homeserver.
    /// </summary>
    public struct HomeserverInformation
    {
        /// <summary>
        /// The base URL for the homeserver for client-server connections, e.g.
        /// "https://matrix.example.org".
        /// </summary>
        [JsonPropertyName("base_url")]
        public string BaseUrl { get; set; }
    }

    /// <summary>
    /// Contains server discovery information (currently only homeserver and no
    /// identity server information).
    /// </summary>
    public struct DiscoveryInformation
    {
        /// <summary>
        /// Information about the homeserver.
        /// </summary>
        [JsonPropertyName("m.homeserver")]
        public HomeserverInformation Homeserver { get; set; }
    }
}
