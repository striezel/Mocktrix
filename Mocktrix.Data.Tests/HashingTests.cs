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

using Mocktrix.utilities;

namespace Mocktrix.Data.Tests
{
    public class HashingTests
    {
        [Fact]
        public void CreateHashedSaltedPassword_Basic()
        {
            var hash = Hashing.CreateHashedSaltedPassword("secret_password", out byte[] salt);

            Assert.False(string.IsNullOrEmpty(hash));
            Assert.True(hash.Length > 40);
            Assert.NotNull(salt);
            Assert.True(salt.Length > 40);
        }

        [Fact]
        public void CreateHashedSaltedPassword_SamePasswordDifferentSaltAndHash()
        {
            // Creating a hash and a salt for the same password twice should
            // yield different hashes and different salts.
            var hash_one = Hashing.CreateHashedSaltedPassword("secret_password", out byte[] salt_one);
            Assert.False(string.IsNullOrEmpty(hash_one));
            Assert.NotNull(salt_one);

            var hash_two = Hashing.CreateHashedSaltedPassword("secret_password", out byte[] salt_two);
            Assert.False(string.IsNullOrEmpty(hash_two));
            Assert.NotNull(salt_two);

            // Hashes and salts must not be equal.
            Assert.NotEqual(hash_one, hash_two);
            Assert.NotEqual(salt_one, salt_two);
        }


        [Fact]
        public void HashPassword_Basic()
        {
            byte[] salt = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"u8.ToArray();
            var hash = Hashing.HashPassword("secret_password", salt);

            Assert.False(string.IsNullOrEmpty(hash));
            Assert.True(hash.Length > 40);
            Assert.Equal(128, hash.Length);
        }

        [Fact]
        public void HashPassword_SamePasswordDifferentSaltGetsDifferentHash()
        {
            const string password = "some_secret_password";
            byte[] salt_one = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"u8.ToArray();
            var hash_one = Hashing.HashPassword(password, salt_one);

            byte[] salt_two = "01234567890foobarbazquuxABC"u8.ToArray();
            var hash_two = Hashing.HashPassword(password, salt_two);

            // Hashes have the same length, ...
            Assert.Equal(hash_one.Length, hash_two.Length);
            // ... but they are not equal.
            Assert.NotEqual(hash_one, hash_two);
        }


        [Fact]
        public void HashPassword_GetsSameResultWhenUsedWithValuesFromCreateHashedSaltedPassword()
        {
            const string password = "Such secret, much wow!";
            var hash_creation = Hashing.CreateHashedSaltedPassword(password, out byte[] salt);

            Assert.True(hash_creation.Length > 40);
            Assert.True(salt.Length > 40);

            var hash_hashed = Hashing.HashPassword(password, salt);

            // Hashes must be equal.
            Assert.Equal(hash_creation, hash_hashed);
        }


        [Fact]
        public void HashPassword_DifferentHashWithSameSaltButDifferentPassword()
        {
            const string password_one = "Such secret, much wow!";
            const string password_two = "Much wow, such secret!";
            byte[] salt = "FooBarBazQuux"u8.ToArray();

            var hash_one = Hashing.HashPassword(password_one, salt);
            var hash_two = Hashing.HashPassword(password_two, salt);

            // Hashes must NOT be equal.
            Assert.NotEqual(hash_one, hash_two);
        }
    }
}
