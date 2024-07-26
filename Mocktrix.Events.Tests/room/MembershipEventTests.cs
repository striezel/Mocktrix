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

using System.Text.Encodings.Web;
using System.Text.Json;

namespace Mocktrix.Events.Tests
{
    /// <summary>
    /// Contains tests for MembershipEvent.
    /// </summary>
    public class MembershipEventTests
    {
        [Fact]
        public void Construction()
        {
            var ev = new MembershipEvent();
            Assert.NotNull(ev.Content);
            Assert.IsType<MembershipEventContent>(ev.Content);
            Assert.Null(ev.Content.AvatarURL);
            Assert.Null(ev.Content.DisplayName);
            Assert.Null(ev.Content.IsDirect);
            Assert.Null(ev.Content.Membership);
            Assert.Null(ev.Content.ThirdPartyInvite);
            Assert.Null(ev.Content.Unsigned);
            Assert.Null(ev.EventId);
            Assert.Equal(0, ev.OriginServerTs);
            Assert.Null(ev.PrevContent);
            Assert.Null(ev.RoomId);
            Assert.Null(ev.Sender);
            Assert.Null(ev.StateKey);
            Assert.Equal("m.room.member", ev.Type);
            Assert.Null(ev.Unsigned);
        }

        [Fact]
        public void DeserializeSpecExample_One()
        {
            var json = """ 
                       {
                           "content": {
                               "avatar_url": "mxc://example.org/SEsfnsuifSDFSSEF",
                               "displayname": "Alice Margatroid",
                               "membership": "join"
                           },
                           "event_id": "$143273582443PhrSn:example.org",
                           "origin_server_ts": 1432735824653,
                           "room_id": "!jEsUZKDJdhlrceRyVU:example.org",
                           "sender": "@example:example.org",
                           "state_key": "@alice:example.org",
                           "type": "m.room.member",
                           "unsigned": {
                               "age": 1234
                           }
                       }
                       """;
            var ev = JsonSerializer.Deserialize<MembershipEvent>(json);

            Assert.NotNull(ev);
            Assert.NotNull(ev.Content);
            var content = ev.Content;
            Assert.Equal("mxc://example.org/SEsfnsuifSDFSSEF", content.AvatarURL);
            Assert.Equal("Alice Margatroid", content.DisplayName);
            Assert.Null(content.IsDirect);
            Assert.Equal("join", content.Membership);
            Assert.Null(content.Unsigned);

            Assert.Equal("$143273582443PhrSn:example.org", ev.EventId);
            Assert.Equal(1432735824653, ev.OriginServerTs);
            Assert.Equal("!jEsUZKDJdhlrceRyVU:example.org", ev.RoomId);
            Assert.Equal("@example:example.org", ev.Sender);
            Assert.Equal("@alice:example.org", ev.StateKey);
            Assert.Equal("m.room.member", ev.Type);
            Assert.NotNull(ev.Unsigned);
            Assert.Equal(1234, ev.Unsigned.Age);
            Assert.Null(ev.Unsigned.RedactedBecause);
            Assert.Null(ev.Unsigned.TransactionId);
        }

        [Fact]
        public void SerializeSpecExample_One()
        {
            var ev = new MembershipEvent
            {
                Content = new MembershipEventContent
                {
                    AvatarURL = "mxc://example.org/SEsfnsuifSDFSSEF",
                    DisplayName = "Alice Margatroid",
                    Membership = "join"
                },
                EventId = "$143273582443PhrSn:example.org",
                OriginServerTs = 1432735824653,
                RoomId = "!jEsUZKDJdhlrceRyVU:example.org",
                Sender = "@example:example.org",
                StateKey = "@alice:example.org",
                Type = "m.room.member",
                Unsigned = new UnsignedData()
                {
                    Age = 1234,
                    RedactedBecause = null,
                    TransactionId = null
                }
            };

            var expected_json = "{\"content\":{\"avatar_url\":\"mxc://example.org/SEsfnsuifSDFSSEF\",\"displayname\":\"Alice Margatroid\",\"membership\":\"join\"},\"event_id\":\"$143273582443PhrSn:example.org\",\"origin_server_ts\":1432735824653,\"room_id\":\"!jEsUZKDJdhlrceRyVU:example.org\",\"sender\":\"@example:example.org\",\"state_key\":\"@alice:example.org\",\"type\":\"m.room.member\",\"unsigned\":{\"age\":1234}}";
            var json = JsonSerializer.Serialize(ev);
            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }

        [Fact]
        public void SerializeSpecExample_Three()
        {
            var ev = new MembershipEvent
            {
                Content = new MembershipEventContent
                {
                    AvatarURL = "mxc://example.org/SEsfnsuifSDFSSEF",
                    DisplayName = "Alice Margatroid",
                    Membership = "invite",
                    ThirdPartyInvite = new Invite()
                    {
                        DisplayName = "alice",
                        Signed = new InviteSignedData()
                        {
                            MatrixId = "@alice:example.org",
                            Signatures = new Dictionary<string, Dictionary<string, string>>(1)
                        {
                            {
                                "magic.forest", new Dictionary<string, string>(1)
                                {
                                    { "ed25519:3", "fQpGIW1Snz+pwLZu6sTy2aHy/DYWWTspTJRPyNp0PKkymfIsNffysMl6ObMMFdIJhk6g6pwlIqZ54rxo8SLmAg" }
                                }
                            }
                        },
                            Token = "abc123"
                        }
                    }
                },
                EventId = "$143273582443PhrSn:example.org",
                OriginServerTs = 1432735824653,
                RoomId = "!jEsUZKDJdhlrceRyVU:example.org",
                Sender = "@example:example.org",
                StateKey = "@alice:example.org",
                Type = "m.room.member",
                Unsigned = new UnsignedData()
                {
                    Age = 1234,
                    RedactedBecause = null,
                    TransactionId = null
                }
            };

            var expected_json = "{\"content\":{\"avatar_url\":\"mxc://example.org/SEsfnsuifSDFSSEF\",\"displayname\":\"Alice Margatroid\",\"membership\":\"invite\",\"third_party_invite\":{\"display_name\":\"alice\",\"signed\":{\"mxid\":\"@alice:example.org\",\"signatures\":{\"magic.forest\":{\"ed25519:3\":\"fQpGIW1Snz+pwLZu6sTy2aHy/DYWWTspTJRPyNp0PKkymfIsNffysMl6ObMMFdIJhk6g6pwlIqZ54rxo8SLmAg\"}},\"token\":\"abc123\"}}},\"event_id\":\"$143273582443PhrSn:example.org\",\"origin_server_ts\":1432735824653,\"room_id\":\"!jEsUZKDJdhlrceRyVU:example.org\",\"sender\":\"@example:example.org\",\"state_key\":\"@alice:example.org\",\"type\":\"m.room.member\",\"unsigned\":{\"age\":1234}}";
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                // Avoids the plus sign ("+") being escaped as \u002B.
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            var json = JsonSerializer.Serialize(ev, options);
            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }

        [Fact]
        public void IsStateEvent()
        {
            Assert.True(new MembershipEvent().IsStateEvent());
        }
    }
}
