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
    /// Contains tests for CanonicalAliasEvent.
    /// </summary>
    public class CanonicalAliasEventTests
    {
        [Fact]
        public void Construction()
        {
            var ev = new CanonicalAliasEvent();
            Assert.NotNull(ev.Content);
            Assert.IsType<CanonicalAliasEventContent>(ev.Content);
            var content = ev.Content;
            Assert.NotNull(content);
            Assert.Null(content.Alias);
            Assert.Null(content.Alias);
            Assert.Null(ev.EventId);
            Assert.Equal(0, ev.OriginServerTs);
            Assert.Null(ev.PrevContent);
            Assert.Null(ev.RoomId);
            Assert.Null(ev.Sender);
            Assert.Empty(ev.StateKey);
            Assert.Equal("m.room.canonical_alias", ev.Type);
            Assert.Null(ev.Unsigned);
        }


        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "content": {
                               "alias": "#somewhere:localhost",
                               "alt_aliases": [
                                   "#somewhere:example.org",
                                   "#myroom:example.com"
                               ]
                           },
                           "event_id": "$143273582443PhrSn:example.org",
                           "origin_server_ts": 1432735824653,
                           "room_id": "!jEsUZKDJdhlrceRyVU:example.org",
                           "sender": "@example:example.org",
                           "state_key": "",
                           "type": "m.room.canonical_alias",
                           "unsigned": {
                               "age": 1234
                           }
                       }
                       """;
            var ev = JsonSerializer.Deserialize<CanonicalAliasEvent>(json);

            Assert.NotNull(ev);
            Assert.NotNull(ev.Content);
            var content = ev.Content;
            Assert.Equal("#somewhere:localhost", content.Alias);
            Assert.NotNull(content.AlternativeAliases);
            Assert.Equal(2, content.AlternativeAliases.Count);
            Assert.Contains("#somewhere:example.org", content.AlternativeAliases);
            Assert.Contains("#myroom:example.com", content.AlternativeAliases);

            Assert.Equal("$143273582443PhrSn:example.org", ev.EventId);
            Assert.Equal(1432735824653, ev.OriginServerTs);
            Assert.Equal("!jEsUZKDJdhlrceRyVU:example.org", ev.RoomId);
            Assert.Equal("@example:example.org", ev.Sender);
            Assert.Empty(ev.StateKey);
            Assert.Equal("m.room.canonical_alias", ev.Type);
            Assert.NotNull(ev.Unsigned);
            Assert.Equal(1234, ev.Unsigned.Age);
            Assert.Null(ev.Unsigned.RedactedBecause);
            Assert.Null(ev.Unsigned.TransactionId);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var ev = new CanonicalAliasEvent
            {
                Content = new CanonicalAliasEventContent
                {
                    Alias = "#somewhere:localhost",
                    AlternativeAliases = ["#somewhere:example.org", "#myroom:example.com"]
                },
                EventId = "$143273582443PhrSn:example.org",
                OriginServerTs = 1432735824653,
                RoomId = "!jEsUZKDJdhlrceRyVU:example.org",
                Sender = "@example:example.org",
                StateKey = "",
                Type = "m.room.canonical_alias",
                Unsigned = new UnsignedData()
                {
                    Age = 1234,
                    RedactedBecause = null,
                    TransactionId = null
                }
            };

            var expected_json = "{\"content\":{\"alias\":\"#somewhere:localhost\",\"alt_aliases\":[\"#somewhere:example.org\",\"#myroom:example.com\"]},\"event_id\":\"$143273582443PhrSn:example.org\",\"origin_server_ts\":1432735824653,\"room_id\":\"!jEsUZKDJdhlrceRyVU:example.org\",\"sender\":\"@example:example.org\",\"state_key\":\"\",\"type\":\"m.room.canonical_alias\",\"unsigned\":{\"age\":1234}}";
            var json = JsonSerializer.Serialize(ev);
            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }

        [Fact]
        public void IsStateEvent()
        {
            Assert.True(new CanonicalAliasEvent().IsStateEvent());
        }
    }
}
