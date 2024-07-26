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
    /// Contains tests for MembershipEventContent.
    /// </summary>
    public class MembershipEventContentTests
    {
        [Fact]
        public void Construction()
        {
            var content = new MembershipEventContent();
            Assert.Null(content.AvatarURL);
            Assert.Null(content.DisplayName);
            Assert.Null(content.IsDirect);
            Assert.Null(content.Membership);
            Assert.Null(content.ThirdPartyInvite);
            Assert.Null(content.Unsigned);
        }

        [Fact]
        public void Construction_Invite()
        {
            var invite = new Invite();
            Assert.Null(invite.DisplayName);
            Assert.Null(invite.Signed);
        }

        [Fact]
        public void Construction_InviteSignedData()
        {
            var signed = new InviteSignedData();
            Assert.Null(signed.MatrixId);
            Assert.Null(signed.Signatures);
            Assert.Null(signed.Token);
        }

        [Fact]
        public void Construction_InviteUsignedMembershipData()
        {
            var unsigned = new UnsignedMembershipData();
            Assert.Null(unsigned.InviteRoomState);
        }

        [Fact]
        public void DeserializeSpecExample_One()
        {
            var json = """ 
                       {
                           "avatar_url": "mxc://example.org/SEsfnsuifSDFSSEF",
                           "displayname": "Alice Margatroid",
                           "membership": "join"
                       }
                       """;
            var content = JsonSerializer.Deserialize<MembershipEventContent>(json);

            Assert.NotNull(content);
            Assert.Equal("mxc://example.org/SEsfnsuifSDFSSEF", content.AvatarURL);
            Assert.Equal("Alice Margatroid", content.DisplayName);
            Assert.Null(content.IsDirect);
            Assert.Equal("join", content.Membership);
        }

        [Fact]
        public void DeserializeSpecExample_Two()
        {
            var json = """ 
                       {
                           "avatar_url": "mxc://example.org/SEsfnsuifSDFSSEF",
                           "displayname": "Alice Margatroid",
                           "membership": "invite"
                       }
                       """;
            var content = JsonSerializer.Deserialize<MembershipEventContent>(json);

            Assert.NotNull(content);
            Assert.Equal("mxc://example.org/SEsfnsuifSDFSSEF", content.AvatarURL);
            Assert.Equal("Alice Margatroid", content.DisplayName);
            Assert.Null(content.IsDirect);
            Assert.Equal("invite", content.Membership);
        }

        [Fact]
        public void DeserializeSpecExample_Three()
        {
            var json = """ 
                       {
                           "avatar_url": "mxc://example.org/SEsfnsuifSDFSSEF",
                           "displayname": "Alice Margatroid",
                           "membership": "invite",
                           "third_party_invite": {
                               "display_name": "alice",
                               "signed": {
                                   "mxid": "@alice:example.org",
                                   "signatures": {
                                       "magic.forest": {
                                           "ed25519:3": "fQpGIW1Snz+pwLZu6sTy2aHy/DYWWTspTJRPyNp0PKkymfIsNffysMl6ObMMFdIJhk6g6pwlIqZ54rxo8SLmAg"
                                       }
                                   },
                                   "token": "abc123"
                               }
                           }
                       }
                       """;
            var content = JsonSerializer.Deserialize<MembershipEventContent>(json);

            Assert.NotNull(content);
            Assert.Equal("mxc://example.org/SEsfnsuifSDFSSEF", content.AvatarURL);
            Assert.Equal("Alice Margatroid", content.DisplayName);
            Assert.Null(content.IsDirect);
            Assert.Equal("invite", content.Membership);
            Assert.NotNull(content.ThirdPartyInvite);
            Assert.Equal("alice", content.ThirdPartyInvite.DisplayName);
            Assert.NotNull(content.ThirdPartyInvite.Signed);
            Assert.Equal("@alice:example.org", content.ThirdPartyInvite.Signed.MatrixId);
            Assert.NotNull(content.ThirdPartyInvite.Signed.Signatures);
            Assert.Single(content.ThirdPartyInvite.Signed.Signatures);
            Assert.True(content.ThirdPartyInvite.Signed.Signatures.ContainsKey("magic.forest"));
            var magic_forest = content.ThirdPartyInvite.Signed.Signatures["magic.forest"];
            Assert.NotNull(magic_forest);
            Assert.Single(magic_forest);
            Assert.True(magic_forest.ContainsKey("ed25519:3"));
            Assert.Equal("fQpGIW1Snz+pwLZu6sTy2aHy/DYWWTspTJRPyNp0PKkymfIsNffysMl6ObMMFdIJhk6g6pwlIqZ54rxo8SLmAg", magic_forest["ed25519:3"]);
            Assert.Equal("abc123", content.ThirdPartyInvite.Signed.Token);
        }

        [Fact]
        public void SerializeSpecExample_One()
        {
            var content = new MembershipEventContent
            {
                AvatarURL = "mxc://example.org/SEsfnsuifSDFSSEF",
                DisplayName = "Alice Margatroid",
                Membership = "join"
            };
            var expected_json = "{\"avatar_url\":\"mxc://example.org/SEsfnsuifSDFSSEF\",\"displayname\":\"Alice Margatroid\",\"membership\":\"join\"}";
            var json = JsonSerializer.Serialize(content);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }

        [Fact]
        public void SerializeSpecExample_Two()
        {
            var content = new MembershipEventContent
            {
                AvatarURL = "mxc://example.org/SEsfnsuifSDFSSEF",
                DisplayName = "Alice Margatroid",
                Membership = "invite"
            };
            var expected_json = "{\"avatar_url\":\"mxc://example.org/SEsfnsuifSDFSSEF\",\"displayname\":\"Alice Margatroid\",\"membership\":\"invite\"}";
            var json = JsonSerializer.Serialize(content);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }

        [Fact]
        public void SerializeSpecExample_Three()
        {
            var content = new MembershipEventContent
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
            };
            var expected_json = "{\"avatar_url\":\"mxc://example.org/SEsfnsuifSDFSSEF\",\"displayname\":\"Alice Margatroid\",\"membership\":\"invite\",\"third_party_invite\":{\"display_name\":\"alice\",\"signed\":{\"mxid\":\"@alice:example.org\",\"signatures\":{\"magic.forest\":{\"ed25519:3\":\"fQpGIW1Snz+pwLZu6sTy2aHy/DYWWTspTJRPyNp0PKkymfIsNffysMl6ObMMFdIJhk6g6pwlIqZ54rxo8SLmAg\"}},\"token\":\"abc123\"}}}";
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                // Avoids the plus sign ("+") being escaped as \u002B.
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            var json = JsonSerializer.Serialize(content, options);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }

        [Theory]
        [InlineData("invite", Enums.Membership.Invite)]
        [InlineData("join", Enums.Membership.Join)]
        [InlineData("knock", Enums.Membership.Knock)]
        [InlineData("leave", Enums.Membership.Leave)]
        [InlineData("ban", Enums.Membership.Ban)]
        public void MembershipAsEnum(string membership_string, Enums.Membership? expected)
        {
            var content = new MembershipEventContent()
            {
                Membership = membership_string
            };
            Assert.Equal(expected, content.MembershipAsEnum());
        }

        [Fact]
        public void MembershipAsEnum_Invalid()
        {
            var content = new MembershipEventContent()
            {
                Membership = null!
            };
            Assert.Null(content.MembershipAsEnum());

            content.Membership = "";
            Assert.Null(content.MembershipAsEnum());

            content.Membership = "foo";
            Assert.Null(content.MembershipAsEnum());
        }
    }
}
