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
    /// Contains tests for EncryptionEventContent.
    /// </summary>
    public class EncryptionEventContentTests
    {
        [Fact]
        public void Construction()
        {
            var content = new EncryptionEventContent();
            Assert.Null(content.Algorithm);
            Assert.Null(content.RotationPeriodMilliseconds);
            Assert.Null(content.RotationPeriodMessages);
        }

        [Fact]
        public void DeserializeSpecExample()
        {
            var json = """ 
                       {
                           "algorithm": "m.megolm.v1.aes-sha2",
                           "rotation_period_ms": 604800000,
                           "rotation_period_msgs": 100
                       }
                       """;
            var content = JsonSerializer.Deserialize<EncryptionEventContent>(json);

            Assert.NotNull(content);
            Assert.Equal("m.megolm.v1.aes-sha2", content.Algorithm);
            Assert.Equal(604800000, content.RotationPeriodMilliseconds);
            Assert.Equal(100, content.RotationPeriodMessages);
        }

        [Fact]
        public void SerializeSpecExample()
        {
            var content = new EncryptionEventContent
            {
                Algorithm = "m.megolm.v1.aes-sha2",
                RotationPeriodMilliseconds = 604800000,
                RotationPeriodMessages = 100
            };
            var expected_json = "{\"algorithm\":\"m.megolm.v1.aes-sha2\",\"rotation_period_ms\":604800000,\"rotation_period_msgs\":100}";
            var json = JsonSerializer.Serialize(content);

            Assert.NotNull(json);
            Assert.Equal(expected_json, json);
        }
    }
}
