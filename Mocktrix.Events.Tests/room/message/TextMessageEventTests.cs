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

using System.Text.Encodings.Web;
using System.Text.Json;

namespace Mocktrix.Events.Tests
{
    /// <summary>
    /// Contains tests for TextMessageEvent.
    /// </summary>
    public class TextMessageEventTests
    {
        [Fact]
        public void Construction()
        {
            var ev = new TextMessageEvent();
            Assert.NotNull(ev.Content);
            Assert.IsType<TextMessageEventContent>(ev.Content);
            Assert.Null(ev.Content.Body);
            Assert.Null(ev.Content.Format);
            Assert.Null(ev.Content.FormattedBody);
            Assert.Equal("m.text", ev.Content.MessageType);
            Assert.Null(ev.EventId);
            Assert.Equal(0, ev.OriginServerTs);
            Assert.Null(ev.RoomId);
            Assert.Null(ev.Sender);
            Assert.Equal("m.room.message", ev.Type);
            Assert.Null(ev.Unsigned);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "content": {
                               "body": "This is an example text message",
                               "format": "org.matrix.custom.html",
                               "formatted_body": "<b>This is an example text message</b>",
                               "msgtype": "m.text"
                           },
                           "event_id": "$143273582443PhrSn:example.org",
                           "origin_server_ts": 1432735824653,
                           "room_id": "!jEsUZKDJdhlrceRyVU:example.org",
                           "sender": "@example:example.org",
                           "type": "m.room.message",
                           "unsigned": {
                               "age": 1234
                           }
                       }
                       """;
            var ev = JsonSerializer.Deserialize<TextMessageEvent>(json);

            Assert.NotNull(ev);
            Assert.NotNull(ev.Content);
            Assert.Equal("This is an example text message", ev.Content.Body);
            Assert.Equal("org.matrix.custom.html", ev.Content.Format);
            Assert.Equal("<b>This is an example text message</b>", ev.Content.FormattedBody);
            Assert.Equal("$143273582443PhrSn:example.org", ev.EventId);
            Assert.Equal(1432735824653, ev.OriginServerTs);
            Assert.Equal("!jEsUZKDJdhlrceRyVU:example.org", ev.RoomId);
            Assert.Equal("@example:example.org", ev.Sender);
            Assert.Equal("m.room.message", ev.Type);
            Assert.NotNull(ev.Unsigned);
            Assert.Equal(1234, ev.Unsigned.Age);
            Assert.Null(ev.Unsigned.RedactedBecause);
            Assert.Null(ev.Unsigned.TransactionId);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var ev = new TextMessageEvent
            {
                Content = new TextMessageEventContent
                {
                    Body = "This is an example text message",
                    Format = "org.matrix.custom.html",
                    FormattedBody = "<b>This is an example text message</b>",
                    MessageType = "m.text"
                },
                EventId = "$143273582443PhrSn:example.org",
                OriginServerTs = 1432735824653,
                RoomId = "!jEsUZKDJdhlrceRyVU:example.org",
                Sender = "@example:example.org",
                Type = "m.room.message",
                Unsigned = new UnsignedData()
                {
                    Age = 1234,
                    RedactedBecause = null,
                    TransactionId = null
                }
            };

            var expected_json = "{\"content\":{\"body\":\"This is an example text message\",\"format\":\"org.matrix.custom.html\",\"formatted_body\":\"<b>This is an example text message</b>\",\"msgtype\":\"m.text\"},\"event_id\":\"$143273582443PhrSn:example.org\",\"origin_server_ts\":1432735824653,\"room_id\":\"!jEsUZKDJdhlrceRyVU:example.org\",\"sender\":\"@example:example.org\",\"type\":\"m.room.message\",\"unsigned\":{\"age\":1234}}";
            var json = JsonSerializer.Serialize(ev, new JsonSerializerOptions(JsonSerializerDefaults.Web) { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }

        [Fact]
        public void IsStateEvent()
        {
            Assert.False(new TextMessageEvent().IsStateEvent());
        }
    }
}
