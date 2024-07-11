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
    public class CreateRoomEventContentTests
    {
        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "creator": "@example:example.org",
                           "m.federate": true,
                           "predecessor": {
                               "event_id": "$something:example.org",
                               "room_id": "!oldroom:example.org"
                           },
                           "room_version": "1"
                       }
                       """;
            var content = JsonSerializer.Deserialize<CreateRoomEventContent>(json);

            Assert.NotNull(content);
            Assert.Equal("@example:example.org", content.Creator);
            Assert.True(content.Federate);
            Assert.NotNull(content.Predecessor);
            Assert.Equal("$something:example.org", content.Predecessor.EventId);
            Assert.Equal("!oldroom:example.org", content.Predecessor.RoomId);
            Assert.Equal("1", content.Version);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var content = new CreateRoomEventContent
            {
                Creator = "@example:example.org",
                Federate = true,
                Predecessor = new PreviousRoom()
                {
                    EventId = "$something:example.org",
                    RoomId = "!oldroom:example.org"
                },
                Version = "1"
            };
            var expected_json = "{\"creator\":\"@example:example.org\",\"m.federate\":true,\"predecessor\":{\"event_id\":\"$something:example.org\",\"room_id\":\"!oldroom:example.org\"},\"room_version\":\"1\"}";
            var json = JsonSerializer.Serialize(content);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}