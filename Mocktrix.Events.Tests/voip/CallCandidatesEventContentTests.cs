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
    /// Contains tests for CallCandidatesEventContent.
    /// </summary>
    public class CallCandidatesEventContentTests
    {
        [Fact]
        public void Construction()
        {
            var content = new CallCandidatesEventContent();
            Assert.Null(content.CallId);
            Assert.Null(content.Candidates);
            Assert.Equal(-1, content.Version);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "call_id": "12345",
                           "candidates": [
                               {
                                   "candidate": "candidate:863018703 1 udp 2122260223 10.9.64.156 43670 typ host generation 0",
                                   "sdpMLineIndex": 0,
                                   "sdpMid": "audio"
                               }
                           ],
                           "version": 0
                       }
                       """;
            var content = JsonSerializer.Deserialize<CallCandidatesEventContent>(json);

            Assert.NotNull(content);
            Assert.Equal("12345", content.CallId);
            Assert.NotNull(content.Candidates);
            Assert.Single(content.Candidates);
            Assert.Equal("candidate:863018703 1 udp 2122260223 10.9.64.156 43670 typ host generation 0", content.Candidates[0].Candidate);
            Assert.Equal(0, content.Candidates[0].SdpMLineIndex);
            Assert.Equal("audio", content.Candidates[0].SdpMid);
            Assert.Equal(0, content.Version);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var content = new CallCandidatesEventContent
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
            };
            var expected_json = "{\"call_id\":\"12345\",\"candidates\":[{\"candidate\":\"candidate:863018703 1 udp 2122260223 10.9.64.156 43670 typ host generation 0\",\"sdpMLineIndex\":0,\"sdpMid\":\"audio\"}],\"version\":0}";
            var json = JsonSerializer.Serialize(content);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}
