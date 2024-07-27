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
    /// Event for invite to a VoIP call in a room.
    /// </summary>
    public class CallInviteEvent: RoomEvent
    {
        /// <summary>
        /// The content object of the event. Type and available field differ
        /// depending on the concrete type.
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(IEvent.ContentPropertyOrder)]
        public CallInviteEventContent Content { get; set; } = new();


        [JsonPropertyName("type")]
        [JsonPropertyOrder(-30)]
        public override string Type
        {
            get => "m.call.invite";
            set
            {
                if (value != "m.call.invite")
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be 'm.call.invite'.");
                }
            }
        }
    }


    /// <summary>
    /// Event content for CallInviteEvent.
    /// </summary>
    public class CallInviteEventContent : IEventContent
    {
        /// <summary>
        /// Unique identifier for the call.
        /// </summary>
        [JsonPropertyName("call_id")]
        public string CallId { get; set; } = null!;


        /// <summary>
        /// The time in milliseconds that the invite is valid for. Once the
        /// invite age exceeds this value, clients should discard it. They
        /// should also no longer show the call as awaiting an answer in the UI.
        /// </summary>
        [JsonPropertyName("lifetime")]
        public long LifeTime { get; set; } = -1;


        /// <summary>
        /// The session description object.
        /// </summary>
        [JsonPropertyName("offer")]
        public InviteOffer Offer { get; set; } = null!;


        /// <summary>
        /// The version of the VoIP specification this message adheres to.
        /// This specification is version 0.
        /// </summary>
        [JsonPropertyName("version")]
        public long Version { get; set; } = -1;
    }


    /// <summary>
    /// A session description object.
    /// </summary>
    public class InviteOffer
    {
        /// <summary>
        /// The SDP text of the session description.
        /// </summary>
        [JsonPropertyName("sdp")]
        public string SDP { get; set; } = null!;


        /// <summary>
        /// The type of session description. Must be 'offer'.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;
    }
}
