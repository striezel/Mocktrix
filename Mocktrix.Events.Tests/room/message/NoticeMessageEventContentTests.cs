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
    /// Contains tests for NoticeMessageEventContent.
    /// </summary>
    public class NoticeMessageEventContentTests
    {
        [Fact]
        public void Construction()
        {
            var content = new NoticeMessageEventContent();
            Assert.Null(content.Body);
            Assert.Null(content.Format);
            Assert.Null(content.FormattedBody);
            Assert.Equal("m.notice", content.MessageType);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "body": "This is an example notice",
                           "format": "org.matrix.custom.html",
                           "formatted_body": "This is an <strong>example</strong> notice",
                           "msgtype": "m.notice"
                       }
                       """;
            var content = JsonSerializer.Deserialize<NoticeMessageEventContent>(json);

            Assert.NotNull(content);
            Assert.Equal("This is an example notice", content.Body);
            Assert.Equal("org.matrix.custom.html", content.Format);
            Assert.Equal("This is an <strong>example</strong> notice", content.FormattedBody);
            Assert.Equal("m.notice", content.MessageType);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var content = new NoticeMessageEventContent
            {
                Body = "This is an example notice",
                Format = "org.matrix.custom.html",
                FormattedBody = "This is an <strong>example</strong> notice",
                MessageType = "m.notice"
            };
            var expected_json = "{\"body\":\"This is an example notice\",\"format\":\"org.matrix.custom.html\",\"formatted_body\":\"This is an <strong>example</strong> notice\",\"msgtype\":\"m.notice\"}";
            var json = JsonSerializer.Serialize(content, new JsonSerializerOptions(JsonSerializerDefaults.Web) { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}
