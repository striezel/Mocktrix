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
    /// Contains tests for FileMessageEventContent.
    /// </summary>
    public class FileMessageEventContentTests
    {
        [Fact]
        public void Construction()
        {
            var content = new FileMessageEventContent();
            Assert.Null(content.Body);
            Assert.Null(content.Info);
            Assert.Equal("m.file", content.MessageType);
            Assert.Null(content.Url);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "body": "something-important.doc",
                           "filename": "something-important.doc",
                           "info": {
                               "mimetype": "application/msword",
                               "size": 46144
                           },
                           "msgtype": "m.file",
                           "url": "mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe"
                       }
                       """;
            var content = JsonSerializer.Deserialize<FileMessageEventContent>(json);

            Assert.NotNull(content);
            Assert.Equal("something-important.doc", content.Body);
            Assert.Equal("something-important.doc", content.FileName);
            Assert.NotNull(content.Info);
            Assert.Equal("application/msword", content.Info.MimeType);
            Assert.Equal(46144, content.Info.Size);
            Assert.Null(content.Info.ThumbnailInfo);
            Assert.Null(content.Info.ThumbnailUrl);
            Assert.Equal("m.file", content.MessageType);
            Assert.Equal("mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe", content.Url);
        }

        [Fact]
        public void DeserializeSpecExample_Constructed()
        {
            var json = """ 
                       {
                           "body": "something-important.doc",
                           "filename": "something-important.doc",
                           "info": {
                               "mimetype": "application/msword",
                               "size": 46144,
                               "thumbnail_info": {
                                   "h": 200,
                                   "mimetype": "image/png",
                                   "size": 73602,
                                   "w": 140
                               },
                               "thumbnail_url": "mxc://matrix.org/sHhqkFCvSkFwtmvtETOtKnLP"
                           },
                           "msgtype": "m.file",
                           "url": "mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe"
                       }
                       """;
            var content = JsonSerializer.Deserialize<FileMessageEventContent>(json);

            Assert.NotNull(content);
            Assert.Equal("something-important.doc", content.Body);
            Assert.Equal("something-important.doc", content.FileName);
            Assert.NotNull(content.Info);
            Assert.Equal("application/msword", content.Info.MimeType);
            Assert.Equal(46144, content.Info.Size);
            Assert.NotNull(content.Info.ThumbnailInfo);
            Assert.Equal(200, content.Info.ThumbnailInfo.Height);
            Assert.Equal("image/png", content.Info.ThumbnailInfo.MimeType);
            Assert.Equal(73602, content.Info.ThumbnailInfo.Size);
            Assert.Equal(140, content.Info.ThumbnailInfo.Width);
            Assert.Equal("mxc://matrix.org/sHhqkFCvSkFwtmvtETOtKnLP", content.Info.ThumbnailUrl);
            Assert.Equal("m.file", content.MessageType);
            Assert.Equal("mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe", content.Url);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var content = new FileMessageEventContent
            {
                Body = "something-important.doc",
                FileName = "something-important.doc",
                Info = new FileInfo()
                {
                    MimeType = "application/msword",
                    Size = 46144,
                    ThumbnailInfo = null,
                    ThumbnailUrl = null
                },
                MessageType = "m.file",
                Url = "mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe"
            };
            var expected_json = "{\"body\":\"something-important.doc\",\"filename\":\"something-important.doc\",\"info\":{\"mimetype\":\"application/msword\",\"size\":46144},\"msgtype\":\"m.file\",\"url\":\"mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe\"}";
            var json = JsonSerializer.Serialize(content);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }

        [Fact]
        public void SerializeSpecExample_Constructed()
        {
            var content = new FileMessageEventContent()
            {
                Body = "something-important.doc",
                FileName = "something-important.doc",
                Info = new FileInfo()
                {
                    MimeType = "application/msword",
                    Size = 46144,
                    ThumbnailInfo = new ThumbnailInfo()
                    {
                        Height = 200,
                        MimeType = "image/png",
                        Size = 73602,
                        Width = 140
                    },
                    ThumbnailUrl = "mxc://matrix.org/sHhqkFCvSkFwtmvtETOtKnLP"
                },
                MessageType = "m.file",
                Url = "mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe"
            };
            var expected_json = "{\"body\":\"something-important.doc\",\"filename\":\"something-important.doc\",\"info\":{\"mimetype\":\"application/msword\",\"size\":46144,\"thumbnail_info\":{\"h\":200,\"mimetype\":\"image/png\",\"size\":73602,\"w\":140},\"thumbnail_url\":\"mxc://matrix.org/sHhqkFCvSkFwtmvtETOtKnLP\"},\"msgtype\":\"m.file\",\"url\":\"mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe\"}";
            var json = JsonSerializer.Serialize(content);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}
