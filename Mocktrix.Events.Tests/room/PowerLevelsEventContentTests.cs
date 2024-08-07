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
    /// Contains tests for PowerLevelsEventContent.
    /// </summary>
    public class PowerLevelsEventContentTests
    {
        [Fact]
        public void Construction()
        {
            var content = new PowerLevelsEventContent();
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
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
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
                       }
                       """;
            var content = JsonSerializer.Deserialize<PowerLevelsEventContent>(json);

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
        }

        [Fact]
        public void DeserializeMaximumCanonicalJsonIntegerValue()
        {
            // According to the canonical JSON specification of Matrix, numbers
            // must be integers in the range [-(2^53)+1, (2^53)-1], so just a
            // simple 32-bit signed integer is not enough. This test checks
            // whether the maximum value can be specified for each part of the
            // power level event content.
            var json = """ 
                       {
                           "ban": 9007199254740991,
                           "events": {
                               "m.room.name": 9007199254740991,
                               "m.room.power_levels": 9007199254740991
                           },
                           "events_default": 9007199254740991,
                           "invite": 9007199254740991,
                           "kick": 9007199254740991,
                           "notifications": {
                               "room": 9007199254740991
                           },
                           "redact": 9007199254740991,
                           "state_default": 9007199254740991,
                           "users": {
                               "@example:localhost": 9007199254740991
                           },
                           "users_default": 9007199254740991
                       }
                       """;
            var content = JsonSerializer.Deserialize<PowerLevelsEventContent>(json);

            const long maxJsonInteger = 9007199254740991;
            Assert.NotNull(content);
            Assert.Equal(maxJsonInteger, content.Ban);
            Assert.NotNull(content.Events);
            Assert.Contains("m.room.name", content.Events.Keys);
            Assert.Equal(maxJsonInteger, content.Events["m.room.name"]);
            Assert.Contains("m.room.power_levels", content.Events.Keys);
            Assert.Equal(maxJsonInteger, content.Events["m.room.power_levels"]);
            Assert.Equal(maxJsonInteger, content.EventsDefault);
            Assert.Equal(maxJsonInteger, content.Invite);
            Assert.Equal(maxJsonInteger, content.Kick);
            Assert.NotNull(content.Notifications);
            Assert.Contains("room", content.Notifications.Keys);
            Assert.Equal(maxJsonInteger, content.Notifications["room"]);
            Assert.Equal(maxJsonInteger, content.Redact);
            Assert.Equal(maxJsonInteger, content.StateDefault);
            Assert.NotNull(content.Users);
            Assert.Contains("@example:localhost", content.Users.Keys);
            Assert.Equal(maxJsonInteger, content.Users["@example:localhost"]);
            Assert.Equal(maxJsonInteger, content.UsersDefault);
        }

        [Fact]
        public void DeserializeMinimumCanonicalJsonIntegerValue()
        {
            // According to the canonical JSON specification of Matrix, numbers
            // must be integers in the range [-(2^53)+1, (2^53)-1], so just a
            // simple 32-bit signed integer is not enough. This test checks
            // whether the minimum value can be specified for each part of the
            // power level event content.
            var json = """ 
                       {
                           "ban": -9007199254740991,
                           "events": {
                               "m.room.name": -9007199254740991,
                               "m.room.power_levels": -9007199254740991
                           },
                           "events_default": -9007199254740991,
                           "invite": -9007199254740991,
                           "kick": -9007199254740991,
                           "notifications": {
                               "room": -9007199254740991
                           },
                           "redact": -9007199254740991,
                           "state_default": -9007199254740991,
                           "users": {
                               "@example:localhost": -9007199254740991
                           },
                           "users_default": -9007199254740991
                       }
                       """;
            var content = JsonSerializer.Deserialize<PowerLevelsEventContent>(json);

            const long minJsonInteger = -9007199254740991;
            Assert.NotNull(content);
            Assert.Equal(minJsonInteger, content.Ban);
            Assert.NotNull(content.Events);
            Assert.Contains("m.room.name", content.Events.Keys);
            Assert.Equal(minJsonInteger, content.Events["m.room.name"]);
            Assert.Contains("m.room.power_levels", content.Events.Keys);
            Assert.Equal(minJsonInteger, content.Events["m.room.power_levels"]);
            Assert.Equal(minJsonInteger, content.EventsDefault);
            Assert.Equal(minJsonInteger, content.Invite);
            Assert.Equal(minJsonInteger, content.Kick);
            Assert.NotNull(content.Notifications);
            Assert.Contains("room", content.Notifications.Keys);
            Assert.Equal(minJsonInteger, content.Notifications["room"]);
            Assert.Equal(minJsonInteger, content.Redact);
            Assert.Equal(minJsonInteger, content.StateDefault);
            Assert.NotNull(content.Users);
            Assert.Contains("@example:localhost", content.Users.Keys);
            Assert.Equal(minJsonInteger, content.Users["@example:localhost"]);
            Assert.Equal(minJsonInteger, content.UsersDefault);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var content = new PowerLevelsEventContent
            {
                Ban = 50,
                Events = new SortedDictionary<string, long>()
                {
                    { "m.room.name", 100 },
                    { "m.room.power_levels", 100 }
                },
                EventsDefault = 0,
                Invite = 50,
                Kick = 50,
                Notifications = new SortedDictionary<string, long>()
                {
                    { "room", 20 }
                },
                Redact = 50,
                StateDefault = 50,
                Users = new SortedDictionary<string, long>()
                {
                    { "@example:localhost", 100 }
                },
                UsersDefault = 0
            };
            var expected_json = "{\"ban\":50,\"events\":{\"m.room.name\":100,\"m.room.power_levels\":100},\"events_default\":0,\"invite\":50,\"kick\":50,\"notifications\":{\"room\":20},\"redact\":50,\"state_default\":50,\"users\":{\"@example:localhost\":100},\"users_default\":0}";
            var json = JsonSerializer.Serialize(content);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }

        [Fact]
        public void GetPowerLevel_DefaultConstructed()
        {
            var content = new PowerLevelsEventContent();
            Assert.Equal(0, content.GetPowerLevel("@alice:matrix.homeserver.tld"));
            Assert.Equal(0, content.GetPowerLevel("@bob:matrix.homeserver.tld"));
            Assert.Equal(0, content.GetPowerLevel("@charlie:matrix.homeserver.tld"));
            Assert.Equal(0, content.GetPowerLevel("@dora:matrix.homeserver.tld"));
        }

        [Fact]
        public void GetPowerLevel_WithUserDefaultValue()
        {
            var content = new PowerLevelsEventContent()
            {
                UsersDefault = 42
            };
            Assert.Equal(42, content.GetPowerLevel("@alice:matrix.homeserver.tld"));
            Assert.Equal(42, content.GetPowerLevel("@bob:matrix.homeserver.tld"));
            Assert.Equal(42, content.GetPowerLevel("@charlie:matrix.homeserver.tld"));
            Assert.Equal(42, content.GetPowerLevel("@dora:matrix.homeserver.tld"));
        }

        [Fact]
        public void GetPowerLevel_WithUserSpecificValues()
        {
            var content = new PowerLevelsEventContent()
            {
                Users = new SortedDictionary<string, long>()
                {
                    { "@alice:matrix.homeserver.tld", 123},
                    { "@bob:matrix.homeserver.tld", 456}
                }
            };
            Assert.Equal(123, content.GetPowerLevel("@alice:matrix.homeserver.tld"));
            Assert.Equal(456, content.GetPowerLevel("@bob:matrix.homeserver.tld"));
            Assert.Equal(0, content.GetPowerLevel("@charlie:matrix.homeserver.tld"));
            Assert.Equal(0, content.GetPowerLevel("@dora:matrix.homeserver.tld"));
        }

        [Fact]
        public void GetPowerLevel_WithUserSpecificValuesAndUserDefaultValue()
        {
            var content = new PowerLevelsEventContent()
            {
                Users = new SortedDictionary<string, long>()
                {
                    { "@alice:matrix.homeserver.tld", 99},
                    { "@bob:matrix.homeserver.tld", 66}
                },
                UsersDefault = 21
            };
            Assert.Equal(99, content.GetPowerLevel("@alice:matrix.homeserver.tld"));
            Assert.Equal(66, content.GetPowerLevel("@bob:matrix.homeserver.tld"));
            Assert.Equal(21, content.GetPowerLevel("@charlie:matrix.homeserver.tld"));
            Assert.Equal(21, content.GetPowerLevel("@dora:matrix.homeserver.tld"));
        }
    }
}
