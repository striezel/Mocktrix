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

using System.Collections.ObjectModel;

namespace Mocktrix.RoomVersions
{
    /// <summary>
    /// Provides information about supported room versions.
    /// </summary>
    public static class Support
    {
        /// <summary>
        /// Read-only dictionary containing the supported versions and their stability.
        /// </summary>
        private static readonly ReadOnlyDictionary<string, string> versions = new Dictionary<string, string>(3)
        {
            { "1", "stable" },
            { "2", "unstable" },
            { "3", "unstable" }
        }.AsReadOnly();


        /// <summary>
        /// Checks whether a given room version is supported.
        /// </summary>
        /// <param name="version">the version to check, e.g. "1" for room version 1</param>
        /// <returns>Returns true, if the room version is supported.
        /// Returns false otherwise.</returns>
        public static bool IsSupportedVersion(string version)
        {
            return version switch
            {
                "1" => true,
                "2" => true,
                "3" => true,
                _ => false,
            };
        }


        /// <summary>
        /// Gets a dictionary of all supported room version (=key) with their
        /// corresponding stability (=value).
        /// </summary>
        /// <returns></returns>
        public static ReadOnlyDictionary<string, string> GetSupportedVersions()
        {
            return versions;
        }
    }
}
