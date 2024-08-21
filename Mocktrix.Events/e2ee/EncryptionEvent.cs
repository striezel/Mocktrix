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
    /// Event for encryption settings of a room.
    /// </summary>
    public class EncryptionEvent : StateEventZeroLengthKey<EncryptionEventContent>
    {
        /// <summary>
        /// The content object of the event. Type and available field differ
        /// depending on the concrete type.
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(IEvent.ContentPropertyOrder)]
        public EncryptionEventContent Content { get; set; } = new();


        [JsonPropertyName("type")]
        [JsonPropertyOrder(-30)]
        public override string Type
        {
            get => "m.room.encryption";
            set
            {
                if (value != "m.room.encryption")
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be 'm.room.encryption'.");
                }
            }
        }
    }


    /// <summary>
    /// Event content for EncryptionEvent.
    /// </summary>
    public class EncryptionEventContent : IEventContent
    {
        /// <summary>
        /// The encryption algorithm to be used to encrypt messages sent in
        /// this room. Must be 'm.megolm.v1.aes-sha2'.
        /// </summary>
        [JsonPropertyName("algorithm")]
        public string Algorithm { get; set; } = null!;

        /// <summary>
        /// How long the session should be used before changing it.
        /// 604800000 (a week) is the recommended default.
        /// </summary>
        [JsonPropertyName("rotation_period_ms")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? RotationPeriodMilliseconds { get; set; } = null;

        /// <summary>
        /// How many messages should be sent before changing the session.
        /// 100 is the recommended default.
        /// </summary>
        [JsonPropertyName("rotation_period_msgs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? RotationPeriodMessages { get; set; } = null;
    }
}
