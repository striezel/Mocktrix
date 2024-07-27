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
    /// Contains tests for CallInviteEventContent.
    /// </summary>
    public class CallInviteEventContentTests
    {
        [Fact]
        public void Construction()
        {
            var content = new CallInviteEventContent();
            Assert.Null(content.CallId);
            Assert.Equal(-1, content.LifeTime);
            Assert.Null(content.Offer);
            Assert.Equal(-1, content.Version);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "call_id": "12345",
                           "lifetime": 60000,
                           "offer": {
                               "sdp": "v=0\r\no=- 6584580628695956864 2 IN IP4 127.0.0.1[...]",
                               "type": "offer"
                           },
                           "version": 0
                       }
                       """;
            var content = JsonSerializer.Deserialize<CallInviteEventContent>(json);

            Assert.NotNull(content);
            Assert.Equal("12345", content.CallId);
            Assert.Equal(60000, content.LifeTime);
            Assert.NotNull(content.Offer);
            Assert.Equal("v=0\r\no=- 6584580628695956864 2 IN IP4 127.0.0.1[...]", content.Offer.SDP);
            Assert.Equal("offer", content.Offer.Type);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var content = new CallInviteEventContent
            {
                CallId = "12345",
                LifeTime = 60000,
                Offer = new InviteOffer()
                {
                    SDP = "v=0\r\no=- 6584580628695956864 2 IN IP4 127.0.0.1[...]",
                    Type = "offer"
                },
                Version = 0
            };
            var expected_json = "{\"call_id\":\"12345\",\"lifetime\":60000,\"offer\":{\"sdp\":\"v=0\\r\\no=- 6584580628695956864 2 IN IP4 127.0.0.1[...]\",\"type\":\"offer\"},\"version\":0}";
            var json = JsonSerializer.Serialize(content);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}
