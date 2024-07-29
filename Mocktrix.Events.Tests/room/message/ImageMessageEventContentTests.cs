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
    /// Contains tests for ImageMessageEventContent.
    /// </summary>
    public class ImageMessageEventContentTests
    {
        [Fact]
        public void Construction()
        {
            var content = new ImageMessageEventContent();
            Assert.Null(content.Body);
            Assert.Null(content.Info);
            Assert.Equal("m.image", content.MessageType);
            Assert.Null(content.Url);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "body": "filename.jpg",
                           "info": {
                               "h": 398,
                               "mimetype": "image/jpeg",
                               "size": 31037,
                               "w": 394
                           },
                           "msgtype": "m.image",
                           "url": "mxc://example.org/JWEIFJgwEIhweiWJE"
                       }
                       """;
            var content = JsonSerializer.Deserialize<ImageMessageEventContent>(json);

            Assert.NotNull(content);
            Assert.Equal("filename.jpg", content.Body);
            Assert.NotNull(content.Info);
            Assert.Equal(398, content.Info.Height);
            Assert.Equal("image/jpeg", content.Info.MimeType);
            Assert.Equal(31037, content.Info.Size);
            Assert.Equal(394, content.Info.Width);
            Assert.Equal("m.image", content.MessageType);
            Assert.Equal("mxc://example.org/JWEIFJgwEIhweiWJE", content.Url);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var content = new ImageMessageEventContent
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
            };
            var expected_json = "{\"body\":\"filename.jpg\",\"info\":{\"h\":398,\"mimetype\":\"image/jpeg\",\"size\":31037,\"w\":394},\"msgtype\":\"m.image\",\"url\":\"mxc://example.org/JWEIFJgwEIhweiWJE\"}";
            var json = JsonSerializer.Serialize(content);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}
