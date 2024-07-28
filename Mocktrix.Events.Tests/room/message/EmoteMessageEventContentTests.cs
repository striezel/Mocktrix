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
    /// Contains tests for EmoteMessageEventContent.
    /// </summary>
    public class EmoteMessageEventContentTests
    {
        [Fact]
        public void Construction()
        {
            var content = new EmoteMessageEventContent();
            Assert.Null(content.Body);
            Assert.Null(content.Format);
            Assert.Null(content.FormattedBody);
            Assert.Equal("m.emote", content.MessageType);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "body": "thinks this is an example emote",
                           "format": "org.matrix.custom.html",
                           "formatted_body": "thinks <b>this</b> is an example emote",
                           "msgtype": "m.emote"
                       }
                       """;
            var content = JsonSerializer.Deserialize<EmoteMessageEventContent>(json);

            Assert.NotNull(content);
            Assert.Equal("thinks this is an example emote", content.Body);
            Assert.Equal("org.matrix.custom.html", content.Format);
            Assert.Equal("thinks <b>this</b> is an example emote", content.FormattedBody);
            Assert.Equal("m.emote", content.MessageType);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var content = new EmoteMessageEventContent
            {
                Body = "thinks this is an example emote",
                Format = "org.matrix.custom.html",
                FormattedBody = "thinks <b>this</b> is an example emote",
                MessageType = "m.emote"
            };
            var expected_json = "{\"body\":\"thinks this is an example emote\",\"format\":\"org.matrix.custom.html\",\"formatted_body\":\"thinks <b>this</b> is an example emote\",\"msgtype\":\"m.emote\"}";
            var json = JsonSerializer.Serialize(content, new JsonSerializerOptions(JsonSerializerDefaults.Web) { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}
