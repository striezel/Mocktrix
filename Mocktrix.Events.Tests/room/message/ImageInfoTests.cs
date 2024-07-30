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
    /// Contains tests for ImageInfo.
    /// </summary>
    public class ImageInfoTests
    {
        [Fact]
        public void Construction()
        {
            var content = new ImageInfo();
            Assert.Null(content.Height);
            Assert.Null(content.MimeType);
            Assert.Null(content.Size);
            Assert.Null(content.ThumbnailInfo);
            Assert.Null(content.ThumbnailUrl);
            Assert.Null(content.Width);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                         "h": 768,
                         "mimetype": "image/jpeg",
                         "size": 211009,
                         "w": 432
                       }
                       """;
            var info = JsonSerializer.Deserialize<ImageInfo>(json);

            Assert.NotNull(info);
            Assert.Equal(768, info.Height);
            Assert.Equal("image/jpeg", info.MimeType);
            Assert.Equal(211009, info.Size);
            Assert.Null(info.ThumbnailInfo);
            Assert.Null(info.ThumbnailUrl);
            Assert.Equal(432, info.Width);
        }

        [Fact]
        public void DeserializeSpecExample2()
        {
            var json = """ 
                       {
                           "h": 200,
                           "mimetype": "image/png",
                           "size": 73602,
                           "thumbnail_info": {
                               "h": 100,
                               "mimetype": "image/png",
                               "size": 3602,
                               "w": 70
                           },
                           "thumbnail_url": "mxc://matrix.org/sHhqkFCvSkFwtmvtETOtKnLP",
                           "w": 140
                       }
                       """;
            var info = JsonSerializer.Deserialize<ImageInfo>(json);

            Assert.NotNull(info);
            Assert.Equal(200, info.Height);
            Assert.Equal("image/png", info.MimeType);
            Assert.Equal(73602, info.Size);
            Assert.NotNull(info.ThumbnailInfo);
            Assert.Equal(100, info.ThumbnailInfo.Height);
            Assert.Equal("image/png", info.ThumbnailInfo.MimeType);
            Assert.Equal(3602, info.ThumbnailInfo.Size);
            Assert.Equal(70, info.ThumbnailInfo.Width);
            Assert.Equal("mxc://matrix.org/sHhqkFCvSkFwtmvtETOtKnLP", info.ThumbnailUrl);
            Assert.Equal(140, info.Width);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var info = new ImageInfo
            {
                Height = 768,
                MimeType = "image/jpeg",
                Size = 211009,
                ThumbnailInfo = null,
                ThumbnailUrl = null,
                Width = 432
            };
            var expected_json = "{\"h\":768,\"mimetype\":\"image/jpeg\",\"size\":211009,\"w\":432}";
            var json = JsonSerializer.Serialize(info);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }

        [Fact]
        public void SerializeSpecExample2()
        {
            var info = new ImageInfo
            {
                Height = 200,
                MimeType = "image/png",
                Size = 73602,
                ThumbnailInfo = new ThumbnailInfo()
                {
                    Height = 100,
                    MimeType = "image/png",
                    Size = 3602,
                    Width = 70
                },
                ThumbnailUrl = "mxc://matrix.org/sHhqkFCvSkFwtmvtETOtKnLP",
                Width = 140
            };
            var expected_json = "{\"h\":200,\"mimetype\":\"image/png\",\"size\":73602,\"thumbnail_info\":{\"h\":100,\"mimetype\":\"image/png\",\"size\":3602,\"w\":70},\"thumbnail_url\":\"mxc://matrix.org/sHhqkFCvSkFwtmvtETOtKnLP\",\"w\":140}";
            var json = JsonSerializer.Serialize(info);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}
