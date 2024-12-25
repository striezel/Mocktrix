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

using System.Text.Json.Nodes;

namespace Mocktrix.Data
{
    /// <summary>
    /// Holds information about user-specific, client-set configuration data.
    /// (It's named "account_data" in the Matrix specification.)
    /// </summary>
    /// <param name="user_id">id of the user that owns the configuration data, e.g. "@alice:matrix.example.org"</param>
    /// <param name="type">event type of the account data to set.
    /// Custom types should be namespaced to avoid clashes.</param>
    /// <param name="data">the account data to set</param>
    public class ConfigDatum(string user_id, string type, JsonNode data)
    {
        /// <summary>
        /// Id of the user that owns the configuration data, e.g. "@alice:matrix.example.org".
        /// </summary>
        public string UserId { get; set; } = user_id;

        /// <summary>
        /// The event type of the configuration data, e.g. "org.example.custom.config".
        /// </summary>
        public string Type { get; set; } = type;

        /// <summary>
        /// The configuration data for the given user and type.
        /// </summary>
        public JsonNode Data { get; set; } = data;
    }
}
