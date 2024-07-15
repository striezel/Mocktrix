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
    [JsonDerivedType(typeof(CanonicalAliasEvent))]
    [JsonDerivedType(typeof(CreateRoomEvent))]
    [JsonDerivedType(typeof(JoinRulesEvent))]
    [JsonDerivedType(typeof(MembershipEvent))]
    [JsonDerivedType(typeof(PowerLevelsEvent))]
    public interface IEvent
    {
        /// <summary>
        /// The content object of the event. Type and available field differ
        /// depending on the concrete type.
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(-100)]
        public IEventContent Content { get; set; }


        /// <summary>
        /// The type of event. This should be namespaced similar to Java package
        /// naming conventions e.g. 'com.example.subdomain.event.type'.
        /// </summary>
        [JsonPropertyName("type")]
        [JsonPropertyOrder(-30)]
        public string Type { get; set; }
    }
}
