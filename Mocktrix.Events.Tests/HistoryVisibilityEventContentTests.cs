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
    /// Contains tests for HistoryVisibilityEventContent.
    /// </summary>
    public class HistoryVisibilityEventContentTests
    {
        [Fact]
        public void Construction()
        {
            var content = new HistoryVisibilityEventContent();
            Assert.Null(content.HistoryVisibility);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "history_visibility": "shared"
                       }
                       """;
            var content = JsonSerializer.Deserialize<HistoryVisibilityEventContent>(json);

            Assert.NotNull(content);
            Assert.Equal("shared", content.HistoryVisibility);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var content = new HistoryVisibilityEventContent
            {
                HistoryVisibility = "shared",
            };
            var expected_json = "{\"history_visibility\":\"shared\"}";
            var json = JsonSerializer.Serialize(content);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }

        [Theory]
        [InlineData("invited", Enums.HistoryVisibility.Invited)]
        [InlineData("joined", Enums.HistoryVisibility.Joined)]
        [InlineData("shared", Enums.HistoryVisibility.Shared)]
        [InlineData("world_readable", Enums.HistoryVisibility.WorldReadable)]
        public void ToEnum(string visibility_string, Enums.HistoryVisibility? expected)
        {
            var content = new HistoryVisibilityEventContent()
            {
                HistoryVisibility = visibility_string
            };
            Assert.Equal(expected, content.ToEnum());
        }

        [Fact]
        public void ToEnum_Invalid()
        {
            var content = new HistoryVisibilityEventContent()
            {
                HistoryVisibility = null!
            };
            Assert.Null(content.ToEnum());

            content.HistoryVisibility = "";
            Assert.Null(content.ToEnum());

            content.HistoryVisibility = "foo";
            Assert.Null(content.ToEnum());
        }
    }
}
