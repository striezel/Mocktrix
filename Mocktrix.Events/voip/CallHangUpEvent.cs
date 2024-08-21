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

namespace Mocktrix.Events.VoIP
{
    /// <summary>
    /// Event for end of a VoIP call in a room.
    /// </summary>
    public class CallHangUpEvent : RoomEvent
    {
        /// <summary>
        /// The content object of the event. Type and available field differ
        /// depending on the concrete type.
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(IEvent.ContentPropertyOrder)]
        public CallHangUpEventContent Content { get; set; } = new();


        [JsonPropertyName("type")]
        [JsonPropertyOrder(-30)]
        public override string Type
        {
            get => "m.call.hangup";
            set
            {
                if (value != "m.call.hangup")
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be 'm.call.hangup'.");
                }
            }
        }
    }


    /// <summary>
    /// Event content for CallHangUpEvent.
    /// </summary>
    public class CallHangUpEventContent : IEventContent
    {
        /// <summary>
        /// The ID of the call this event relates to.
        /// </summary>
        [JsonPropertyName("call_id")]
        public string CallId { get; set; } = null!;


        /// <summary>
        /// Optional error reason for the hangup. This should not be provided
        /// when the user naturally ends or rejects the call. When there was an
        /// error in the call negotiation, this should be "ice_failed" for when
        /// ICE negotiation fails or "invite_timeout" for when the other party
        /// did not answer in time. One of: ["ice_failed", "invite_timeout"].
        /// </summary>
        [JsonPropertyName("reason")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Reason { get; set; } = null;


        /// <summary>
        /// The version of the VoIP specification this message adheres to.
        /// This specification is version 0.
        /// </summary>
        [JsonPropertyName("version")]
        public long Version { get; set; } = -1;
    }
}
