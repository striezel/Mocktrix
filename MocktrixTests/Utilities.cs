/*
    This file is part of test suite for Mocktrix.
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

using System.Text.Json;

namespace MocktrixTests
{
    /// <summary>
    /// Contains utilities to ease testing.
    /// </summary>
    internal class Utilities
    {
        /// <summary>
        /// Gets the base address for the server to test against.
        /// </summary>
        public static Uri BaseAddress
        {
            get
            {
                return new Uri("http://localhost:5289");
            }
        }


        /// <summary>
        /// Returns the deserialized JSON content from a HTTP response.
        /// </summary>
        /// <typeparam name="T">type of the returned object</typeparam>
        /// <param name="response">the actual (successful) HTTP response</param>
        /// <param name="_">dummy parameter to allow type deduction</param>
        /// <returns>Returns the deserialized response.</returns>
        public static T GetContent<T>(HttpResponseMessage response, T _)
        {
            return JsonSerializer.Deserialize<T?>(response.Content.ReadAsStream());
        }
    }
}
