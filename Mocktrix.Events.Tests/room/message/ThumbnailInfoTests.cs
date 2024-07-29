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
    /// Contains tests for ThumbnailInfo.
    /// </summary>
    public class ThumbnailInfoTests
    {
        [Fact]
        public void Construction()
        {
            var content = new ThumbnailInfo();
            Assert.Null(content.Height);
            Assert.Null(content.MimeType);
            Assert.Null(content.Size);
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
            var info = JsonSerializer.Deserialize<ThumbnailInfo>(json);

            Assert.NotNull(info);
            Assert.Equal(768, info.Height);
            Assert.Equal("image/jpeg", info.MimeType);
            Assert.Equal(211009, info.Size);
            Assert.Equal(432, info.Width);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var info = new ThumbnailInfo
            {
                Height = 768,
                MimeType = "image/jpeg",
                Size = 211009,
                Width = 432
            };
            var expected_json = "{\"h\":768,\"mimetype\":\"image/jpeg\",\"size\":211009,\"w\":432}";
            var json = JsonSerializer.Serialize(info);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}
