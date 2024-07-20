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
    /// Event for visibility of the history of a room.
    /// </summary>
    public class HistoryVisibilityEvent: StateEventZeroLengthKey<HistoryVisibilityEventContent>
    {
        /// <summary>
        /// The content object of the event. Type and available field differ
        /// depending on the concrete type.
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(IEvent.ContentPropertyOrder)]
        public HistoryVisibilityEventContent Content { get; set; } = new();


        [JsonPropertyName("type")]
        [JsonPropertyOrder(-30)]
        public override string Type
        {
            get => "m.room.history_visibility";
            set
            {
                if (value != "m.room.history_visibility")
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be 'm.room.history_visibility'.");
                }
            }
        }
    }


    /// <summary>
    /// Event content for HistoryVisibilityEvent.
    /// </summary>
    public class HistoryVisibilityEventContent : IEventContent
    {
        /// <summary>
        /// Who can see the room history. One of: ["invited", "joined", "shared", "world_readable"].
        /// </summary>
        [JsonPropertyName("history_visibility")]
        public string HistoryVisibility { get; set; } = null!;


        /// <summary>
        /// Returns the current history visibility string as an enumeration value.
        /// </summary>
        /// <returns>Returns the corresponding enumeration value, or null if
        /// HistoryVisibility is an unrecognized string value.</returns>
        public Enums.HistoryVisibility? ToEnum()
        {
            return HistoryVisibility switch
            {
                "invited" => Enums.HistoryVisibility.Invited,
                "joined" => Enums.HistoryVisibility.Joined,
                "shared" => Enums.HistoryVisibility.Shared,
                "world_readable" => Enums.HistoryVisibility.WorldReadable,
                _ => null
            };
        }
    }
}
