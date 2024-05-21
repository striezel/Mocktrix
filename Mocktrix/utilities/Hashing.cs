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

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Mocktrix.utilities
{
    /// <summary>
    /// Contains functions for password hashing.
    /// </summary>
    public class Hashing
    {
        private const int iterations = 334455;
        private const int hash_bytes = 64;


        /// <summary>
        /// Creates a password hash and a random salt.
        /// </summary>
        /// <param name="password">the password to hash</param>
        /// <param name="salt">returns the generated salt</param>
        /// <returns>Returns the password hash as hexadecimal string.</returns>
        public static string CreateHashedSaltedPassword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(65);
            var hash = KeyDerivation.Pbkdf2(password, salt,
                KeyDerivationPrf.HMACSHA512, iterations, hash_bytes);
            return Convert.ToHexString(hash);
        }


        /// <summary>
        /// Hashes a password, using a given salt.
        /// </summary>
        /// <param name="password">the password to hash</param>
        /// <param name="salt">the salt to use</param>
        /// <returns>Returns the password hash as hexadecimal string.</returns>
        public static string HashPassword(string password, byte[] salt)
        {
            var hash = KeyDerivation.Pbkdf2(password, salt,
                KeyDerivationPrf.HMACSHA512, iterations, hash_bytes);
            return Convert.ToHexString(hash);
        }
    }
}
