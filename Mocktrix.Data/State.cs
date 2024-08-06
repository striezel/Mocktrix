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

using System;
using System.Collections.Generic;

namespace Mocktrix.Data
{
    public class StateDictionaryKey : IEquatable<StateDictionaryKey>
    {
        /// <summary>
        /// The type of event. This should be namespaced similar to Java package
        /// naming conventions e.g. 'com.example.subdomain.event.type'.
        /// </summary>
        public string EventType { get; set; } = null!;


        /// <summary>
        /// A unique key which defines the overwriting semantics for this piece
        /// of room state. This value is often a zero-length string. The
        /// presence of this key makes this event a State Event. State keys
        /// starting with an "@" are reserved for referencing user IDs, such as
        /// room members.
        /// </summary>
        public string StateKey { get; set; } = null!;


        public override bool Equals(object? obj)
        {
            if (obj is StateDictionaryKey sdk)
            {
                return Equals(sdk);
            }

            return false;
        }

        public bool Equals(StateDictionaryKey? other)
        {
            if (other == null)
            {
                return false;
            }

            return other.EventType == EventType && other.StateKey == StateKey;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EventType, StateKey);
        }
    }
}
