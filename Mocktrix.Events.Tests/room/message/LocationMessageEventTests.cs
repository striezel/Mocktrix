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
    /// Contains tests for LocationMessageEvent.
    /// </summary>
    public class LocationMessageEventTests
    {
        [Fact]
        public void Construction()
        {
            var ev = new LocationMessageEvent();
            Assert.NotNull(ev.Content);
            Assert.IsType<LocationMessageEventContent>(ev.Content);
            Assert.Null(ev.Content.Body);
            Assert.Null(ev.Content.GeoUri);
            Assert.Null(ev.Content.Info);
            Assert.Equal("m.location", ev.Content.MessageType);
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
                               "body": "Big Ben, London, UK",
                               "geo_uri": "geo:51.5008,0.1247",
                               "info": {
                                   "thumbnail_info": {
                                       "h": 300,
                                       "mimetype": "image/jpeg",
                                       "size": 46144,
                                       "w": 301
                                   },
                                   "thumbnail_url": "mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe"
                               },
                               "msgtype": "m.location"
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
            var ev = JsonSerializer.Deserialize<LocationMessageEvent>(json);

            Assert.NotNull(ev);
            Assert.NotNull(ev.Content);
            Assert.Equal("Big Ben, London, UK", ev.Content.Body);
            Assert.Equal("geo:51.5008,0.1247", ev.Content.GeoUri);
            Assert.NotNull(ev.Content.Info);
            Assert.NotNull(ev.Content.Info.ThumbnailInfo);
            Assert.Equal(300, ev.Content.Info.ThumbnailInfo.Height);
            Assert.Equal("image/jpeg", ev.Content.Info.ThumbnailInfo.MimeType);
            Assert.Equal(46144, ev.Content.Info.ThumbnailInfo.Size);
            Assert.Equal(301, ev.Content.Info.ThumbnailInfo.Width);
            Assert.Equal("mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe", ev.Content.Info.ThumbnailUrl);
            Assert.Equal("m.location", ev.Content.MessageType);
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
            var ev = new LocationMessageEvent
            {
                Content = new LocationMessageEventContent
                {
                    Body = "Big Ben, London, UK",
                    GeoUri = "geo:51.5008,0.1247",
                    Info = new LocationInfo()
                    {
                        ThumbnailInfo = new ThumbnailInfo()
                        {
                            Height = 300,
                            MimeType = "image/jpeg",
                            Size = 46144,
                            Width = 301
                        },
                        ThumbnailUrl = "mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe"
                    },
                    MessageType = "m.location"
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

            var expected_json = "{\"content\":{\"body\":\"Big Ben, London, UK\",\"geo_uri\":\"geo:51.5008,0.1247\",\"info\":{\"thumbnail_info\":{\"h\":300,\"mimetype\":\"image/jpeg\",\"size\":46144,\"w\":301},\"thumbnail_url\":\"mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe\"},\"msgtype\":\"m.location\"},\"event_id\":\"$143273582443PhrSn:example.org\",\"origin_server_ts\":1432735824653,\"room_id\":\"!jEsUZKDJdhlrceRyVU:example.org\",\"sender\":\"@example:example.org\",\"type\":\"m.room.message\",\"unsigned\":{\"age\":1234}}";
            var json = JsonSerializer.Serialize(ev);
            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }

        [Fact]
        public void IsStateEvent()
        {
            Assert.False(new LocationMessageEvent().IsStateEvent());
        }
    }
}
