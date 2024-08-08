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
    /// Contains tests for TagEventContent.
    /// </summary>
    public class TagEventContentTests
    {
        [Fact]
        public void Construction()
        {
            var content = new TagEventContent();
            Assert.Null(content.Tags);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "tags": {
                               "u.work": {
                                   "order": 0.9
                               }
                           }
                       }
                       """;
            var content = JsonSerializer.Deserialize<TagEventContent>(json);

            Assert.NotNull(content);
            Assert.NotNull(content.Tags);
            Assert.True(content.Tags.ContainsKey("u.work"));
            Assert.True(content.Tags["u.work"].Order.HasValue);
            Assert.Equal(0.9, content.Tags["u.work"].Order);
        }

        [Fact]
        public void DeserializeMultipleTags()
        {
            var json = """ 
                       {
                         "tags": {
                           "m.favourite": {},
                           "m.lowpriority": {
                             "order": 0.5
                           },
                           "m.server_notice": {},
                           "u.work": {
                             "order": 0.9
                           }
                         }
                       }
                       """;
            var content = JsonSerializer.Deserialize<TagEventContent>(json);

            Assert.NotNull(content);
            Assert.NotNull(content.Tags);
            Assert.True(content.Tags.ContainsKey("m.favourite"));
            Assert.Null(content.Tags["m.favourite"].Order);
            Assert.True(content.Tags.ContainsKey("m.lowpriority"));
            Assert.True(content.Tags["m.lowpriority"].Order.HasValue);
            Assert.Equal(0.5, content.Tags["m.lowpriority"].Order);
            Assert.True(content.Tags.ContainsKey("m.server_notice"));
            Assert.Null(content.Tags["m.favourite"].Order);
            Assert.True(content.Tags.ContainsKey("u.work"));
            Assert.True(content.Tags["u.work"].Order.HasValue);
            Assert.Equal(0.9, content.Tags["u.work"].Order);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var content = new TagEventContent
            {
                Tags = new SortedDictionary<string, OrderInfo>()
                {
                    { "u.work", new() { Order = 0.9  } }
                }
            };
            var expected_json = "{\"tags\":{\"u.work\":{\"order\":0.9}}}";
            var json = JsonSerializer.Serialize(content);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }

        [Fact]
        public void SerializeMultipleTags()
        {
            var content = new TagEventContent
            {
                Tags = new SortedDictionary<string, OrderInfo>()
                {
                    { "m.favourite", new() },
                    { "m.lowpriority", new() { Order = 0.5  } },
                    { "m.server_notice", new() },
                    { "u.work", new() { Order = 0.9  } }
                }
            };
            var expected_json = "{\"tags\":{\"m.favourite\":{},\"m.lowpriority\":{\"order\":0.5},\"m.server_notice\":{},\"u.work\":{\"order\":0.9}}}";
            var json = JsonSerializer.Serialize(content);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}
