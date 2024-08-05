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
    /// Contains tests for AliasesEventContent.
    /// </summary>
    public class AliasesEventContentTests
    {
        [Fact]
        public void Construction()
        {
            var content = new AliasesEventContent();
            Assert.Null(content.Aliases);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """
                       {
                           "aliases": [
                               "#somewhere:example.org",
                               "#another:example.org"
                           ]
                       }
                       """;
            var content = JsonSerializer.Deserialize<AliasesEventContent>(json);

            Assert.NotNull(content);
            Assert.NotNull(content.Aliases);
            Assert.Equal(2, content.Aliases.Count);
            Assert.Contains("#somewhere:example.org", content.Aliases);
            Assert.Contains("#another:example.org", content.Aliases);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var content = new AliasesEventContent
            {
                Aliases = [ "#somewhere:example.org", "#another:example.org" ]
            };
            var expected_json = "{\"aliases\":[\"#somewhere:example.org\",\"#another:example.org\"]}";
            var json = JsonSerializer.Serialize(content);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}
