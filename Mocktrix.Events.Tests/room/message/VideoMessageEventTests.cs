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
    /// Contains tests for VideoMessageEvent.
    /// </summary>
    public class VideoMessageEventTests
    {
        [Fact]
        public void Construction()
        {
            var ev = new VideoMessageEvent();
            Assert.NotNull(ev.Content);
            Assert.IsType<VideoMessageEventContent>(ev.Content);
            Assert.Null(ev.Content.Body);
            Assert.Null(ev.Content.Info);
            Assert.Equal("m.video", ev.Content.MessageType);
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
                               "body": "Never Gonna Give You Up",
                               "info": {
                                   "duration": 2140786,
                                   "h": 320,
                                   "mimetype": "video/mp4",
                                   "size": 1563685,
                                   "thumbnail_info": {
                                       "h": 300,
                                       "mimetype": "image/jpeg",
                                       "size": 46144,
                                       "w": 302
                                   },
                                   "thumbnail_url": "mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe",
                                   "w": 480
                               },
                               "msgtype": "m.video",
                               "url": "mxc://example.org/a526eYUSFFxlgbQYZmo442"
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
            var ev = JsonSerializer.Deserialize<VideoMessageEvent>(json);

            Assert.NotNull(ev);
            Assert.NotNull(ev.Content);
            Assert.Equal("Never Gonna Give You Up", ev.Content.Body);
            Assert.NotNull(ev.Content.Info);
            Assert.Equal(2140786, ev.Content.Info.Duration);
            Assert.Equal(320, ev.Content.Info.Height);
            Assert.Equal("video/mp4", ev.Content.Info.MimeType);
            Assert.Equal(1563685, ev.Content.Info.Size);
            Assert.NotNull(ev.Content.Info.ThumbnailInfo);
            Assert.Equal(300, ev.Content.Info.ThumbnailInfo.Height);
            Assert.Equal("image/jpeg", ev.Content.Info.ThumbnailInfo.MimeType);
            Assert.Equal(46144, ev.Content.Info.ThumbnailInfo.Size);
            Assert.Equal(302, ev.Content.Info.ThumbnailInfo.Width);
            Assert.Equal("mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe", ev.Content.Info.ThumbnailUrl);
            Assert.Equal(480, ev.Content.Info.Width);
            Assert.Equal("m.video", ev.Content.MessageType);
            Assert.Equal("mxc://example.org/a526eYUSFFxlgbQYZmo442", ev.Content.Url);
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
            var ev = new VideoMessageEvent
            {
                Content = new VideoMessageEventContent
                {
                    Body = "Never Gonna Give You Up",
                    Info = new VideoInfo()
                    {
                        Duration = 2140786,
                        Height = 320,
                        MimeType = "video/mp4",
                        Size = 1563685,
                        ThumbnailInfo = new ThumbnailInfo()
                        {
                            Height = 300,
                            MimeType = "image/jpeg",
                            Size = 46144,
                            Width = 302
                        },
                        ThumbnailUrl = "mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe",
                        Width = 480
                    },
                    MessageType = "m.video",
                    Url = "mxc://example.org/a526eYUSFFxlgbQYZmo442"
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

            var expected_json = "{\"content\":{\"body\":\"Never Gonna Give You Up\",\"info\":{\"duration\":2140786,\"h\":320,\"mimetype\":\"video/mp4\",\"size\":1563685,\"thumbnail_info\":{\"h\":300,\"mimetype\":\"image/jpeg\",\"size\":46144,\"w\":302},\"thumbnail_url\":\"mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe\",\"w\":480},\"msgtype\":\"m.video\",\"url\":\"mxc://example.org/a526eYUSFFxlgbQYZmo442\"},\"event_id\":\"$143273582443PhrSn:example.org\",\"origin_server_ts\":1432735824653,\"room_id\":\"!jEsUZKDJdhlrceRyVU:example.org\",\"sender\":\"@example:example.org\",\"type\":\"m.room.message\",\"unsigned\":{\"age\":1234}}";
            var json = JsonSerializer.Serialize(ev);
            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }

        [Fact]
        public void IsStateEvent()
        {
            Assert.False(new VideoMessageEvent().IsStateEvent());
        }
    }
}
