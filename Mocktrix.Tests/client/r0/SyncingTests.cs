/*
    This file is part of test suite for Mocktrix.
    Copyright (C) 2024, 2025  Dirk Stolle

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

using System.Net;
using System.Text.Json.Nodes;

namespace MocktrixTests.client.r0
{
#pragma warning disable IDE1006 // naming style
    /// <summary>
    /// Class to test account data events in a /sync response.
    /// </summary>
    internal class AccountDataEvent
    {
        public JsonNode? content { get; set; } = null;
        public string? type { get; set; } = null;
    }

    /// <summary>
    /// Class to test room-specific data in a /sync response.
    /// </summary>
    internal class PseudoRoom
    {
        public AccountDataInRoom? account_data { get; set; } = null;
    }

    /// <summary>
    /// Class to test room-specific account data events in a /sync response.
    /// </summary>
    internal class AccountDataInRoom
    {
        public List<AccountDataEvent>? events { get; set; } = null;
    }
#pragma warning restore IDE1006 // naming style

    public class SyncingTests
    {
        private readonly HttpClient client = new()
        {
            BaseAddress = Utilities.BaseAddress
        };

        [Fact]
        public async Task TestSync_NoAuthorization()
        {
            var response = await client.GetAsync("/_matrix/client/r0/sync", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_MISSING_TOKEN",
                error = "Missing access token."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestSync_InvalidAccessToken()
        {
            HttpClient unauthenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            unauthenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer foobar");
            var response = await unauthenticated_client.GetAsync("/_matrix/client/r0/sync", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var expected = new
            {
                errcode = "M_UNKNOWN_TOKEN",
                error = "Unrecognized access token."
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.errcode, content.errcode);
            Assert.Equal(expected.error, content.error);
        }

        [Fact]
        public async Task TestSync_Success()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client);

            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);
            var response = await authenticated_client.GetAsync("/_matrix/client/r0/sync", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Equal("{\"next_batch\":\"not_implemented\"}", content);
        }

        [Fact]
        public async Task TestSync_Success_WithAccountData()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client, "sync_user_with_account_data");

            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);
            var response = await authenticated_client.GetAsync("/_matrix/client/r0/sync", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var foo_object = JsonNode.Parse("{\"foo\":\"bar\",\"baz\":\"quux\"}");
            var hey_object = JsonNode.Parse("{\"hey\":\"there\",\"go\":true}");

            var join_object = JsonNode.Parse("{\"what\":\"joined room config data\",\"count\":3}");
            var left_object = JsonNode.Parse("{\"what\":\"left room config data\",\"count\":5}");

            var expected = new
            {
                account_data = new
                {
                    events = new List<AccountDataEvent>
                    {
                        new()
                        {
                            content = foo_object,
                            type = "org.example.foo"
                        },
                        new()
                        {
                            content = hey_object,
                            type = "org.test.hey"
                        }
                    }
                },
                next_batch = "not_implemented",
                rooms = new
                {
                    join = new Dictionary<string, PseudoRoom>
                     {
                        {
                            "!joined_room_with_account_data:matrix.example.org",
                            new PseudoRoom()
                            {
                                account_data = new AccountDataInRoom()
                                {
                                    events =
                                    [
                                        new()
                                        {
                                            content = join_object,
                                            type = "test.join.data"
                                        }
                                    ]
                                }
                            }
                        }
                    },
                    leave = new Dictionary<string, PseudoRoom>
                    {
                        {
                            "!left_room_with_account_data:matrix.example.org",
                            new PseudoRoom()
                            {
                                account_data = new()
                                {
                                    events =
                                    [
                                        new()
                                        {
                                            content = left_object,
                                            type = "test.leave.data"
                                        }
                                    ]
                                }
                            }
                        }
                    }
                }
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.next_batch, content.next_batch);
            Assert.NotNull(content.account_data);
            Assert.NotNull(content.account_data.events);
            Assert.Equal(2, content.account_data.events.Count);

            {
                var foo_event = content.account_data.events.Find(e => e.type == "org.example.foo");
                Assert.NotNull(foo_event);
                Assert.Equal(expected.account_data.events[0].type, foo_event.type);
                Assert.NotNull(foo_event.content);
                Assert.IsType<JsonObject>(foo_event.content);
                var obj = foo_event.content as JsonObject;
                Assert.NotNull(obj);
                Assert.Equal("bar", obj["foo"]!.GetValue<string>());
                Assert.Equal("quux", obj["baz"]!.GetValue<string>());
            }

            {
                var hey_event = content.account_data.events.Find(e => e.type == "org.test.hey");
                Assert.NotNull(hey_event);
                Assert.Equal(expected.account_data.events[1].type, hey_event.type);
                Assert.NotNull(hey_event.content);
                Assert.IsType<JsonObject>(hey_event.content);
                var obj = hey_event.content as JsonObject;
                Assert.NotNull(obj);
                Assert.Equal("there", obj["hey"]!.GetValue<string>());
                Assert.True(obj["go"]!.GetValue<bool>());
            }

            Assert.NotNull(content.rooms);
            Assert.NotNull(content.rooms.join);
            Assert.Contains("!joined_room_with_account_data:matrix.example.org", content.rooms.join);
            var joined_room = content.rooms.join["!joined_room_with_account_data:matrix.example.org"];
            Assert.NotNull(joined_room.account_data);
            Assert.NotNull(joined_room.account_data.events);
            Assert.Single(joined_room.account_data.events);

            {
                var join_event = joined_room.account_data.events[0];
                Assert.NotNull(join_event);
                var expected_acc_data = expected.rooms.join["!joined_room_with_account_data:matrix.example.org"].account_data;
                Assert.NotNull(expected_acc_data);
                Assert.NotNull(expected_acc_data.events);
                Assert.Equal(expected_acc_data.events[0].type, join_event.type);
                var obj = join_event.content as JsonObject;
                Assert.NotNull(obj);
                Assert.Equal("joined room config data", obj["what"]!.GetValue<string>());
                Assert.Equal(3, obj["count"]!.GetValue<int>());
            }

            Assert.NotNull(content.rooms.leave);
            Assert.Contains("!left_room_with_account_data:matrix.example.org", content.rooms.leave);
            var left_room = content.rooms.leave["!left_room_with_account_data:matrix.example.org"];
            Assert.NotNull(left_room.account_data);
            Assert.NotNull(left_room.account_data.events);
            Assert.Single(left_room.account_data.events);

            {
                var leave_event = left_room.account_data.events[0];
                Assert.NotNull(leave_event);
                var expected_acc_data = expected.rooms.leave["!left_room_with_account_data:matrix.example.org"].account_data;
                Assert.NotNull(expected_acc_data);
                Assert.NotNull(expected_acc_data.events);
                Assert.Equal(expected_acc_data.events[0].type, leave_event.type);
                var obj = leave_event.content as JsonObject;
                Assert.NotNull(obj);
                Assert.Equal("left room config data", obj["what"]!.GetValue<string>());
                Assert.Equal(5, obj["count"]!.GetValue<int>());
            }

            var plain_text = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            // Note: Technically, this assertion could fail, if the order of account_data events changes.
            Assert.Equal("{\"account_data\":{\"events\":[{\"content\":{\"foo\":\"bar\",\"baz\":\"quux\"},\"type\":\"org.example.foo\"},{\"content\":{\"hey\":\"there\",\"go\":true},\"type\":\"org.test.hey\"}]},\"next_batch\":\"not_implemented\",\"rooms\":{\"join\":{\"!joined_room_with_account_data:matrix.example.org\":{\"account_data\":{\"events\":[{\"content\":{\"what\":\"joined room config data\",\"count\":3},\"type\":\"test.join.data\"}]}}},\"leave\":{\"!left_room_with_account_data:matrix.example.org\":{\"account_data\":{\"events\":[{\"content\":{\"what\":\"left room config data\",\"count\":5},\"type\":\"test.leave.data\"}]}}}}}", plain_text);
        }

        [Fact]
        public async Task TestSync_Success_WithTagData()
        {
            // We need to be logged in and have an access token before we can
            // use the endpoint. So let's do the login first.
            var access_token = await Utilities.PerformLogin(client, "sync_tag_user");

            HttpClient authenticated_client = new()
            {
                BaseAddress = Utilities.BaseAddress
            };
            authenticated_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);
            var response = await authenticated_client.GetAsync("/_matrix/client/r0/sync", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            var join_object = JsonNode.Parse("""
                {
                  "tags": {
                    "m.favourite": {
                      "order": 0.25
                    },
                    "u.null": { },
                    "u.some_tag": {
                      "order": 1.0
                    }
                  }
                }
                """);

            var left_object = JsonNode.Parse("""
                {
                  "tags": {
                    "u.null": { },
                    "u.ooooh": {
                      "order": 0.25
                    },
                    "u.some_tag": {
                      "order": 0.75
                    }
                  }
                }
                """);

            var expected = new
            {
                next_batch = "not_implemented",
                rooms = new
                {
                    join = new Dictionary<string, PseudoRoom>
                     {
                        {
                            "!joined_room_with_some_tags:matrix.example.org",
                            new PseudoRoom()
                            {
                                account_data = new AccountDataInRoom()
                                {
                                    events =
                                    [
                                        new()
                                        {
                                            content = join_object,
                                            type = "m.tag"
                                        }
                                    ]
                                }
                            }
                        }
                    },
                    leave = new Dictionary<string, PseudoRoom>
                    {
                        {
                            "!left_room_with_some_tags:matrix.example.org",
                            new PseudoRoom()
                            {
                                account_data = new()
                                {
                                    events =
                                    [
                                        new()
                                        {
                                            content = left_object,
                                            type = "m.tag"
                                        }
                                    ]
                                }
                            }
                        }
                    }
                }
            };

            var content = Utilities.GetContent(response, expected);
            Assert.Equal(expected.next_batch, content.next_batch);

            Assert.NotNull(content.rooms);
            Assert.NotNull(content.rooms.join);
            Assert.Contains("!joined_room_with_some_tags:matrix.example.org", content.rooms.join);
            var joined_room = content.rooms.join["!joined_room_with_some_tags:matrix.example.org"];
            Assert.NotNull(joined_room.account_data);
            Assert.NotNull(joined_room.account_data.events);
            Assert.Single(joined_room.account_data.events);

            {
                var join_event = joined_room.account_data.events[0];
                Assert.NotNull(join_event);
                var expected_acc_data = expected.rooms.join["!joined_room_with_some_tags:matrix.example.org"].account_data;
                Assert.NotNull(expected_acc_data);
                Assert.NotNull(expected_acc_data.events);
                Assert.Equal(expected_acc_data.events[0].type, join_event.type);
                var obj = join_event.content as JsonObject;
                Assert.NotNull(obj);
                Assert.Equal(0.25, obj["tags"]!["m.favourite"]!["order"]!.GetValue<double>());
                Assert.Equal(1.0, obj["tags"]!["u.some_tag"]!["order"]!.GetValue<double>());
            }

            Assert.NotNull(content.rooms.leave);
            Assert.Contains("!left_room_with_some_tags:matrix.example.org", content.rooms.leave);
            var left_room = content.rooms.leave["!left_room_with_some_tags:matrix.example.org"];
            Assert.NotNull(left_room.account_data);
            Assert.NotNull(left_room.account_data.events);
            Assert.Single(left_room.account_data.events);

            {
                var leave_event = left_room.account_data.events[0];
                Assert.NotNull(leave_event);
                var expected_acc_data = expected.rooms.leave["!left_room_with_some_tags:matrix.example.org"].account_data;
                Assert.NotNull(expected_acc_data);
                Assert.NotNull(expected_acc_data.events);
                Assert.Equal(expected_acc_data.events[0].type, leave_event.type);
                var obj = leave_event.content as JsonObject;
                Assert.NotNull(obj);
                Assert.Equal(0.25, obj["tags"]!["u.ooooh"]!["order"]!.GetValue<double>());
                Assert.Equal(0.75, obj["tags"]!["u.some_tag"]!["order"]!.GetValue<double>());
            }

            var plain_text = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            // Note: Technically, this assertion could fail, if the order of account_data events changes.
            Assert.Equal("{\"next_batch\":\"not_implemented\",\"rooms\":{\"join\":{\"!joined_room_with_some_tags:matrix.example.org\":{\"account_data\":{\"events\":[{\"content\":{\"tags\":{\"m.favourite\":{\"order\":0.25},\"u.null\":{},\"u.some_tag\":{\"order\":1}}},\"type\":\"m.tag\"}]}}},\"leave\":{\"!left_room_with_some_tags:matrix.example.org\":{\"account_data\":{\"events\":[{\"content\":{\"tags\":{\"u.null\":{},\"u.ooooh\":{\"order\":0.25},\"u.some_tag\":{\"order\":0.75}}},\"type\":\"m.tag\"}]}}}}}", plain_text);
        }
    }
}
