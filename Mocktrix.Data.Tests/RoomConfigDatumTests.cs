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

using System.Text.Json.Nodes;

namespace Mocktrix.Data.Tests
{
    public class RoomConfigDatumTests
    {
        [Fact]
        public void Constructor()
        {
            JsonNode? node = JsonNode.Parse("""
                {
                    "song": "Feliz Navidad",
                    "songwriter": "Jose Feliciano"
                }
                """);
            Assert.NotNull(node);
            
            RoomConfigDatum data = new("@alice:matrix.example.org", "!roomyRoom:matrix.example.org", "x.mas.custom.config", node);

            Assert.NotNull(data);
            Assert.Equal("@alice:matrix.example.org", data.UserId);
            Assert.Equal("!roomyRoom:matrix.example.org", data.RoomId);
            Assert.Equal("x.mas.custom.config", data.Type);
            Assert.Equal("{\"song\":\"Feliz Navidad\",\"songwriter\":\"Jose Feliciano\"}", data.Data.ToJsonString());
        }
    }
}
