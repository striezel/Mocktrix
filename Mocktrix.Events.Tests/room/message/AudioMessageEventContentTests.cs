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

using System.Text.Encodings.Web;
using System.Text.Json;

namespace Mocktrix.Events.Tests
{
    /// <summary>
    /// Contains tests for AudioMessageEventContent.
    /// </summary>
    public class AudioMessageEventContentTests
    {
        [Fact]
        public void Construction()
        {
            var content = new AudioMessageEventContent();
            Assert.Null(content.Body);
            Assert.Null(content.Info);
            Assert.Equal("m.audio", content.MessageType);
            Assert.Null(content.Url);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "body": "Bee Gees - Stayin' Alive",
                           "info": {
                               "duration": 2140786,
                               "mimetype": "audio/mpeg",
                               "size": 1563685
                           },
                           "msgtype": "m.audio",
                           "url": "mxc://example.org/ffed755USFFxlgbQYZGtryd"
                       }
                       """;
            var content = JsonSerializer.Deserialize<AudioMessageEventContent>(json);

            Assert.NotNull(content);
            Assert.Equal("Bee Gees - Stayin' Alive", content.Body);
            Assert.NotNull(content.Info);
            Assert.Equal(2140786, content.Info.Duration);
            Assert.Equal("audio/mpeg", content.Info.MimeType);
            Assert.Equal(1563685, content.Info.Size);
            Assert.Equal("m.audio", content.MessageType);
            Assert.Equal("mxc://example.org/ffed755USFFxlgbQYZGtryd", content.Url);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var content = new AudioMessageEventContent
            {
                Body = "Bee Gees - Stayin' Alive",
                Info = new AudioInfo()
                {
                    Duration = 2140786,
                    MimeType = "audio/mpeg",
                    Size = 1563685
                },
                MessageType = "m.audio",
                Url = "mxc://example.org/ffed755USFFxlgbQYZGtryd"
            };
            var expected_json = "{\"body\":\"Bee Gees - Stayin' Alive\",\"info\":{\"duration\":2140786,\"mimetype\":\"audio/mpeg\",\"size\":1563685},\"msgtype\":\"m.audio\",\"url\":\"mxc://example.org/ffed755USFFxlgbQYZGtryd\"}";
            var json = JsonSerializer.Serialize(content, new JsonSerializerOptions(JsonSerializerDefaults.Web) { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}
