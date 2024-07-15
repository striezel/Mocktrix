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
    /// Event for power levels of a room.
    /// </summary>
    public class PowerLevelsEvent: GenericStateEventZeroLengthKey<PowerLevelsEventContent>
    {
        [JsonPropertyName("type")]
        [JsonPropertyOrder(-30)]
        public override string Type
        {
            get => "m.room.power_levels";
            set
            {
                if (value != "m.room.power_levels")
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be 'm.room.power_levels'.");
                }
            }
        }
    }


    /// <summary>
    /// Event content for PowerLevelsEvent.
    /// </summary>
    public class PowerLevelsEventContent : IEventContent
    {
        /// <summary>
        /// The level required to ban a user. Defaults to 50 if unspecified.
        /// </summary>
        [JsonPropertyName("ban")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Ban { get; set; } = null;


        /// <summary>
        /// The level required to send specific event types. This is a mapping
        /// from event type to power level required.
        /// </summary>
        [JsonPropertyName("events")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SortedDictionary<string, int>? Events { get; set; } = null;


        /// <summary>
        /// The default level required to send message events.
        /// Can be overridden by the events key. Defaults to 0 if unspecified.
        /// </summary>
        [JsonPropertyName("events_default")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? EventsDefault { get; set; } = null;


        /// <summary>
        /// The level required to invite a user. Defaults to 50 if unspecified.
        /// </summary>
        [JsonPropertyName("invite")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Invite { get; set; } = null;


        /// <summary>
        /// The level required to kick a user. Defaults to 50 if unspecified.
        /// </summary>
        [JsonPropertyName("kick")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Kick { get; set; } = null;


        /// <summary>
        /// The power level requirements for specific notification types. This
        /// is a mapping from key to power level for that notifications key.
        /// </summary>
        /// <remarks>Currently, the specification lists only one key: "rooms".
        /// This is the level required to trigger an @room notification. It
        /// defaults to 50 if unspecified.</remarks>
        [JsonPropertyName("notifications")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SortedDictionary<string, int>? Notifications { get; set; } = null;


        /// <summary>
        /// The level required to redact an event. Defaults to 50 if unspecified.
        /// </summary>
        [JsonPropertyName("redact")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Redact { get; set; } = null;


        /// <summary>
        /// The default level required to send state events.
        /// Can be overridden by the events key. Defaults to 50 if unspecified.
        /// </summary>
        [JsonPropertyName("state_default")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? StateDefault { get; set; } = null;


        /// <summary>
        /// The power levels for specific users. This is a mapping from user id
        /// to power level for that user.
        /// </summary>
        [JsonPropertyName("users")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SortedDictionary<string, int>? Users { get; set; } = null;


        /// <summary>
        /// The default power level for every user in the room, unless their
        /// user id is mentioned in the users key. Defaults to 0 if unspecified.
        /// </summary>
        [JsonPropertyName("users_default")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? UsersDefault { get; set; } = null;
    }
}
