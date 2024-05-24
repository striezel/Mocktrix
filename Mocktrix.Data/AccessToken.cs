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

using System;
using System.Security.Cryptography;

namespace Mocktrix.Data
{
    /// <summary>
    /// Holds information about an access token.
    /// </summary>
    /// <param name="uid">id of the user, including homeserver domain, e. g. "@foo:matrix.example.org"</param>
    /// <param name="dev_id">device id</param>
    /// <param name="tok">the actual token</param>
    public class AccessToken(string uid, string dev_id, string tok)
    {
        /// <summary>
        /// id of the user, including homeserver domain, e. g. "@foo:matrix.example.org"
        /// </summary>
        public string user_id = uid;


        /// <summary>
        /// id of the device, may be a random string
        /// </summary>
        public string device_id = dev_id;


        /// <summary>
        /// the access token
        /// </summary>
        public string token = tok;


        /// <summary>
        /// Generates a random access token.
        /// </summary>
        /// <returns>Returns a new access token.</returns>
        public static string GenerateRandomToken()
        {
            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZbcdefghijklmnopqrstuvwxyz0123456789".AsSpan();
            return RandomNumberGenerator.GetString(alphabet, 32);
        }
    }
}
