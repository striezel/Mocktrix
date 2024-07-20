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

using Mocktrix.Events;

namespace Mocktrix.Database.Memory
{
    /// <summary>
    /// In-memory implementation of room event database.
    /// </summary>
    public static class RoomEvents
    {
        /// <summary>
        /// in-memory event list
        /// </summary>
        private static readonly List<RoomEvent> events = [];


        /// <summary>
        /// Checks whether an event can be added to avoid some very basic mistakes.
        /// </summary>
        /// <param name="ev">the event to add (potentially)</param>
        /// <returns>Returns true, if the event can be added.
        /// Returns false otherwise.</returns>
        private static bool AllowAdd(RoomEvent ev)
        {
            if (string.IsNullOrWhiteSpace(ev.EventId) || !ev.EventId.StartsWith('$')
                || string.IsNullOrWhiteSpace(ev.RoomId) || !ev.RoomId.StartsWith('!')
                || (events.FindIndex(e => e.EventId == ev.EventId) != -1))
            {
                return false;
            }

            var property = ev.GetType().GetProperty("Content");
            return property == null || property.GetValue(ev) != null;
        }


        /// <summary>
        /// Adds an event.
        /// </summary>
        /// <param name="the_event">the populated room event to add</param>
        /// <returns>Returns whether the event was added.</returns>
        public static bool Add(RoomEvent the_event)
        {
            if (!AllowAdd(the_event))
            {
                return false;
            }

            events.Add(the_event);
            return true;
        }


        /// <summary>
        /// Gets an existing event.
        /// </summary>
        /// <param name="event_id">id of the event</param>
        /// <returns>Returns the event with the matching id, if it exists.
        /// Returns null, if no match was found.</returns>
        public static RoomEvent? GetEvent(string event_id)
        {
            return events.Find(e => e.EventId == event_id);
        }


        /// <summary>
        /// Gets an existing event.
        /// </summary>
        /// <param name="room_id">id of the room, e.g. "!some_room:matrix.example.org"</param>
        /// <returns>Returns all events with the matching room id, if any exist.
        /// Returns an empty list otherwise.</returns>
        public static List<RoomEvent> GetEventsOfRoom(string room_id)
        {
            return events.FindAll(e => e.RoomId == room_id);
        }
    }
}
