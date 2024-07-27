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
    /// Basic interface for all event types.
    /// </summary>
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(VoIP.CallAnswerEvent), "m.call.answer")]
    [JsonDerivedType(typeof(VoIP.CallCandidatesEvent), "m.call.candidates")]
    [JsonDerivedType(typeof(VoIP.CallHangUpEvent), "m.call.hangup")]
    [JsonDerivedType(typeof(VoIP.CallInviteEvent), "m.call.invite")]
    [JsonDerivedType(typeof(CanonicalAliasEvent), "m.room.canonical_alias")]
    [JsonDerivedType(typeof(CreateRoomEvent), "m.room.create")]
    [JsonDerivedType(typeof(EncryptionEvent), "m.room.encryption")]
    [JsonDerivedType(typeof(GuestAccessEvent), "m.room.guest_access")]
    [JsonDerivedType(typeof(HistoryVisibilityEvent), "m.room.history_visibility")]
    [JsonDerivedType(typeof(JoinRulesEvent), "m.room.join_rules")]
    [JsonDerivedType(typeof(MembershipEvent), "m.room.member")]
    [JsonDerivedType(typeof(NameEvent), "m.room.name")]
    [JsonDerivedType(typeof(PinnedEventsEvent), "m.room.pinned_events")]
    [JsonDerivedType(typeof(PowerLevelsEvent), "m.room.power_levels")]
    [JsonDerivedType(typeof(TopicEvent), "m.room.topic")]
    public interface IEvent
    {
        /// <summary>
        /// The JsonPropertyOrder value for the Content object in events.
        /// </summary>
        public const int ContentPropertyOrder = -100;


        /// <summary>
        /// The type of event. This should be namespaced similar to Java package
        /// naming conventions e.g. 'com.example.subdomain.event.type'.
        /// </summary>
        [JsonPropertyName("type")]
        [JsonPropertyOrder(-30)]
        public string Type { get; set; }


        /// <summary>
        /// Indicates whether this event is a state event.
        /// </summary>
        /// <returns>Returns true, if the event is a state event.
        /// Returns false otherwise.</returns>
        public bool IsStateEvent();
    }
}
