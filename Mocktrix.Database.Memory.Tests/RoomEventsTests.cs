/*
    This file is part of test suite for Mocktrix.
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
using System.Security.Cryptography;

namespace Mocktrix.Database.Memory.Tests
{
    public class RoomsEventsTests
    {
        /// <summary>
        /// Provides a test event with random event id.
        /// </summary>
        /// <returns>Returns the test event.</returns>
        private static HistoryVisibilityEvent GetTestEvent()
        {
            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".AsSpan();

            return new HistoryVisibilityEvent()
            {
                Content = new HistoryVisibilityEventContent()
                {
                    HistoryVisibility = "invited"
                },
                EventId = "$test_" + RandomNumberGenerator.GetString(alphabet, 12) + ":matrix.example.org",
                OriginServerTs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                RoomId = "!test_room_01:matrix.example.org",
                Sender = "@alice:matrix.example.org"
            };
        }

        [Fact]
        public void AddEvent_Success()
        {
            var ev = GetTestEvent();

            Assert.True(RoomEvents.Add(ev));
        }

        [Fact]
        public void AddEvent_SameEventTwiceFails()
        {
            var ev = GetTestEvent();

            // First attempt is successful.
            Assert.True(RoomEvents.Add(ev));
            // But second attempt fails.
            Assert.False(RoomEvents.Add(ev));
        }


        [Fact]
        public void AddEvent_Fail()
        {
            var ev = GetTestEvent();

            ev.EventId = null!;
            Assert.False(RoomEvents.Add(ev));

            ev.EventId = "";
            Assert.False(RoomEvents.Add(ev));

            ev.EventId = "          ";
            Assert.False(RoomEvents.Add(ev));

            ev.EventId = "@wrong_sigil_character:matrix.example.com";
            Assert.False(RoomEvents.Add(ev));

            ev = GetTestEvent();

            ev.RoomId = null!;
            Assert.False(RoomEvents.Add(ev));

            ev.RoomId = "";
            Assert.False(RoomEvents.Add(ev));

            ev.RoomId = "          ";
            Assert.False(RoomEvents.Add(ev));

            ev.RoomId = "@wrong_sigil_character:matrix.example.com";
            Assert.False(RoomEvents.Add(ev));
        }

        [Fact]
        public void GetEvent_NonExistentEventNotFound()
        {
            const string event_id = "$not_here:matrix.example.org";
            var the_event = RoomEvents.GetEvent(event_id);

            // Event does not exist, function shall return null.
            Assert.Null(the_event);
        }


        [Fact]
        public void GetEvent_ExistentEvent()
        {
            // Create an event.
            var ev = GetTestEvent();
            Assert.True(RoomEvents.Add(ev));
            // Query the created event by id.
            var the_event = RoomEvents.GetEvent(ev.EventId);
            Assert.NotNull(the_event);
            // Values of created event and queried event must match.
            Assert.IsType<HistoryVisibilityEventContent>(the_event.Content);
            var typed_event_content = (HistoryVisibilityEventContent)the_event.Content;
            Assert.Equal(((HistoryVisibilityEventContent)ev.Content).HistoryVisibility, typed_event_content.HistoryVisibility);
            Assert.Equal(ev.EventId, the_event.EventId);
            Assert.Equal(ev.OriginServerTs, the_event.OriginServerTs);
            var typed_event = (HistoryVisibilityEvent)the_event;
            Assert.Null(typed_event.PrevContent);
            Assert.Equal(ev.RoomId, the_event.RoomId);
            Assert.Equal(ev.Sender, the_event.Sender);
            Assert.Equal(ev.StateKey, typed_event.StateKey);
            Assert.Equal(ev.Type, the_event.Type);
            Assert.Null(the_event.Unsigned);
            // As a special property of this implementation, both events refer
            // to the same instance.
            Assert.True(ReferenceEquals(the_event, ev));
        }

        [Fact]
        public void GetEventsOfRoom_NonExistentRoomNotFound()
        {
            const string room_id = "!not_here:matrix.example.org";
            var events = RoomEvents.GetEventsOfRoom(room_id);

            // Room does not exist, function shall return empty list.
            Assert.NotNull(events);
            Assert.Empty(events);
        }

        [Fact]
        public void GetEventsOfRoom_ExistingRoomEventsFound()
        {
            const string room_id = "!test_room_events_01:matrix.example.org";

            var event_one = GetTestEvent();
            event_one.RoomId = room_id;
            Assert.True(RoomEvents.Add(event_one));

            var event_two = GetTestEvent();
            event_two.RoomId = room_id;
            Assert.True(RoomEvents.Add(event_two));

            var events = RoomEvents.GetEventsOfRoom(room_id);

            // Room events exist, function shall return non-empty list.
            Assert.NotNull(events);
            Assert.NotEmpty(events);
            // Both events shall be in the list.
            Assert.Contains(events, e => e.EventId == event_one.EventId);
            Assert.Contains(events, e => e.EventId == event_two.EventId);
        }

        [Fact]
        public void GetEventsOfRoom_OnlyEventsOfGivenRoomAreFound()
        {
            const string room_one_id = "!test_room_events_02:matrix.example.org";
            const string room_two_id = "!test_room_events_03:matrix.example.org";

            var event_one_room_one = GetTestEvent();
            event_one_room_one.RoomId = room_one_id;
            Assert.True(RoomEvents.Add(event_one_room_one));

            var event_two_room_one = GetTestEvent();
            event_two_room_one.RoomId = room_one_id;
            Assert.True(RoomEvents.Add(event_two_room_one));

            var event_one_room_two = GetTestEvent();
            event_one_room_two.RoomId = room_two_id;
            Assert.True(RoomEvents.Add(event_one_room_two));

            var event_two_room_two = GetTestEvent();
            event_two_room_two.RoomId = room_two_id;
            Assert.True(RoomEvents.Add(event_two_room_two));

            // room one
            {
                var events = RoomEvents.GetEventsOfRoom(room_one_id);

                // Room events exist, function shall return non-empty list.
                Assert.NotNull(events);
                Assert.NotEmpty(events);
                // Both events shall be in the list.
                Assert.Contains(events, e => e.EventId == event_one_room_one.EventId);
                Assert.Contains(events, e => e.EventId == event_two_room_one.EventId);
            }

            // room two
            {
                var events = RoomEvents.GetEventsOfRoom(room_two_id);

                // Room events exist, function shall return non-empty list.
                Assert.NotNull(events);
                Assert.NotEmpty(events);
                // Both events shall be in the list.
                Assert.Contains(events, e => e.EventId == event_one_room_two.EventId);
                Assert.Contains(events, e => e.EventId == event_two_room_two.EventId);
            }
        }
    }
}
