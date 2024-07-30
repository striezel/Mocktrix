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
    /// Contains tests for AudioInfo.
    /// </summary>
    public class AudioInfoTests
    {
        [Fact]
        public void Construction()
        {
            var content = new AudioInfo();
            Assert.Null(content.Duration);
            Assert.Null(content.MimeType);
            Assert.Null(content.Size);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "duration": 2140786,
                           "mimetype": "audio/mpeg",
                           "size": 1563685
                       }
                       """;
            var info = JsonSerializer.Deserialize<AudioInfo>(json);

            Assert.NotNull(info);
            Assert.Equal(2140786, info.Duration);
            Assert.Equal("audio/mpeg", info.MimeType);
            Assert.Equal(1563685, info.Size);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var info = new AudioInfo
            {
                Duration = 2140786,
                MimeType = "audio/mpeg",
                Size = 1563685
            };
            var expected_json = "{\"duration\":2140786,\"mimetype\":\"audio/mpeg\",\"size\":1563685}";
            var json = JsonSerializer.Serialize(info);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}
