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

using System.Text.Json.Serialization;

namespace Mocktrix.Events
{
    /// <summary>
    /// Event for membership change of a user in a room.
    /// </summary>
    public class MembershipEvent : StateEvent<MembershipEventContent>
    {
        /// <summary>
        /// The content object of the event. Type and available field differ
        /// depending on the concrete type.
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(IEvent.ContentPropertyOrder)]
        public MembershipEventContent Content { get; set; } = new();


        [JsonPropertyName("type")]
        [JsonPropertyOrder(-30)]
        public override string Type
        {
            get => "m.room.member";
            set
            {
                if (value != "m.room.member")
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be 'm.room.member'.");
                }
            }
        }
    }


    /// <summary>
    /// Event content for MembershipEvent.
    /// </summary>
    public class MembershipEventContent : IEventContent
    {
        /// <summary>
        /// The avatar URL for this user, if any.
        /// </summary>
        [JsonPropertyName("avatar_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AvatarURL { get; set; } = null;


        /// <summary>
        /// The display name for the user, if any.
        /// </summary>
        [JsonPropertyName("displayname")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DisplayName { get; set; } = null;


        /// <summary>
        /// The membership state of the user.
        /// One of: ["invite", "join", "knock", "leave", "ban"].
        /// </summary>
        [JsonPropertyName("membership")]
        public string Membership { get; set; } = null!;


        /// <summary>
        /// Returns the current membership string as an enumeration value.
        /// </summary>
        /// <returns>Returns the corresponding enumeration value, or null if
        /// Membership is an unrecognized string value.</returns>
        public Enums.Membership? MembershipAsEnum()
        {
            return Membership switch
            {
                "invite" => Enums.Membership.Invite,
                "join" => Enums.Membership.Join,
                "knock" => Enums.Membership.Knock,
                "leave" => Enums.Membership.Leave,
                "ban" => Enums.Membership.Ban,
                _ => null
            };
        }


        /// <summary>
        /// Whether this room was created with the intention to be a direct chat.
        /// </summary>
        [JsonPropertyName("is_direct")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsDirect { get; set; } = null;


        /// <summary>
        /// Will be set if this invite is an invite event and is the successor
        /// of an m.room.third_party_invite event, and absent otherwise.
        /// </summary>
        [JsonPropertyName("third_party_invite")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Invite? ThirdPartyInvite { get; set; } = null;


        [JsonPropertyName("unsigned")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public UnsignedMembershipData Unsigned { get; set; } = null!;
    }


    /// <summary>
    /// Third-party invite data.
    /// </summary>
    public class Invite
    {
        /// <summary>
        /// A name which can be displayed to represent the user instead of their
        /// third party identifier.
        /// </summary>
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = null!;


        /// <summary>
        /// A block of content which has been signed, which servers can use to
        /// verify the event. Clients should ignore this.
        /// </summary>
        [JsonPropertyName("signed")]
        public InviteSignedData Signed { get; set; } = null!;
    }


    /// <summary>
    /// Signed data.
    /// </summary>
    public class InviteSignedData
    {
        /// <summary>
        /// The invited matrix user ID. Must be equal to the user id of the event.
        /// </summary>
        [JsonPropertyName("mxid")]
        public string MatrixId { get; set; } = null!;


        /// <summary>
        /// A single signature from the verifying server, in the format
        /// specified by the Signing Events section of the server-server API.
        /// </summary>
        [JsonPropertyName("signatures")]
        public Dictionary<string, Dictionary<string, string>> Signatures { get; set; } = null!;


        /// <summary>
        /// The token property of the containing third_party_invite object.
        /// </summary>
        [JsonPropertyName("token")]
        public string Token { get; set; } = null!;
    }


    /// <summary>
    /// Unsigned informational data about the room the user was invited to.
    /// </summary>
    public class UnsignedMembershipData
    {
        /// <summary>
        /// A subset of the state of the room at the time of the invite, if
        /// membership is invite. Note that this state is informational, and
        /// should not be trusted.
        /// </summary>
        [JsonPropertyName("invite_room_state")]
        public List<IEvent> InviteRoomState { get; set; } = null!;
    }
}
