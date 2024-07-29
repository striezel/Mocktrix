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

using System.Text.Json;

namespace Mocktrix.Events.Tests
{
    /// <summary>
    /// Contains tests for ImageMessageEvent.
    /// </summary>
    public class ImageMessageEventTests
    {
        [Fact]
        public void Construction()
        {
            var ev = new ImageMessageEvent();
            Assert.NotNull(ev.Content);
            Assert.IsType<ImageMessageEventContent>(ev.Content);
            Assert.Null(ev.Content.Body);
            Assert.Null(ev.Content.Info);
            Assert.Equal("m.image", ev.Content.MessageType);
            Assert.Null(ev.Content.Url);
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
                               "body": "filename.jpg",
                               "info": {
                                   "h": 398,
                                   "mimetype": "image/jpeg",
                                   "size": 31037,
                                   "w": 394
                               },
                               "msgtype": "m.image",
                               "url": "mxc://example.org/JWEIFJgwEIhweiWJE"
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
            var ev = JsonSerializer.Deserialize<ImageMessageEvent>(json);

            Assert.NotNull(ev);
            Assert.NotNull(ev.Content);
            Assert.Equal("filename.jpg", ev.Content.Body);
            Assert.NotNull(ev.Content.Info);
            Assert.Equal(398, ev.Content.Info.Height);
            Assert.Equal("image/jpeg", ev.Content.Info.MimeType);
            Assert.Equal(31037, ev.Content.Info.Size);
            Assert.Equal(394, ev.Content.Info.Width);
            Assert.Equal("m.image", ev.Content.MessageType);
            Assert.Equal("mxc://example.org/JWEIFJgwEIhweiWJE", ev.Content.Url);
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
            var ev = new ImageMessageEvent
            {
                Content = new ImageMessageEventContent
                {
                    Body = "filename.jpg",
                    Info = new ImageInfo()
                    {
                        Height = 398,
                        MimeType = "image/jpeg",
                        Size = 31037,
                        Width = 394
                    },
                    MessageType = "m.image",
                    Url = "mxc://example.org/JWEIFJgwEIhweiWJE"
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

            var expected_json = "{\"content\":{\"body\":\"filename.jpg\",\"info\":{\"h\":398,\"mimetype\":\"image/jpeg\",\"size\":31037,\"w\":394},\"msgtype\":\"m.image\",\"url\":\"mxc://example.org/JWEIFJgwEIhweiWJE\"},\"event_id\":\"$143273582443PhrSn:example.org\",\"origin_server_ts\":1432735824653,\"room_id\":\"!jEsUZKDJdhlrceRyVU:example.org\",\"sender\":\"@example:example.org\",\"type\":\"m.room.message\",\"unsigned\":{\"age\":1234}}";
            var json = JsonSerializer.Serialize(ev);
            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }

        [Fact]
        public void IsStateEvent()
        {
            Assert.False(new ImageMessageEvent().IsStateEvent());
        }
    }
}
