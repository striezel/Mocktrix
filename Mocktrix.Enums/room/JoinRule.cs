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

namespace Mocktrix.Enums
{
    /// <summary>
    /// Enumeration for posssible join rules of a room.
    /// </summary>
    public enum JoinRule
    {
        /// <summary>
        /// Anyone can join the room without any prerequisites.
        /// </summary>
        Public,

        /// <summary>
        /// Reserverd for future use.
        /// </summary>
        Knock,

        /// <summary>
        /// Users who wish to join have to receive an invite first.
        /// </summary>
        Invite,

        /// <summary>
        /// Reserved.
        /// </summary>
        Private
    }
}
