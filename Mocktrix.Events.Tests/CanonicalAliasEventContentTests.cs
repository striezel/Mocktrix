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
    /// Contains tests for CanonicalAliasEventContent.
    /// </summary>
    public class CanonicalAliasEventContentTests
    {
        [Fact]
        public void Construction()
        {
            var content = new CanonicalAliasEventContent();
            Assert.Null(content.Alias);
            Assert.Null(content.AlternativeAliases);
        }

        [Fact]
        public void HasCanonicalAlias()
        {
            var content = new CanonicalAliasEventContent();
            Assert.False(content.HasCanonicalAlias());

            content.Alias = "";
            Assert.False(content.HasCanonicalAlias());

            content.Alias = "        ";
            Assert.False(content.HasCanonicalAlias());

            content.Alias = "#nice:example.org";
            Assert.True(content.HasCanonicalAlias());
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "alias": "#somewhere:localhost",
                           "alt_aliases": [
                               "#somewhere:example.org",
                               "#myroom:example.com"
                           ]
                       }
                       """;
            var content = JsonSerializer.Deserialize<CanonicalAliasEventContent>(json);

            Assert.NotNull(content);
            Assert.Equal("#somewhere:localhost", content.Alias);
            Assert.NotNull(content.AlternativeAliases);
            Assert.Equal(2, content.AlternativeAliases.Count);
            Assert.Contains("#somewhere:example.org", content.AlternativeAliases);
            Assert.Contains("#myroom:example.com", content.AlternativeAliases);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var content = new CanonicalAliasEventContent
            {
                Alias = "#somewhere:localhost",
                AlternativeAliases = ["#somewhere:example.org", "#myroom:example.com"]
            };
            var expected_json = "{\"alias\":\"#somewhere:localhost\",\"alt_aliases\":[\"#somewhere:example.org\",\"#myroom:example.com\"]}";
            var json = JsonSerializer.Serialize(content);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}