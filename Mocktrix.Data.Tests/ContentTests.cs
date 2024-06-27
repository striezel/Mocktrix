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
    /// <summary>
    /// Contains tests for the Content class.
    /// </summary>
    public class ContentTests
    {
        [Fact]
        public void Constructor()
        {
            Content content = new("foo", "text/plain", "hello.txt", "Hello, Matrix!"u8.ToArray());

            Assert.NotNull(content);
            Assert.Equal("foo", content.Id);
            Assert.Equal("text/plain", content.ContentType);
            Assert.Equal("hello.txt", content.FileName);
            Assert.Equal(14, content.Bytes.Length);
            Assert.Equal([0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x2C, 0x20, 0x4D, 0x61, 0x74, 0x72, 0x69, 0x78, 0x21], content.Bytes);
        }

        [Fact]
        public void GenerateRandomId()
        {
            var id_one = Content.GenerateRandomId();
            Assert.NotNull(id_one);
            Assert.Matches("^[A-Za-z]+$", id_one);

            var id_two = Content.GenerateRandomId();
            Assert.NotNull(id_two);
            Assert.Matches("^[A-Za-z]+$", id_two);

            // Ideally, both ids are unequal, but there is a slim chance to get
            // the same id twice, because it's random and that could give us the
            // same sequence twice, although it is very unlikely.
            Assert.NotEqual(id_one, id_two);
        }


        [Fact]
        public void SanitizeFileName()
        {
            Assert.Null(Content.SanitizeFileName(null));
            Assert.Null(Content.SanitizeFileName(""));
            Assert.Null(Content.SanitizeFileName("           "));

            Assert.Equal("foo.txt", Content.SanitizeFileName("foo.txt"));
            Assert.Equal("bar.txt", Content.SanitizeFileName("   bar.txt"));
            Assert.Equal("baz.txt", Content.SanitizeFileName("baz.txt   "));

            Assert.Equal("foo.txt", Content.SanitizeFileName("../../../../../foo.txt"));
            Assert.Equal("bar.txt", Content.SanitizeFileName("..\\..\\..\\..\\bar.txt"));
        }
    }
}