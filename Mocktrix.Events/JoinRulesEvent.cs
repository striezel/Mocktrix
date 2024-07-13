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
    /// Event for join rules of a room.
    /// </summary>
    public class JoinRulesEvent: GenericStateEventZeroLengthKey<JoinRulesEventContent>
    {
        [JsonPropertyName("type")]
        [JsonPropertyOrder(-30)]
        public override string Type
        {
            get => "m.room.join_rules";
            set
            {
                if (value != "m.room.join_rules")
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be 'm.room.join_rules'.");
                }
            }
        }
    }


    /// <summary>
    /// Event content for JoinRulesEvent.
    /// </summary>
    public class JoinRulesEventContent : IEventContent
    {
        /// <summary>
        /// The type of rules used for users wishing to join this room. One of: ["public", "knock", "invite", "private"].
        /// </summary>
        [JsonPropertyName("join_rule")]
        public string JoinRule { get; set; } = null!;
    }
}
