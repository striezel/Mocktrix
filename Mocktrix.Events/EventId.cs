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

using System.Security.Cryptography;

namespace Mocktrix.Events
{
    /// <summary>
    /// Contains utility functions for event ids.
    /// </summary>
    public static class EventId
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
        /// <remarks>These ids are only valid for room versions 1 and 2.
        /// Starting with room version 3, Matrix rooms use another id format.</remarks>
        public static string Generate(Uri server_uri)
        {
            ArgumentNullException.ThrowIfNull(server_uri, nameof(server_uri));

            // According to the Matrix protocol specification, event ids must
            // not exceed the length of 255 characters, including sigil
            // character, localpart and domain.
            var host = server_uri.Host;
            // Usually, the generated localpart has 20 characters here. However,
            // if the host part is too long, it is shortened down to fit into
            // the 255 character limit. But then again, the size of the
            // localpart is increased to at least 10 characters. Otherwise we
            // might have localparts with just one or two letters, and in that
            // case the probability of generating to identical localparts is too
            // high.
            var random_char_count = Math.Max(Math.Min(255 - 2 - host.Length, 20), 10);
            return '$' + RandomNumberGenerator.GetString(id_alphabet, random_char_count) + ':' + server_uri.Host;
        }
    }
}
