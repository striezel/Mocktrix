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
    /// Contains tests for VideoMessageEventContent.
    /// </summary>
    public class VideoMessageEventContentTests
    {
        [Fact]
        public void Construction()
        {
            var content = new VideoMessageEventContent();
            Assert.Null(content.Body);
            Assert.Null(content.Info);
            Assert.Equal("m.video", content.MessageType);
            Assert.Null(content.Url);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "body": "Never Gonna Give You Up",
                           "info": {
                               "duration": 2140786,
                               "h": 320,
                               "mimetype": "video/mp4",
                               "size": 1563685,
                               "thumbnail_info": {
                                   "h": 300,
                                   "mimetype": "image/jpeg",
                                   "size": 41644,
                                   "w": 302
                               },
                               "thumbnail_url": "mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe",
                               "w": 480
                           },
                           "msgtype": "m.video",
                           "url": "mxc://example.org/a526eYUSFFxlgbQYZmo442"
                       }
                       """;
            var content = JsonSerializer.Deserialize<VideoMessageEventContent>(json);

            Assert.NotNull(content);
            Assert.Equal("Never Gonna Give You Up", content.Body);
            Assert.NotNull(content.Info);
            Assert.Equal(2140786, content.Info.Duration);
            Assert.Equal(320, content.Info.Height);
            Assert.Equal("video/mp4", content.Info.MimeType);
            Assert.Equal(1563685, content.Info.Size);
            Assert.NotNull(content.Info.ThumbnailInfo);
            Assert.Equal(300, content.Info.ThumbnailInfo.Height);
            Assert.Equal("image/jpeg", content.Info.ThumbnailInfo.MimeType);
            Assert.Equal(41644, content.Info.ThumbnailInfo.Size);
            Assert.Equal(302, content.Info.ThumbnailInfo.Width);
            Assert.Equal("mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe", content.Info.ThumbnailUrl);
            Assert.Equal(480, content.Info.Width);
            Assert.Equal("m.video", content.MessageType);
            Assert.Equal("mxc://example.org/a526eYUSFFxlgbQYZmo442", content.Url);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var content = new VideoMessageEventContent
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
            };
            var expected_json = "{\"body\":\"Never Gonna Give You Up\",\"info\":{\"duration\":2140786,\"h\":320,\"mimetype\":\"video/mp4\",\"size\":1563685,\"thumbnail_info\":{\"h\":300,\"mimetype\":\"image/jpeg\",\"size\":46144,\"w\":302},\"thumbnail_url\":\"mxc://example.org/FHyPlCeYUSFFxlgbQYZmoEoe\",\"w\":480},\"msgtype\":\"m.video\",\"url\":\"mxc://example.org/a526eYUSFFxlgbQYZmo442\"}";
            var json = JsonSerializer.Serialize(content);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}
