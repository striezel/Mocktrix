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
    /// Enumeration type for history visibility of a room.
    /// </summary>
    public enum HistoryVisibility
    {
        /// <summary>
        /// History is visible to joined members from the point they were
        /// invited onwards. Events stop being accessible when the member's
        /// membership state changes to something other than "invite" or "join".
        /// </summary>
        Invited,

        /// <summary>
        /// History is visible to joined members from the point they joined the
        /// room onwards. Events stop being accessible when the member's
        /// membership state changes to something other than "join".
        /// </summary>
        Joined,

        /// <summary>
        /// Previous history is always visible to joined members. All events in
        /// the room are accessible, even those sent when the member was not a
        /// part of the room.
        /// </summary>
        Shared,


        /// <summary>
        /// All history may be shared by any participating homeserver with
        /// anyone, regardless of whether they have ever joined the room.
        /// </summary>
        WorldReadable
    }
}
