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
    /// Contains tests for LocationInfo.
    /// </summary>
    public class LocationInfoTests
    {
        [Fact]
        public void Construction()
        {
            var content = new LocationInfo();
            Assert.Null(content.ThumbnailInfo);
            Assert.Null(content.ThumbnailUrl);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "thumbnail_info": {
                               "h": 300,
                               "mimetype": "image/jpeg",
                               "size": 46144,
                               "w": 301
                           },
                           "thumbnail_url": "mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe"
                       }
                       """;
            var info = JsonSerializer.Deserialize<LocationInfo>(json);

            Assert.NotNull(info);
            Assert.NotNull(info.ThumbnailInfo);
            Assert.Equal(300, info.ThumbnailInfo.Height);
            Assert.Equal("image/jpeg", info.ThumbnailInfo.MimeType);
            Assert.Equal(46144, info.ThumbnailInfo.Size);
            Assert.Equal(301, info.ThumbnailInfo.Width);
            Assert.Equal("mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe", info.ThumbnailUrl);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var info = new LocationInfo()
            {
                ThumbnailInfo = new ThumbnailInfo()
                {
                    Height = 300,
                    MimeType = "image/jpeg",
                    Size = 46144,
                    Width = 301
                },
                ThumbnailUrl = "mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe"
            };
            var expected_json = "{\"thumbnail_info\":{\"h\":300,\"mimetype\":\"image/jpeg\",\"size\":46144,\"w\":301},\"thumbnail_url\":\"mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe\"}";
            var json = JsonSerializer.Serialize(info);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}
