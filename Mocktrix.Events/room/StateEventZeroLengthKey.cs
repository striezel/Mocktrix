﻿/*
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

namespace Mocktrix.Events
{
    /// <summary>
    /// Specialization of StateEvent for empty state keys.
    /// </summary>
    /// <typeparam name="C">type of the event content, must be derived from
    /// IEventContent</typeparam>
    public abstract class StateEventZeroLengthKey<C> : StateEvent<C>
        where C : IEventContent, new()
    {
        /// <summary>
        /// Sets the state key to an empty string.
        /// </summary>
        public StateEventZeroLengthKey()
        {
            StateKey = string.Empty;
        }
    }
}
