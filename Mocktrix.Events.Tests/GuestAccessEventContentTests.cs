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

namespace Mocktrix.Events.Tests
{
    /// <summary>
    /// Contains tests for GuestAccessEventContent.
    /// </summary>
    public class GuestAccessEventContentTests
    {
        [Fact]
        public void Construction()
        {
            var content = new GuestAccessEventContent();
            Assert.Null(content.GuestAccess);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "guest_access": "can_join"
                       }
                       """;
            var content = JsonSerializer.Deserialize<GuestAccessEventContent>(json);

            Assert.NotNull(content);
            Assert.Equal("can_join", content.GuestAccess);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var content = new GuestAccessEventContent
            {
                GuestAccess = "can_join",
            };
            var expected_json = "{\"guest_access\":\"can_join\"}";
            var json = JsonSerializer.Serialize(content);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }

        [Theory]
        [InlineData("can_join", Enums.GuestAccess.CanJoin)]
        [InlineData("forbidden", Enums.GuestAccess.Forbidden)]
        public void ToEnum(string ga_string, Enums.GuestAccess? expected)
        {
            var content = new GuestAccessEventContent()
            {
                GuestAccess = ga_string
            };
            Assert.Equal(expected, content.ToEnum());
        }

        [Fact]
        public void ToEnum_Invalid()
        {
            var content = new GuestAccessEventContent()
            {
                GuestAccess = null!
            };
            Assert.Null(content.ToEnum());

            content.GuestAccess = "";
            Assert.Null(content.ToEnum());

            content.GuestAccess = "foo";
            Assert.Null(content.ToEnum());
        }
    }
}
