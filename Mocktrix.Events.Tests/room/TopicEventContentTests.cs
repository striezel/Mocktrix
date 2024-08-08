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
    /// Contains tests for TopicEventContent.
    /// </summary>
    public class TopicEventContentTests
    {
        [Fact]
        public void Construction()
        {
            var content = new TopicEventContent();
            Assert.Null(content.Topic);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "topic": "A room topic"
                       }
                       """;
            var content = JsonSerializer.Deserialize<TopicEventContent>(json);

            Assert.NotNull(content);
            Assert.Equal("A room topic", content.Topic);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var content = new TopicEventContent
            {
                Topic = "A room topic",
            };
            var expected_json = "{\"topic\":\"A room topic\"}";
            var json = JsonSerializer.Serialize(content);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}
