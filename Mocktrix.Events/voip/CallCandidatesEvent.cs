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
    /// Event for candidates of a call in a room.
    /// </summary>
    public class CallCandidatesEvent : RoomEvent
    {
        /// <summary>
        /// The content object of the event. Type and available field differ
        /// depending on the concrete type.
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(IEvent.ContentPropertyOrder)]
        public CallCandidatesEventContent Content { get; set; } = new();


        [JsonPropertyName("type")]
        [JsonPropertyOrder(-30)]
        public override string Type
        {
            get => "m.call.candidates";
            set
            {
                if (value != "m.call.candidates")
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be 'm.call.candidates'.");
                }
            }
        }
    }


    /// <summary>
    /// Event content for CallCandidatesEvent.
    /// </summary>
    public class CallCandidatesEventContent : IEventContent
    {
        /// <summary>
        /// The ID of the call this event relates to.
        /// </summary>
        [JsonPropertyName("call_id")]
        public string CallId { get; set; } = null!;


        /// <summary>
        /// List of objects describing the candidates.
        /// </summary>
        [JsonPropertyName("candidates")]
        public List<CallCandidate> Candidates { get; set; } = null!;


        /// <summary>
        /// The version of the VoIP specification this message adheres to.
        /// This specification is version 0.
        /// </summary>
        [JsonPropertyName("version")]
        public long Version { get; set; } = -1;
    }


    /// <summary>
    /// A candidate for a VoIP call.
    /// </summary>
    public class CallCandidate
    {
        /// <summary>
        /// The SDP 'a' line of the candidate.
        /// </summary>
        [JsonPropertyName("candidate")]
        public string Candidate { get; set; } = null!;


        /// <summary>
        /// The index of the SDP 'm' line this candidate is intended for.
        /// </summary>
        [JsonPropertyName("sdpMLineIndex")]
        public long SdpMLineIndex { get; set; } = -1;


        /// <summary>
        /// The SDP media type this candidate is intended for.
        /// </summary>
        [JsonPropertyName("sdpMid")]
        public string SdpMid { get; set; } = null!;
    }
}
