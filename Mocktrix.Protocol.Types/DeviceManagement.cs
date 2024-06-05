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

namespace Mocktrix.Protocol.Types.DeviceManagement
{
    /// <summary>
    /// Contains information about a device.
    /// </summary>
    public class DeviceData
    {
        /// <summary>
        /// Device identifier.
        /// </summary>
        [JsonPropertyName("device_id")]
        public string DeviceId { get; set; } = string.Empty;

        /// <summary>
        /// Display name of the device, if any.
        /// </summary>
        [JsonPropertyName("display_name")]
        public string? DisplayName { get; set; }

        /// <summary>
        /// IP address where this device was last seen.
        /// </summary>
        [JsonPropertyName("last_seen_ip")]
        public string? LastSeenIP { get; set; }

        /// <summary>
        /// The timestamp in milliseconds since the Unix epoch when this
        /// devices was last active.
        /// </summary>
        [JsonPropertyName("last_seen_ts")]
        public long? LastSeenTimestamp { get; set; }
    }
}
