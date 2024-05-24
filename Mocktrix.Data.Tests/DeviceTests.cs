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

namespace Mocktrix.Data.Tests
{
    public class DeviceTests
    {
        [Fact]
        public void Constructor_WithoutDisplayName()
        {
            Device dev = new("Foobarbaz", "@alice:matrix.example.com");

            Assert.NotNull(dev);
            Assert.Equal("Foobarbaz", dev.device_id);
            Assert.Equal("@alice:matrix.example.com", dev.user_id);
            Assert.Null(dev.display_name);
        }


        [Fact]
        public void Constructor_WithDisplayName()
        {
            Device dev = new("Foobarbaz", "@bob:matrix.example.com", "My Phone");

            Assert.NotNull(dev);
            Assert.Equal("Foobarbaz", dev.device_id);
            Assert.Equal("@bob:matrix.example.com", dev.user_id);
            Assert.Equal("My Phone", dev.display_name);
        }


        [Fact]
        public void GenerateRandomId()
        {
            var id_one = Device.GenerateRandomId();
            Assert.NotNull(id_one);
            Assert.Matches("^[A-Z]+$", id_one);

            var id_two = Device.GenerateRandomId();
            Assert.NotNull(id_two);
            Assert.Matches("^[A-Z]+$", id_two);

            // Ideally, both ids are unequal, but there is a slim chance to get
            // the same id twice, because it's random and that could give us the
            // same sequence twice, although it is very unlikely.
            Assert.NotEqual(id_one, id_two);
            
        }
    }
}