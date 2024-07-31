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
    /// Contains tests for LocationMessageEventContent.
    /// </summary>
    public class LocationMessageEventContentTests
    {
        [Fact]
        public void Construction()
        {
            var content = new LocationMessageEventContent();
            Assert.Null(content.Body);
            Assert.Null(content.GeoUri);
            Assert.Null(content.Info);
            Assert.Equal("m.location", content.MessageType);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
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
                       }
                       """;
            var content = JsonSerializer.Deserialize<LocationMessageEventContent>(json);

            Assert.NotNull(content);
            Assert.Equal("Big Ben, London, UK", content.Body);
            Assert.Equal("geo:51.5008,0.1247", content.GeoUri);
            Assert.NotNull(content.Info);
            Assert.NotNull(content.Info.ThumbnailInfo);
            Assert.Equal(300, content.Info.ThumbnailInfo.Height);
            Assert.Equal("image/jpeg", content.Info.ThumbnailInfo.MimeType);
            Assert.Equal(46144, content.Info.ThumbnailInfo.Size);
            Assert.Equal(301, content.Info.ThumbnailInfo.Width);
            Assert.Equal("mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe", content.Info.ThumbnailUrl);
            Assert.Equal("m.location", content.MessageType);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var content = new LocationMessageEventContent
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
                MessageType = "m.location",
            };
            var expected_json = "{\"body\":\"Big Ben, London, UK\",\"geo_uri\":\"geo:51.5008,0.1247\",\"info\":{\"thumbnail_info\":{\"h\":300,\"mimetype\":\"image/jpeg\",\"size\":46144,\"w\":301},\"thumbnail_url\":\"mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe\"},\"msgtype\":\"m.location\"}";
            var json = JsonSerializer.Serialize(content);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}
