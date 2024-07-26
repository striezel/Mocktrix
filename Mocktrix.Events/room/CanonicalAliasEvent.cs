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
    /// Event to set the canonical alias of a room.
    /// </summary>
    public class CanonicalAliasEvent : StateEventZeroLengthKey<CanonicalAliasEventContent>
    {
        /// <summary>
        /// The content object of the event. Type and available field differ
        /// depending on the concrete type.
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(IEvent.ContentPropertyOrder)]
        public CanonicalAliasEventContent Content { get; set; } = new();


        [JsonPropertyName("type")]
        [JsonPropertyOrder(-30)]
        public override string Type
        {
            get => "m.room.canonical_alias";
            set
            {
                if (value != "m.room.canonical_alias")
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be 'm.room.canonical_alias'.");
                }
            }
        }
    }


    /// <summary>
    /// Event content for CanonicalAliasEvent.
    /// </summary>
    public class CanonicalAliasEventContent : IEventContent
    {
        /// <summary>
        /// The canonical alias for the room, e.g. "#nice_room:matrix.example.org".
        /// </summary>
        [JsonPropertyName("alias")]
        public string? Alias { get; set; }


        /// <summary>
        /// Checks whether this content contains a canonical alias.
        /// </summary>
        /// <returns></returns>
        public bool HasCanonicalAlias()
        {
            return !string.IsNullOrWhiteSpace(Alias);
        }


        /// <summary>
        /// Alternative aliases for the room. This list can have entries even if
        /// the canonical alias is not present.
        /// </summary>
        [JsonPropertyName("alt_aliases")]
        public List<string>? AlternativeAliases { get; set; }
    }
}
