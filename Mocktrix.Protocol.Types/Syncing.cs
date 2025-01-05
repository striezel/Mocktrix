/*
    This file is part of Mocktrix.
    Copyright (C) 2024, 2025  Dirk Stolle

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
using System.Text.Json.Serialization;

namespace Mocktrix.Protocol.Types.Sync
{
    /// <summary>
    /// Contains information provided by the server as response to a /sync request.
    /// </summary>
    public class SyncResponse
    {
        /// <summary>
        /// Contains user-specific configuration data (called "account data" by
        /// the Matrix specification).
        /// </summary>
        [JsonPropertyName("account_data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AccountData? AccountData { get; set; } = null;


        /// <summary>
        /// The batch token to supply in the since parameter of the next /sync
        /// request.
        /// </summary>
        [JsonPropertyName("next_batch")]
        public required string NextBatch { get; set; }


        /// <summary>
        /// Room-related sync data.
        /// </summary>
        [JsonPropertyName("rooms")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Rooms? Rooms { get; set; } = null;
    }


    /// <summary>
    /// Contains events for user-specific configuration data
    /// (called "account data" by the Matrix specification).
    /// </summary>
    public class AccountData
    {
        /// <summary>
        /// List containing the account data events.
        /// </summary>
        [JsonPropertyName("events")]
        public List<AccountDataEvent>? Events { get; set; } = null;
    }


    /// <summary>
    /// Contains room events and other room-related information for a sync
    /// request.
    /// </summary>
    public class Rooms
    {
        /// <summary>
        /// Rooms that the user has joined.
        /// </summary>
        [JsonPropertyName("join")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, JoinedRoom>? Joined { get; set; } = null;


        /// <summary>
        /// Rooms that the user has left.
        /// </summary>
        [JsonPropertyName("leave")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, LeftRoom>? Left { get; set; } = null;
    }


    /// <summary>
    /// Sync information about a room that the user has joined.
    /// </summary>
    public class JoinedRoom
    {
        /// <summary>
        /// Contains user-specific room configuration data (called "account
        /// data" by the Matrix specification).
        /// </summary>
        [JsonPropertyName("account_data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AccountData? AccountData { get; set; } = null;
    }


    /// <summary>
    /// Sync information about a room that the user has left.
    /// </summary>
    public class LeftRoom
    {
        /// <summary>
        /// Contains user-specific room configuration data (called "account
        /// data" by the Matrix specification).
        /// </summary>
        [JsonPropertyName("account_data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AccountData? AccountData { get; set; } = null;
    }
}
