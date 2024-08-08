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
    /// Contains tests for TagEvent.
    /// </summary>
    public class TagEventTests
    {
        [Fact]
        public void Construction()
        {
            var ev = new TagEvent();
            Assert.NotNull(ev.Content);
            Assert.IsType<TagEventContent>(ev.Content);
            Assert.Null(ev.Content.Tags);
            Assert.Equal("m.tag", ev.Type);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "content": {
                               "tags": {
                                   "u.work": {
                                       "order": 0.9
                                   }
                               }
                           },
                           "type": "m.tag"
                       }
                       """;
            var ev = JsonSerializer.Deserialize<TagEvent>(json);

            Assert.NotNull(ev);
            Assert.NotNull(ev.Content);
            Assert.NotNull(ev.Content.Tags);
            Assert.True(ev.Content.Tags.ContainsKey("u.work"));
            Assert.Equal(0.9, ev.Content.Tags["u.work"].Order);
            Assert.Equal("m.tag", ev.Type);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var ev = new TagEvent
            {
                Content = new TagEventContent
                {
                    Tags = new SortedDictionary<string, OrderInfo>()
                    {
                        { "u.work", new() { Order = 0.9 } }
                    }
                },
                Type = "m.tag"
            };

            var expected_json = "{\"content\":{\"tags\":{\"u.work\":{\"order\":0.9}}},\"type\":\"m.tag\"}";
            var json = JsonSerializer.Serialize(ev);
            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }

        [Fact]
        public void IsStateEvent()
        {
            Assert.False(new TagEvent().IsStateEvent());
        }
    }
}
