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
    /// Contains tests for CallCandidatesEvent.
    /// </summary>
    public class CallCandidatesEventTests
    {
        [Fact]
        public void Construction()
        {
            var ev = new CallCandidatesEvent();
            Assert.NotNull(ev.Content);
            Assert.IsType<CallCandidatesEventContent>(ev.Content);
            Assert.Null(ev.Content.CallId);
            Assert.Null(ev.Content.Candidates);
            Assert.Equal(-1, ev.Content.Version);
            Assert.Null(ev.EventId);
            Assert.Equal(0, ev.OriginServerTs);
            Assert.Null(ev.RoomId);
            Assert.Null(ev.Sender);
            Assert.Equal("m.call.candidates", ev.Type);
            Assert.Null(ev.Unsigned);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "content": {
                               "call_id": "12345",
                               "candidates": [
                                   {
                                       "candidate": "candidate:863018703 1 udp 2122260223 10.9.64.156 43670 typ host generation 0",
                                       "sdpMLineIndex": 0,
                                       "sdpMid": "audio"
                                   }
                               ],
                               "version": 0
                           },
                           "event_id": "$143273582443PhrSn:example.org",
                           "origin_server_ts": 1432735824653,
                           "room_id": "!jEsUZKDJdhlrceRyVU:example.org",
                           "sender": "@example:example.org",
                           "type": "m.call.candidates",
                           "unsigned": {
                               "age": 1234
                           }
                       }
                       """;
            var ev = JsonSerializer.Deserialize<CallCandidatesEvent>(json);

            Assert.NotNull(ev);
            Assert.NotNull(ev.Content);
            Assert.Equal("12345", ev.Content.CallId);
            Assert.NotNull(ev.Content.Candidates);
            Assert.Single(ev.Content.Candidates);
            Assert.Equal("candidate:863018703 1 udp 2122260223 10.9.64.156 43670 typ host generation 0", ev.Content.Candidates[0].Candidate);
            Assert.Equal(0, ev.Content.Candidates[0].SdpMLineIndex);
            Assert.Equal("audio", ev.Content.Candidates[0].SdpMid);
            Assert.Equal(0, ev.Content.Version);

            Assert.Equal("$143273582443PhrSn:example.org", ev.EventId);
            Assert.Equal(1432735824653, ev.OriginServerTs);
            Assert.Equal("!jEsUZKDJdhlrceRyVU:example.org", ev.RoomId);
            Assert.Equal("@example:example.org", ev.Sender);
            Assert.Equal("m.call.candidates", ev.Type);
            Assert.NotNull(ev.Unsigned);
            Assert.Equal(1234, ev.Unsigned.Age);
            Assert.Null(ev.Unsigned.RedactedBecause);
            Assert.Null(ev.Unsigned.TransactionId);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var ev = new CallCandidatesEvent
            {
                Content = new CallCandidatesEventContent
                {
                    CallId = "12345",
                    Candidates =
                    [
                        new()
                        {
                            Candidate = "candidate:863018703 1 udp 2122260223 10.9.64.156 43670 typ host generation 0",
                            SdpMLineIndex = 0,
                            SdpMid = "audio"
                        }
                    ],
                    Version = 0
                },
                EventId = "$143273582443PhrSn:example.org",
                OriginServerTs = 1432735824653,
                RoomId = "!jEsUZKDJdhlrceRyVU:example.org",
                Sender = "@example:example.org",
                Type = "m.call.candidates",
                Unsigned = new UnsignedData()
                {
                    Age = 1234,
                    RedactedBecause = null,
                    TransactionId = null
                }
            };

            var expected_json = "{\"content\":{\"call_id\":\"12345\",\"candidates\":[{\"candidate\":\"candidate:863018703 1 udp 2122260223 10.9.64.156 43670 typ host generation 0\",\"sdpMLineIndex\":0,\"sdpMid\":\"audio\"}],\"version\":0},\"event_id\":\"$143273582443PhrSn:example.org\",\"origin_server_ts\":1432735824653,\"room_id\":\"!jEsUZKDJdhlrceRyVU:example.org\",\"sender\":\"@example:example.org\",\"type\":\"m.call.candidates\",\"unsigned\":{\"age\":1234}}";
            var json = JsonSerializer.Serialize(ev);
            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }

        [Fact]
        public void IsStateEvent()
        {
            Assert.False(new CallCandidatesEvent().IsStateEvent());
        }
    }
}
