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
using System.Text.Json.Serialization;

namespace Mocktrix.Protocol.Types.Rooms
{
    /// <summary>
    /// Data sent by a client via POST request to create a new room.
    /// </summary>
    public class RoomCreationData
    {
        /// <summary>
        /// A public visibility indicates that the room will be shown in the
        /// published room list. A private visibility will hide the room from
        /// the published room list. Rooms default to private visibility, if
        /// this key is not included.
        /// 
        /// One of ["private", "public"].
        /// </summary>
        [JsonPropertyName("visibility")]
        public string? Visibility { get; set; } = null;


        /// <summary>
        /// Checks whether public room visibility is set.
        /// </summary>
        /// <returns>Returns true, if visibility is public.</returns>
        public bool IsPublic()
        {
            return Visibility == "public";
        }


        /// <summary>
        /// The desired room alias local part. If this is included, a room alias
        /// will be created and mapped to the newly created room. The alias will
        /// belong on the same homeserver which created the room. For example,
        /// if this was set to "foo" and sent to the homeserver "example.com",
        /// the complete room alias would be #foo:example.com.
        /// </summary>
        [JsonPropertyName("room_alias_name")]
        public string? RoomAliasName { get; set; } = null;


        /// <summary>
        /// If this is set, an m.room.name event will be sent into the room
        /// to indicate the name of the room.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; } = null;


        /// <summary>
        /// If this is included, an m.room.topic event will be sent into the
        /// room to indicate the topic of the room.
        /// </summary>
        [JsonPropertyName("topic")]
        public string? Topic { get; set; } = null;


        /// <summary>
        /// A list of Matrix user ids to invite to the room.
        /// </summary>
        [JsonPropertyName("invite")]
        public List<string>? Invite { get; set; } = null;


        /// <summary>
        /// A list of objects representing third party IDs to invite into the room.
        /// </summary>
        [JsonPropertyName("invite_3pid")]
        public List<Invite3Pid>? ThirdPartyInvites { get; set; } = null;


        /// <summary>
        /// The room version to set for the room.
        /// 
        /// If not provided, the homeserver is to use its configured default.
        /// If provided, the homeserver will return a 400 error with the errcode
        /// M_UNSUPPORTED_ROOM_VERSION if it does not support the room version.
        /// </summary>
        [JsonPropertyName("room_version")]
        public string? RoomVersion { get; set; } = null;


        /// <summary>
        /// Extra keys, such as m.federate, to be added to the content of the
        /// m.room.create event. The server will clobber the following keys:
        /// creator, room_version.
        /// 
        /// Future versions of the specification may allow the server to
        /// clobber other keys.
        /// </summary>
        [JsonPropertyName("creation_content")]
        public CreateRoomEventContent? CreationContent { get; set; } = null;


        /// <summary>
        /// A list of state events to set in the new room.
        /// 
        /// This allows the user to override the default state events set in
        /// the new room. The expected format of the state events are an object
        /// with type, state_key and content keys set.
        /// 
        /// Takes precedence over events set by preset, but gets overriden by
        /// name and topic keys.
        /// </summary>
        [JsonPropertyName("initial_state")]
        public List<IEvent>? InitialState { get; set; } = null;


        /// <summary>
        /// Convenience parameter for setting various default state events
        /// based on a preset.
        /// 
        /// If unspecified, the server should use the visibility to determine
        /// which preset to use. A visibility of "public" equates to a preset
        /// of "public_chat" and "private" visibility equates to a preset of
        /// "private_chat".
        /// 
        /// One of: ["private_chat", "public_chat", "trusted_private_chat"].
        /// </summary>
        [JsonPropertyName("preset")]
        public string? Preset { get; set; } = null;


        /// <summary>
        /// This flag makes the server set the is_direct flag on the
        /// m.room.member events sent to the users in invite and invite_3pid.
        /// </summary>
        [JsonPropertyName("is_direct")]
        public bool? IsDirect { get; set; } = null;


        /// <summary>
        /// The power level content to override in the default power level event.
        /// This object is applied on top of the generated m.room.power_levels
        /// event content prior to it being sent to the room.
        /// 
        /// Defaults to overriding nothing.
        /// </summary>
        [JsonPropertyName("power_level_content_override")]
        public PowerLevelsEventContent? PowerLevelContentOverride { get; set; } = null;
    }


    /// <summary>
    /// Contains data for third-party invites.
    /// </summary>
    public class Invite3Pid
    {
        /// <summary>
        /// The hostname+port of the identity server which should be used for
        /// third party identifier lookups.
        /// </summary>
        [JsonPropertyName("id_server")]
        public string IdentityServer { get; set; } = null!;


        /// <summary>
        /// An access token previously registered with the identity server.
        /// Servers can treat this as optional to distinguish between
        /// r0.5-compatible clients and this specification version.
        /// </summary>
        [JsonPropertyName("id_access_token")]
        public string? IdAccessToken { get; set; } = null;


        /// <summary>
        /// The kind of address being passed in the address field, e.g. "email".
        /// </summary>
        [JsonPropertyName("medium")]
        public string Medium { get; set; } = null!;


        /// <summary>
        /// The invitee's third party identifier.
        /// </summary>
        [JsonPropertyName("address")]
        public string Address { get; set; } = null!;
    }
}
