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
    /// Contains tests for FileInfo.
    /// </summary>
    public class FileInfoTests
    {
        [Fact]
        public void Construction()
        {
            var content = new FileInfo();
            Assert.Null(content.MimeType);
            Assert.Null(content.Size);
            Assert.Null(content.ThumbnailInfo);
            Assert.Null(content.ThumbnailUrl);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "mimetype": "application/pdf",
                           "size": 46144
                       }
                       """;
            var info = JsonSerializer.Deserialize<FileInfo>(json);

            Assert.NotNull(info);
            Assert.Equal("application/pdf", info.MimeType);
            Assert.Equal(46144, info.Size);
            Assert.Null(info.ThumbnailInfo);
            Assert.Null(info.ThumbnailUrl);
        }

        [Fact]
        public void DeserializeSpecExample_Constructed()
        {
            var json = """ 
                       {
                           "mimetype": "application/pdf",
                           "size": 46144,
                           "thumbnail_info": {
                               "h": 100,
                               "mimetype": "image/png",
                               "size": 3602,
                               "w": 70
                           },
                           "thumbnail_url": "mxc://matrix.org/sHhqkFCvSkFwtmvtETOtKnLP"
                       }
                       """;
            var info = JsonSerializer.Deserialize<FileInfo>(json);

            Assert.NotNull(info);
            Assert.Equal("application/pdf", info.MimeType);
            Assert.Equal(46144, info.Size);
            Assert.NotNull(info.ThumbnailInfo);
            Assert.Equal(100, info.ThumbnailInfo.Height);
            Assert.Equal("image/png", info.ThumbnailInfo.MimeType);
            Assert.Equal(3602, info.ThumbnailInfo.Size);
            Assert.Equal(70, info.ThumbnailInfo.Width);
            Assert.Equal("mxc://matrix.org/sHhqkFCvSkFwtmvtETOtKnLP", info.ThumbnailUrl);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var info = new FileInfo()
            {
                MimeType = "application/pdf",
                Size = 46144,
                ThumbnailInfo = null,
                ThumbnailUrl = null
            };
            var expected_json = "{\"mimetype\":\"application/pdf\",\"size\":46144}";
            var json = JsonSerializer.Serialize(info);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }

        [Fact]
        public void SerializeSpecExample_Constructed()
        {
            var info = new FileInfo()
            {
                MimeType = "application/pdf",
                Size = 46144,
                ThumbnailInfo = new ThumbnailInfo()
                {
                    Height = 100,
                    MimeType = "image/png",
                    Size = 3602,
                    Width = 70
                },
                ThumbnailUrl = "mxc://matrix.org/sHhqkFCvSkFwtmvtETOtKnLP",
            };
            var expected_json = "{\"mimetype\":\"application/pdf\",\"size\":46144,\"thumbnail_info\":{\"h\":100,\"mimetype\":\"image/png\",\"size\":3602,\"w\":70},\"thumbnail_url\":\"mxc://matrix.org/sHhqkFCvSkFwtmvtETOtKnLP\"}";
            var json = JsonSerializer.Serialize(info);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}
