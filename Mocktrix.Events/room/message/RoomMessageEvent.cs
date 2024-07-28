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
    /// Event for messages of a room.
    /// </summary>
    public class RoomMessageEvent<C>: RoomEvent
        where C : RoomMessageEventContent, new()
    {
        /// <summary>
        /// The content object of the event. Type and available field differ
        /// depending on the concrete type.
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(IEvent.ContentPropertyOrder)]
        public C Content { get; set; } = new C();


        [JsonPropertyName("type")]
        [JsonPropertyOrder(-30)]
        public override string Type
        {
            get => "m.room.message";
            set
            {
                if (value != "m.room.message")
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be 'm.room.message'.");
                }
            }
        }
    }


    /// <summary>
    /// Event content for RoomMessageEvent.
    /// </summary>
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "msgtype")]
    [JsonDerivedType(typeof(EmoteMessageEventContent), "m.emote")]
    [JsonDerivedType(typeof(TextMessageEventContent), "m.text")]
    public abstract class RoomMessageEventContent : IEventContent
    {
        /// <summary>
        /// The textual representation of this message.
        /// </summary>
        [JsonPropertyName("body")]
        [JsonPropertyOrder(-100)]
        public string Body { get; set; } = null!;


        /// <summary>
        /// The type of message, e.g. "m.text", "m.file", ...
        /// </summary>
        [JsonPropertyName("msgtype")]
        [JsonPropertyOrder(-50)]
        public abstract string MessageType { get; set; }
    }
}
