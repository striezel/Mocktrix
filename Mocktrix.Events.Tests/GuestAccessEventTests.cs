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
    /// Contains tests for GuestAccessEvent.
    /// </summary>
    public class GuestAccessEventTests
    {
        [Fact]
        public void Construction()
        {
            var ev = new GuestAccessEvent();
            Assert.NotNull(ev.Content);
            Assert.IsType<GuestAccessEventContent>(ev.Content);
            var content = ev.Content as GuestAccessEventContent;
            Assert.NotNull(content);
            Assert.Null(content.GuestAccess);
            Assert.Null(ev.EventId);
            Assert.Equal(0, ev.OriginServerTs);
            Assert.Null(ev.PrevContent);
            Assert.Null(ev.RoomId);
            Assert.Null(ev.Sender);
            Assert.Empty(ev.StateKey);
            Assert.Equal("m.room.guest_access", ev.Type);
            Assert.Null(ev.Unsigned);
        }

        /* Interface types are not supported for deserialization, so this fails.

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "content": {
                               "guest_access": "can_join"
                           },
                           "event_id": "$143273582443PhrSn:example.org",
                           "origin_server_ts": 1432735824653,
                           "room_id": "!jEsUZKDJdhlrceRyVU:example.org",
                           "sender": "@example:example.org",
                           "state_key": "",
                           "type": "m.room.guest_access",
                           "unsigned": {
                               "age": 1234
                           }
                       }
                       """;
            var ev = JsonSerializer.Deserialize<GuestAccessEvent>(json);

            Assert.NotNull(ev);
            Assert.NotNull(ev.Content);
            var content = ev.Content as GuestAccessEventContent;
            Assert.NotNull(content);
            Assert.Equal("can_join", content.GuestAccess);

            Assert.Equal("$143273582443PhrSn:example.org", ev.EventId);
            Assert.Equal(1432735824653, ev.OriginServerTs);
            Assert.Equal("!jEsUZKDJdhlrceRyVU:example.org", ev.RoomId);
            Assert.Equal("@example:example.org", ev.Sender);
            Assert.Empty(ev.StateKey);
            Assert.Equal("m.room.guest_access", ev.Type);
            Assert.NotNull(ev.Unsigned);
            Assert.Equal(1234, ev.Unsigned.Age);
            Assert.Null(ev.Unsigned.RedactedBecause);
            Assert.Null(ev.Unsigned.TransactionId);
        }*/

        [Fact]
        public void SerializeSpecExample()
        {
            var ev = new GuestAccessEvent
            {
                Content = new GuestAccessEventContent
                {
                    GuestAccess = "can_join",
                },
                EventId = "$143273582443PhrSn:example.org",
                OriginServerTs = 1432735824653,
                RoomId = "!jEsUZKDJdhlrceRyVU:example.org",
                Sender = "@example:example.org",
                StateKey = "",
                Type = "m.room.guest_access",
                Unsigned = new UnsignedData()
                {
                    Age = 1234,
                    RedactedBecause = null,
                    TransactionId = null
                }
            };

            var expected_json = "{\"content\":{\"guest_access\":\"can_join\"},\"event_id\":\"$143273582443PhrSn:example.org\",\"origin_server_ts\":1432735824653,\"room_id\":\"!jEsUZKDJdhlrceRyVU:example.org\",\"sender\":\"@example:example.org\",\"state_key\":\"\",\"type\":\"m.room.guest_access\",\"unsigned\":{\"age\":1234}}";
            var json = JsonSerializer.Serialize(ev);
            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}