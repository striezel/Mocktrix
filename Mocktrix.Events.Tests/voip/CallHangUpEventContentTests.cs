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

namespace Mocktrix.Events.VoIP.Tests
{
    /// <summary>
    /// Contains tests for CallHangUpEventContent.
    /// </summary>
    public class CallHangUpEventContentTests
    {
        [Fact]
        public void Construction()
        {
            var content = new CallHangUpEventContent();
            Assert.Null(content.CallId);
            Assert.Null(content.Reason);
            Assert.Equal(-1, content.Version);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "call_id": "12345",
                           "version": 0
                       }
                       """;
            var content = JsonSerializer.Deserialize<CallHangUpEventContent>(json);

            Assert.NotNull(content);
            Assert.Equal("12345", content.CallId);
            Assert.Null(content.Reason);
            Assert.Equal(0, content.Version);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var content = new CallHangUpEventContent
            {
                CallId = "12345",
                Reason = null,
                Version = 0
            };
            var expected_json = "{\"call_id\":\"12345\",\"version\":0}";
            var json = JsonSerializer.Serialize(content);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}
