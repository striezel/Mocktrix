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
    /// Contains tests for JoinRulesEventContent.
    /// </summary>
    public class JoinRulesEventContentTests
    {
        [Fact]
        public void Construction()
        {
            var content = new JoinRulesEventContent();
            Assert.Null(content.JoinRule);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "join_rule": "public"
                       }
                       """;
            var content = JsonSerializer.Deserialize<JoinRulesEventContent>(json);

            Assert.NotNull(content);
            Assert.Equal("public", content.JoinRule);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var content = new JoinRulesEventContent
            {
                JoinRule = "public",
            };
            var expected_json = "{\"join_rule\":\"public\"}";
            var json = JsonSerializer.Serialize(content);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }

        [Theory]
        [InlineData("public", Enums.JoinRule.Public)]
        [InlineData("knock", Enums.JoinRule.Knock)]
        [InlineData("invite", Enums.JoinRule.Invite)]
        [InlineData("private", Enums.JoinRule.Private)]
        public void ToEnum(string jr_string, Enums.JoinRule expected)
        {
            var content = new JoinRulesEventContent()
            {
                JoinRule = jr_string
            };
            Assert.Equal(expected, content.ToEnum());
        }

        [Fact]
        public void ToEnum_Invalid()
        {
            var content = new JoinRulesEventContent()
            {
                JoinRule = null!
            };
            Assert.Null(content.ToEnum());

            content.JoinRule = "";
            Assert.Null(content.ToEnum());

            content.JoinRule = "foo";
            Assert.Null(content.ToEnum());
        }
    }
}