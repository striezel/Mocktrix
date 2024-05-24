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

namespace Mocktrix.Data.Tests
{
    public class AccessTokenTests
    {
        [Fact]
        public void Constructor()
        {
            AccessToken token = new("@alice:matrix.example.com", "MyDeviceId", "SomeRandomString");

            Assert.Equal("@alice:matrix.example.com", token.user_id);
            Assert.Equal("MyDeviceId", token.device_id);
            Assert.Equal("SomeRandomString", token.token);
        }


        [Fact]
        public void GenerateRandomToken()
        {
            var token_one = AccessToken.GenerateRandomToken();
            Assert.NotNull(token_one);
            Assert.Matches("^[A-Za-z0-9]+$", token_one);

            var token_two = AccessToken.GenerateRandomToken();
            Assert.NotNull(token_two);
            Assert.Matches("^[A-Za-z0-9]+$", token_two);

            // Ideally, both tokens are unequal, but there is a slim chance to
            // get the same token twice, because it's random and that could give
            // us the same sequence twice, although it is very unlikely.
            Assert.NotEqual(token_one, token_two);
        }
    }
}