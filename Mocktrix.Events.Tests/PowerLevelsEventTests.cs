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
    /// Contains tests for PowerLevelsEvent.
    /// </summary>
    public class PowerLevelsEventTests
    {
        [Fact]
        public void Construction()
        {
            var ev = new PowerLevelsEvent();
            Assert.NotNull(ev.Content);
            Assert.IsType<PowerLevelsEventContent>(ev.Content);
            var content = ev.Content as PowerLevelsEventContent;
            Assert.NotNull(content);
            Assert.Null(content.Ban);
            Assert.Null(content.Events);
            Assert.Null(content.EventsDefault);
            Assert.Null(content.Invite);
            Assert.Null(content.Kick);
            Assert.Null(content.Notifications);
            Assert.Null(content.Redact);
            Assert.Null(content.StateDefault);
            Assert.Null(content.Users);
            Assert.Null(content.UsersDefault);
            Assert.Null(ev.EventId);
            Assert.Equal(0, ev.OriginServerTs);
            Assert.Null(ev.PrevContent);
            Assert.Null(ev.RoomId);
            Assert.Null(ev.Sender);
            Assert.Empty(ev.StateKey);
            Assert.Equal("m.room.power_levels", ev.Type);
            Assert.Null(ev.Unsigned);
        }

        /* Interface types are not supported for serialization, so this fails.

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "content": {
                               "ban": 50,
                               "events": {
                                   "m.room.name": 100,
                                   "m.room.power_levels": 100
                               },
                               "events_default": 0,
                               "invite": 50,
                               "kick": 50,
                               "notifications": {
                                   "room": 20
                               },
                               "redact": 50,
                               "state_default": 50,
                               "users": {
                                   "@example:localhost": 100
                               },
                               "users_default": 0
                           },
                           "event_id": "$143273582443PhrSn:example.org",
                           "origin_server_ts": 1432735824653,
                           "room_id": "!jEsUZKDJdhlrceRyVU:example.org",
                           "sender": "@example:example.org",
                           "state_key": "",
                           "type": "m.room.power_levels",
                           "unsigned": {
                               "age": 1234
                           }
                       }
                       """;
            var ev = JsonSerializer.Deserialize<PowerLevelsEvent>(json);

            Assert.NotNull(ev);
            Assert.NotNull(ev.Content);
            var content = ev.Content as PowerLevelsEventContent;
            Assert.NotNull(content);
            Assert.Equal(50, content.Ban);
            Assert.NotNull(content.Events);
            Assert.Contains("m.room.name", content.Events.Keys);
            Assert.Equal(100, content.Events["m.room.name"]);
            Assert.Contains("m.room.power_levels", content.Events.Keys);
            Assert.Equal(100, content.Events["m.room.power_levels"]);
            Assert.Equal(0, content.EventsDefault);
            Assert.Equal(50, content.Invite);
            Assert.Equal(50, content.Kick);
            Assert.NotNull(content.Notifications);
            Assert.Contains("room", content.Notifications.Keys);
            Assert.Equal(20, content.Notifications["room"]);
            Assert.Equal(50, content.Redact);
            Assert.Equal(50, content.StateDefault);
            Assert.NotNull(content.Users);
            Assert.Contains("@example:localhost", content.Users.Keys);
            Assert.Equal(100, content.Users["@example:localhost"]);
            Assert.Equal(0, content.UsersDefault);

            Assert.Equal("$143273582443PhrSn:example.org", ev.EventId);
            Assert.Equal(1432735824653, ev.OriginServerTs);
            Assert.Equal("!jEsUZKDJdhlrceRyVU:example.org", ev.RoomId);
            Assert.Equal("@example:example.org", ev.Sender);
            Assert.Empty(ev.StateKey);
            Assert.Equal("m.room.power_levels", ev.Type);
            Assert.NotNull(ev.Unsigned);
            Assert.Equal(1234, ev.Unsigned.Age);
            Assert.Null(ev.Unsigned.RedactedBecause);
            Assert.Null(ev.Unsigned.TransactionId);
        }*/

        [Fact]
        public void SerializeSpecExample()
        {
            var ev = new PowerLevelsEvent
            {
                Content = new PowerLevelsEventContent
                {
                    Ban = 50,
                    Events = new SortedDictionary<string, int>()
                    {
                        { "m.room.name", 100 },
                        { "m.room.power_levels", 100 }
                    },
                    EventsDefault = 0,
                    Invite = 50,
                    Kick = 50,
                    Notifications = new SortedDictionary<string, int>()
                    {
                        { "room", 20 }
                    },
                    Redact = 50,
                    StateDefault = 50,
                    Users = new SortedDictionary<string, int>()
                    {
                        { "@example:localhost", 100 }
                    },
                    UsersDefault = 0
                },
                EventId = "$143273582443PhrSn:example.org",
                OriginServerTs = 1432735824653,
                RoomId = "!jEsUZKDJdhlrceRyVU:example.org",
                Sender = "@example:example.org",
                StateKey = "",
                Type = "m.room.power_levels",
                Unsigned = new UnsignedData()
                {
                    Age = 1234,
                    RedactedBecause = null,
                    TransactionId = null
                }
            };

            var expected_json = "{\"content\":{\"ban\":50,\"events\":{\"m.room.name\":100,\"m.room.power_levels\":100},\"events_default\":0,\"invite\":50,\"kick\":50,\"notifications\":{\"room\":20},\"redact\":50,\"state_default\":50,\"users\":{\"@example:localhost\":100},\"users_default\":0},\"event_id\":\"$143273582443PhrSn:example.org\",\"origin_server_ts\":1432735824653,\"room_id\":\"!jEsUZKDJdhlrceRyVU:example.org\",\"sender\":\"@example:example.org\",\"state_key\":\"\",\"type\":\"m.room.power_levels\",\"unsigned\":{\"age\":1234}}";
            var json = JsonSerializer.Serialize(ev);
            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}
