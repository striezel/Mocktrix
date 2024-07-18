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
    /// Enumeration type for possible room membership values.
    /// </summary>
    public enum Membership
    {
        /// <summary>
        /// User was invited to a room, but has not joined yet.
        /// User may not participate in the room until they joined.
        /// </summary>
        Invite,

        /// <summary>
        /// User joined the room and may participate in it.
        /// </summary>
        Join,

        /// <summary>
        /// Reserved, in r0.6.1 this is not implemented.
        /// </summary>
        Knock,

        /// <summary>
        /// User was once joined to the room, but has left or was kicked.
        /// </summary>
        Leave,

        /// <summary>
        /// User has been banned from the room and is not allowed to join.
        /// </summary>
        Ban
    }
}
