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
    /// Generic state event.
    /// </summary>
    /// <typeparam name="C">type of the event content, must be derived from
    /// IEventContent</typeparam>
    public abstract class GenericStateEvent<C> : StateEvent
        where C : IEventContent, new()
    {
        [JsonPropertyName("content")]
        [JsonPropertyOrder(-100)]
        public override IEventContent Content
        {
            get => _content;
            set
            {
                if (value is C content)
                {
                    _content = content;
                }
                else
                {
                    throw new InvalidOperationException("Content must be of type "
                        + typeof(C).Name + ".");
                }
            }
        }


        /// <summary>
        /// The event's content.
        /// </summary>
        private C _content = new();
    }


    /// <summary>
    /// Generic state event with zero length key.
    /// </summary>
    /// <typeparam name="C">type of the event content, must be derived from
    /// IEventContent</typeparam>
    public abstract class GenericStateEventZeroLengthKey<C>: StateEventZeroLengthKey
        where C : IEventContent, new()
    {
        [JsonPropertyName("content")]
        [JsonPropertyOrder(-100)]
        public override IEventContent Content
        {
            get => _content;
            set
            {
                if (value is C content)
                {
                    _content = content;
                }
                else
                {
                    throw new InvalidOperationException("Content must be of type "
                        + typeof(C).Name + ".");
                }
            }
        }


        /// <summary>
        /// The event's content.
        /// </summary>
        private C _content = new();
    }
}
