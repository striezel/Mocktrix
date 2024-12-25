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

using Mocktrix.Data;
using System.Text.Json.Nodes;

namespace Mocktrix.Database.Memory
{
    /// <summary>
    /// In-memory implementation of user-specific configuration setting database.
    /// </summary>
    public static class ConfigData
    {
        /// <summary>
        /// in-memory configuration data list
        /// </summary>
        private static readonly List<ConfigDatum> data = [];


        /// <summary>
        /// Creates and adds a new configuration datum.
        /// </summary>
        /// <param name="user_id">id of the user that owns the configuration data, e.g. "@alice:matrix.example.org"</param>
        /// <param name="type">event type of the account data to set.
        /// Custom types should be namespaced to avoid clashes.</param>
        /// <param name="_data">the account data to set</param>
        /// <returns>Returns the created configuration datum.</returns>
        public static ConfigDatum Create(string user_id, string type, JsonNode _data)
        {
            ConfigDatum entry = new(user_id, type, _data);
            data.Add(entry);
            return entry;
        }


        /// <summary>
        /// Gets an existing configuration datum.
        /// </summary>
        /// <param name="user_id">id of the user that owns the configuration data, e.g. "@alice:matrix.example.org"</param>
        /// <param name="type">event type of the account data to set.
        /// Custom types should be namespaced to avoid clashes.</param>
        /// <returns>Returns a configuration datum with the matching properties, if it exists.
        /// Returns null, if no match was found.</returns>
        public static ConfigDatum? GetDatum(string user_id, string type)
        {
            return data.Find(d => d.UserId == user_id && d.Type == type);
        }


        /// <summary>
        /// Gets all account data information for a given user.
        /// </summary>
        /// <param name="user_id">id of the user that owns the configuration data, e.g. "@alice:matrix.example.org"</param>
        /// <returns>Returns a list of configuration data items for a user, if they exist.
        /// Returns an empty list otherwise.</returns>
        public static List<ConfigDatum> GetAllConfigData(string user_id)
        {
            return data.FindAll(entry => entry.UserId == user_id);
        }
    }
}
