/*
    This file is part of test suite for Mocktrix.
    Copyright (C) 2025  Dirk Stolle

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
using System.Text.Json.Nodes;

namespace Mocktrix.Events.Tests
{
    /// <summary>
    /// Contains tests for ConfigDataEvent.
    /// </summary>
    public class ConfigDataEventTests
    {
        [Fact]
        public void Construction()
        {
            JsonNode? node = JsonNode.Parse("{\"a\": 1, \"b\": 2}");
            Assert.NotNull(node);

            var ev = new ConfigDataEvent()
            {
                Content = node,
                Type = "org.example.foo"
            };
            Assert.NotNull(ev.Content);
            Assert.IsType<JsonNode>(ev.Content, exactMatch: false);
            Assert.Equal("{\"a\":1,\"b\":2}", ev.Content.ToJsonString());
            Assert.Equal("org.example.foo", ev.Type);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                         "type": "org.example.custom.config",
                         "content": {
                           "custom_config_key": "custom_config_value"
                         }
                       }
                       """;
            var ev = JsonSerializer.Deserialize<ConfigDataEvent>(json);

            Assert.NotNull(ev);
            Assert.NotNull(ev.Content);
            var key = ev.Content["custom_config_key"];
            Assert.NotNull(key);
            Assert.Equal("custom_config_value", key.GetValue<string>());
            Assert.Equal("org.example.custom.config", ev.Type);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var ev = new ConfigDataEvent
            {
                Content = JsonNode.Parse("{\"custom_config_key\": \"custom_config_value\"}")!,
                Type = "org.example.custom.room.config"
            };

            var expected_json = "{\"content\":{\"custom_config_key\":\"custom_config_value\"},\"type\":\"org.example.custom.room.config\"}";
            var json = JsonSerializer.Serialize(ev);
            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }

        [Fact]
        public void IsStateEvent()
        {
            var ev = new ConfigDataEvent()
            {
                Content = JsonNode.Parse("{\"a\":1}")!,
                Type = "org.foo"
            };
            Assert.False(ev.IsStateEvent());
        }
    }
}
