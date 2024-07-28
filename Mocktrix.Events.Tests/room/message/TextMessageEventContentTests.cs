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
    /// Contains tests for TextMessageEventContent.
    /// </summary>
    public class TextMessageEventContentTests
    {
        [Fact]
        public void Construction()
        {
            var content = new TextMessageEventContent();
            Assert.Null(content.Body);
            Assert.Null(content.Format);
            Assert.Null(content.FormattedBody);
            Assert.Equal("m.text", content.MessageType);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "body": "This is an example text message",
                           "format": "org.matrix.custom.html",
                           "formatted_body": "<b>This is an example text message</b>",
                           "msgtype": "m.text"
                       }
                       """;
            var content = JsonSerializer.Deserialize<TextMessageEventContent>(json);

            Assert.NotNull(content);
            Assert.Equal("This is an example text message", content.Body);
            Assert.Equal("org.matrix.custom.html", content.Format);
            Assert.Equal("<b>This is an example text message</b>", content.FormattedBody);
            Assert.Equal("m.text", content.MessageType);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var content = new TextMessageEventContent
            {
                Body = "This is an example text message",
                Format = "org.matrix.custom.html",
                FormattedBody = "<b>This is an example text message</b>",
                MessageType = "m.text"
            };
            var expected_json = "{\"body\":\"This is an example text message\",\"format\":\"org.matrix.custom.html\",\"formatted_body\":\"<b>This is an example text message</b>\",\"msgtype\":\"m.text\"}";
            var json = JsonSerializer.Serialize(content, new JsonSerializerOptions(JsonSerializerDefaults.Web) { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}
