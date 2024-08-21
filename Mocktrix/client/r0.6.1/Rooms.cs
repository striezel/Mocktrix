/*
    This file is part of Mocktrix.
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

using Mocktrix.Events;
using Mocktrix.Protocol.Types;
using Mocktrix.Protocol.Types.Rooms;

namespace Mocktrix.client.r0_6_1
{
    /// <summary>
    /// Contains implementation for room-related endpoints of protocol version r0.6.1
    /// </summary>
    public static class Rooms
    {
        // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-joined-rooms,
        // i.e. the endpoint to get information about joined rooms of a
        // user.
        private static IResult GetJoinedRooms(HttpContext context)
        {
            var access_token = Utilities.GetAccessToken(context);
            if (string.IsNullOrWhiteSpace(access_token))
            {
                var error = new ErrorResponse
                {
                    errcode = "M_MISSING_TOKEN",
                    error = "Missing access token."
                };
                return Results.Json(error, statusCode: StatusCodes.Status401Unauthorized);
            }
            var token = Database.Memory.AccessTokens.Find(access_token);
            if (token == null)
            {
                var error = new ErrorResponse
                {
                    errcode = "M_UNKNOWN_TOKEN",
                    error = "Unrecognized access token."
                };
                return Results.Json(error, statusCode: StatusCodes.Status401Unauthorized);
            }

            // Token was found, so get joined rooms.
            var joined_rooms = Database.Memory.RoomMemberships
                              .GetAllMembershipsOfUser(token.user_id)
                              .FindAll(e => e.Membership == Enums.Membership.Join);
            var result = new
            {
                joined_rooms = new List<string>(joined_rooms.Count)
            };
            foreach (var element in joined_rooms)
            {
                result.joined_rooms.Add(element.RoomId);
            }
            return Results.Ok(result);
        }

        // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-directory-list-room-roomid,
        // i.e. the endpoint to get a room's visibility.
        private static IResult GetRoomVisibility(string roomId, HttpContext context)
        {
            var room = Database.Memory.Rooms.GetRoom(roomId);
            if (room == null)
            {
                return Results.NotFound(new ErrorResponse
                {
                    errcode = "M_NOT_FOUND",
                    error = "The requested room was not found."
                });
            }

            return Results.Json(new
            {
                visibility = room.Public ? "public" : "private"
            });
        }


        /// <summary>
        /// Adds room-related endpoints to the web application.
        /// </summary>
        /// <param name="app">the app to which the endpoint shall be added</param>
        public static void AddEndpoints(WebApplication app)
        {
            // Add https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-joined-rooms,
            // i.e. the endpoint to get information about joined rooms of a
            // user.
            app.MapGet("/_matrix/client/r0/joined_rooms", GetJoinedRooms);

            // Implement https://spec.matrix.org/historical/client_server/r0.6.1.html#post-matrix-client-r0-createroom,
            // i.e. the endpoint to create a new room.
            app.MapPost("/_matrix/client/r0/createRoom", async (HttpContext context) =>
            {
                var access_token = Utilities.GetAccessToken(context);
                if (string.IsNullOrWhiteSpace(access_token))
                {
                    var error = new ErrorResponse
                    {
                        errcode = "M_MISSING_TOKEN",
                        error = "Missing access token."
                    };
                    return Results.Json(error, statusCode: StatusCodes.Status401Unauthorized);
                }
                var token = Database.Memory.AccessTokens.Find(access_token);
                if (token == null)
                {
                    var error = new ErrorResponse
                    {
                        errcode = "M_UNKNOWN_TOKEN",
                        error = "Unrecognized access token."
                    };
                    return Results.Json(error, statusCode: StatusCodes.Status401Unauthorized);
                }

                RoomCreationData? data;
                try
                {
                    data = await context.Request.ReadFromJsonAsync<RoomCreationData>();
                }
                catch (Exception)
                {
                    data = null;
                }
                if (data == null)
                {
                    return Results.BadRequest(new ErrorResponse
                    {
                        errcode = "M_NOT_JSON",
                        error = "The request does not contain JSON or contains invalid JSON."
                    });
                }

                // Check room version.
                string version = string.IsNullOrWhiteSpace(data.RoomVersion) ? "1" : data.RoomVersion;
                if (!RoomVersions.Support.IsSupportedVersion(version))
                {
                    return Results.BadRequest(new ErrorResponse
                    {
                        errcode = "M_UNSUPPORTED_ROOM_VERSION",
                        error = "The given room version is not supported."
                    });
                }

                Uri server = new(app.Urls.FirstOrDefault("http://localhost"));
                if (!string.IsNullOrWhiteSpace(data.RoomAliasName))
                {
                    if (data.RoomAliasName.Contains(':'))
                    {
                        return Results.BadRequest(new ErrorResponse
                        {
                            errcode = "M_UNKNOWN",
                            error = "The ':' character is not allowed in the room alias."
                                  + " This endpoint only expects the localpart "
                                  + "of the alias and not the fully-qualified alias,"
                                  + " e.g. 'foo' instead of '#foo:example.org'."
                        });
                    }
                    bool contains_invalid_char = data.RoomAliasName.IndexOfAny([' ', '\t', '\n', '\r']) != -1;
                    if (contains_invalid_char)
                    {
                        return Results.BadRequest(new ErrorResponse
                        {
                            errcode = "M_UNKNOWN",
                            error = "The requested alias contains invalid characters."
                        });
                    }
                    string full = "#" + data.RoomAliasName + ":" + server.Host;
                    if (full.Length > 255)
                    {
                        return Results.BadRequest(new ErrorResponse
                        {
                            errcode = "M_UNKNOWN",
                            error = "The requested alias is too long."
                        });
                    }
                }

                var create_content = data.CreationContent ?? new CreateRoomEventContent();
                create_content.Creator = token.user_id;
                create_content.Version = version;

                // "Hacky" room id generation.
                // TODO: Implement separate function that can generate room ids.
                var room_id = Id.Generate(server).Replace('$', '!');

                var room = Database.Memory.Rooms.Create(room_id, token.user_id, version, data.IsPublic());

                var create_event = new CreateRoomEvent()
                {
                    Content = create_content,
                    EventId = Id.Generate(server),
                    OriginServerTs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    RoomId = room_id,
                    Sender = token.user_id,
                    StateKey = ""
                };
                Database.Memory.RoomEvents.Add(create_event);
                var state = Database.Memory.RoomStates.Create(room_id, []);
                state.State.Add(new Data.StateDictionaryKey()
                {
                    EventType = create_event.Type,
                    StateKey = create_event.StateKey
                }, create_event.EventId);

                var user = Database.Memory.Users.GetUser(token.user_id);
                var member_event = new MembershipEvent()
                {
                    Content = new MembershipEventContent()
                    {
                        AvatarURL = user?.avatar_url,
                        DisplayName = user?.display_name,
                        IsDirect = data.IsDirect,
                        Membership = "join"
                    },
                    EventId = Id.Generate(server),
                    OriginServerTs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    RoomId = room_id,
                    Sender = token.user_id,
                    StateKey = token.user_id
                };
                Database.Memory.RoomEvents.Add(member_event);
                _ = Database.Memory.RoomMemberships.Create(room_id, token.user_id, Enums.Membership.Join);
                state.State.Add(new Data.StateDictionaryKey()
                {
                    EventType = member_event.Type,
                    StateKey = member_event.StateKey
                }, member_event.EventId);

                var power_levels_event = new PowerLevelsEvent()
                {
                    Content = data.PowerLevelContentOverride ?? new PowerLevelsEventContent()
                    {
                        Ban = 50,
                        EventsDefault = 0,
                        Invite = 50,
                        Kick = 50,
                        Redact = 50,
                        StateDefault = 50,
                        Users = new SortedDictionary<string, long>()
                        {
                            { token.user_id, 100 }
                        },
                        UsersDefault = 0
                    },
                    EventId = Id.Generate(server),
                    OriginServerTs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    RoomId = room_id,
                    Sender = token.user_id,
                    StateKey = string.Empty
                };
                Database.Memory.RoomEvents.Add(power_levels_event);
                state.State.Add(new Data.StateDictionaryKey()
                {
                    EventType = power_levels_event.Type,
                    StateKey = power_levels_event.StateKey
                }, power_levels_event.EventId);

                string real_preset = data.Preset ?? (data.IsPublic() ? "public_chat" : "private_chat");
                var join_rules = new JoinRulesEvent()
                {
                    Content = new JoinRulesEventContent()
                    {
                        JoinRule = real_preset switch
                        {
                            "public_chat" => "public",
                            _ => "invite"
                        }
                    },
                    EventId = Id.Generate(server),
                    OriginServerTs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    RoomId = room_id,
                    Sender = token.user_id,
                    StateKey = string.Empty
                };
                if (data.InitialState != null)
                {
                    int idx = data.InitialState.FindIndex(e => e.Type == "m.room.join_rules");
                    if (idx != -1)
                    {
                        var ev = (JoinRulesEvent)data.InitialState[idx];
                        join_rules.Content = ev.Content;
                    }
                }
                Database.Memory.RoomEvents.Add(join_rules);
                room.JoinRule = join_rules.Content.ToEnum();
                state.State.Add(new Data.StateDictionaryKey()
                {
                    EventType = join_rules.Type,
                    StateKey = join_rules.StateKey
                }, join_rules.EventId);

                var history_visibility = new HistoryVisibilityEvent()
                {
                    Content = new HistoryVisibilityEventContent()
                    {
                        HistoryVisibility = "shared"
                    },
                    EventId = Id.Generate(server),
                    OriginServerTs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    RoomId = room_id,
                    Sender = token.user_id,
                    StateKey = string.Empty
                };
                if (data.InitialState != null)
                {
                    int idx = data.InitialState.FindIndex(e => e.Type == "m.room.history_visibility");
                    if (idx != -1)
                    {
                        var ev = (HistoryVisibilityEvent)data.InitialState[idx];
                        history_visibility.Content = ev.Content;
                    }
                }
                Database.Memory.RoomEvents.Add(history_visibility);
                room.HistoryVisibility = history_visibility.Content.ToEnum();
                state.State.Add(new Data.StateDictionaryKey()
                {
                    EventType = history_visibility.Type,
                    StateKey = history_visibility.StateKey
                }, history_visibility.EventId);

                var guest_access = new GuestAccessEvent()
                {
                    Content = new GuestAccessEventContent()
                    {
                        GuestAccess = real_preset switch
                        {
                            "public_chat" => "forbidden",
                            _ => "can_join"
                        }
                    },
                    EventId = Id.Generate(server),
                    OriginServerTs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    RoomId = room_id,
                    Sender = token.user_id,
                    StateKey = string.Empty
                };
                if (data.InitialState != null)
                {
                    int idx = data.InitialState.FindIndex(e => e.Type == "m.room.guest_access");
                    if (idx != -1)
                    {
                        var ev = (GuestAccessEvent)data.InitialState[idx];
                        guest_access.Content = ev.Content;
                    }
                }
                Database.Memory.RoomEvents.Add(guest_access);
                room.GuestAccess = guest_access.Content.ToEnum();
                state.State.Add(new Data.StateDictionaryKey()
                {
                    EventType = guest_access.Type,
                    StateKey = guest_access.StateKey
                }, guest_access.EventId);

                if (!string.IsNullOrWhiteSpace(data.RoomAliasName))
                {
                    string full_alias = "#" + data.RoomAliasName + ":" + server.Host;
                    var alias_event = new CanonicalAliasEvent()
                    {
                        Content = new CanonicalAliasEventContent()
                        {
                            Alias = full_alias
                        },
                        EventId = Id.Generate(server),
                        OriginServerTs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        RoomId = room_id,
                        Sender = token.user_id,
                        StateKey = string.Empty
                    };
                    Database.Memory.RoomEvents.Add(alias_event);
                    Database.Memory.RoomAliases.Create(room_id, full_alias, token.user_id);
                    state.State.Add(new Data.StateDictionaryKey()
                    {
                        EventType = alias_event.Type,
                        StateKey = alias_event.StateKey
                    }, alias_event.EventId);
                }

                // Add initial state events.
                foreach (var element in data.InitialState ?? [])
                {
                    if (!element.IsStateEvent() || element.Type == "m.room.create"
                        || element.Type == "m.room.member"
                        || element.Type == "m.room.power_levels"
                        || element.Type == "m.room.join_rules"
                        || element.Type == "m.room.history_visibility"
                        || element.Type == "m.room.guest_access"
                        || (element.Type == "m.room.name" && string.IsNullOrWhiteSpace(data.Name))
                        || (element.Type == "m.room.topic" && string.IsNullOrWhiteSpace(data.Topic))
                        || element.Type == "m.room.guest_access")
                    {
                        continue;
                    }

                    BasicStateEvent ev = (BasicStateEvent)element;
                    ev.EventId = Id.Generate(server);
                    ev.OriginServerTs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    ev.RoomId = room_id;
                    ev.Sender = token.user_id;
                    Database.Memory.RoomEvents.Add(ev);
                    state.State.Add(new Data.StateDictionaryKey()
                    {
                        EventType = ev.Type,
                        StateKey = ev.StateKey
                    }, ev.EventId);
                }

                if (!string.IsNullOrWhiteSpace(data.Name))
                {
                    var name_event = new NameEvent()
                    {
                        Content = new NameEventContent()
                        {
                            Name = data.Name
                        },
                        EventId = Id.Generate(server),
                        OriginServerTs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        RoomId = room_id,
                        Sender = token.user_id,
                        StateKey = string.Empty
                    };
                    Database.Memory.RoomEvents.Add(name_event);
                    room.Name = data.Name;
                    state.State.Add(new Data.StateDictionaryKey()
                    {
                        EventType = name_event.Type,
                        StateKey = name_event.StateKey
                    }, name_event.EventId);
                }

                if (!string.IsNullOrWhiteSpace(data.Topic))
                {
                    var topic_event = new TopicEvent()
                    {
                        Content = new TopicEventContent()
                        {
                            Topic = data.Topic
                        },
                        EventId = Id.Generate(server),
                        OriginServerTs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        RoomId = room_id,
                        Sender = token.user_id,
                        StateKey = string.Empty
                    };
                    Database.Memory.RoomEvents.Add(topic_event);
                    room.Topic = data.Topic;
                    state.State.Add(new Data.StateDictionaryKey()
                    {
                        EventType = topic_event.Type,
                        StateKey = topic_event.StateKey
                    }, topic_event.EventId);
                }

                // Add invite events.
                foreach (var user_id in data.Invite ?? [])
                {
                    var invitee = Database.Memory.Users.GetUser(user_id);
                    var invite_event = new MembershipEvent()
                    {
                        Content = new MembershipEventContent()
                        {
                            AvatarURL = invitee?.avatar_url,
                            DisplayName = invitee?.display_name,
                            IsDirect = data.IsDirect,
                            Membership = "invite"
                        },
                        EventId = Id.Generate(server),
                        OriginServerTs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        RoomId = room_id,
                        Sender = token.user_id,
                        StateKey = user_id
                    };
                    Database.Memory.RoomEvents.Add(invite_event);
                    _ = Database.Memory.RoomMemberships.Create(room_id, user_id, Enums.Membership.Invite);
                    state.State.Add(new Data.StateDictionaryKey()
                    {
                        EventType = invite_event.Type,
                        StateKey = invite_event.StateKey
                    }, invite_event.EventId);
                }

                // TODO: Handle or outright deny third party invites. Currently
                // 3pids are not supported by the server.

                // TODO: Implement state resolution + event authorization rules
                // and check them.

                return Results.Ok(new { room_id });
            });


            // Add https://spec.matrix.org/historical/client_server/r0.6.1.html#get-matrix-client-r0-directory-list-room-roomid,
            // i.e. the endpoint to get a room's visibility.
            app.MapGet("/_matrix/client/r0/directory/list/room/{roomId}", GetRoomVisibility);
        }
    }
}
