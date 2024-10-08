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

using System.Security.Cryptography;

namespace Mocktrix.Events
{
    /// <summary>
    /// Contains utility functions for event ids.
    /// </summary>
    public static class Id
    {
        /// <summary>
        /// alphabet containing all possible characters for the "localpart" of event ids in rooms of version 1 and 2
        /// </summary>
        private static readonly string id_alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";


        /// <summary>
        /// Generates a random event id for a server with the given URI.
        /// </summary>
        /// <param name="server_uri">the server's URI</param>
        /// <returns>Returns an event id for the server.</returns>
        public static string Generate(Uri server_uri)
        {
            ArgumentNullException.ThrowIfNull(server_uri, nameof(server_uri));

            return '$' + RandomNumberGenerator.GetString(id_alphabet, 20) + ':' + server_uri.Host;
        }
    }
}
