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

namespace Mocktrix.Data.Tests
{
    public class StateDictionaryKeyTests
    {
        [Fact]
        public void Constructor()
        {
            StateDictionaryKey key = new();

            Assert.Null(key.EventType);
            Assert.Null(key.StateKey);
        }

        [Fact]
        public void EqualsWithOtherTypes()
        {
            StateDictionaryKey key = new()
            {
                EventType = "m.foo.bar", 
                StateKey = ""
            };
            
            Assert.False(key.Equals(null));
            Assert.False(key.Equals("m.foo.bar"));
            Assert.False(key.Equals(""));
        }

        [Fact]
        public void EqualsWithSameType()
        {
            StateDictionaryKey key_one = new()
            {
                EventType = "m.foo.bar",
                StateKey = ""
            };

            StateDictionaryKey key_two = new()
            {
                EventType = "m.foo.bar",
                StateKey = ""
            };

            Assert.True(key_one.Equals(key_two));
            Assert.True(key_two.Equals(key_one));

            // Not equal when state keys are different.
            key_two.StateKey = "something else";
            Assert.False(key_one.Equals(key_two));
            Assert.False(key_two.Equals(key_one));

            // Not equal when event type is different.
            key_one = new()
            {
                EventType = "m.foo.bar",
                StateKey = ""
            };
            key_two = new()
            {
                EventType = "m.baz.quux",
                StateKey = ""
            };
            Assert.False(key_one.Equals(key_two));
            Assert.False(key_two.Equals(key_one));

            // Not equal when event type and state key are different.
            key_one = new()
            {
                EventType = "m.foo.bar",
                StateKey = "foo"
            };
            key_two = new()
            {
                EventType = "m.baz.quux",
                StateKey = "bar"
            };
            Assert.False(key_one.Equals(key_two));
            Assert.False(key_two.Equals(key_one));
        }

        [Fact]
        public void HashCodeTests()
        {
            StateDictionaryKey key_one = new()
            {
                EventType = "m.foo.bar",
                StateKey = ""
            };

            StateDictionaryKey key_two = new()
            {
                EventType = "m.foo.bar",
                StateKey = ""
            };

            Assert.Equal(key_one.GetHashCode(), key_two.GetHashCode());

            // Not equal when state keys are different.
            key_two.StateKey = "something else";
            Assert.NotEqual(key_one.GetHashCode(), key_two.GetHashCode());

            // Not equal when event type is different.
            key_one = new()
            {
                EventType = "m.foo.bar",
                StateKey = ""
            };
            key_two = new()
            {
                EventType = "m.baz.quux",
                StateKey = ""
            };
            Assert.NotEqual(key_one.GetHashCode(), key_two.GetHashCode());

            // Not equal when event type and state key are different.
            key_one = new()
            {
                EventType = "m.foo.bar",
                StateKey = "foo"
            };
            key_two = new()
            {
                EventType = "m.baz.quux",
                StateKey = "bar"
            };
            Assert.NotEqual(key_one.GetHashCode(), key_two.GetHashCode());
        }
    }
}