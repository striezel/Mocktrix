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

namespace Mocktrix.ContentRepository.Memory.Tests
{
    public class MediaTests
    {
        [Fact]
        public void Create_FullParameterSet()
        {
            var id = Media.Create("This is content."u8.ToArray(), "text/plain", "foo.txt");
            Assert.NotEmpty(id);
            Assert.Matches("^[A-Za-z]+$", id);

            var content = Media.GetContent(id);
            Assert.NotNull(content);
            Assert.Equal(id, content.Id);
            Assert.Equal("text/plain", content.ContentType);
            Assert.Equal("foo.txt", content.FileName);
            Assert.Equal(16, content.Bytes.Length);
            Assert.Equal("This is content."u8.ToArray(), content.Bytes);
        }

        [Fact]
        public void Create_NoContentType()
        {
            var id = Media.Create("more content"u8.ToArray(), null, "content.txt");
            Assert.NotEmpty(id);
            Assert.Matches("^[A-Za-z]+$", id);

            var content = Media.GetContent(id);
            Assert.NotNull(content);
            Assert.Equal(id, content.Id);
            Assert.Null(content.ContentType);
            Assert.Equal("content.txt", content.FileName);
            Assert.Equal(12, content.Bytes.Length);
            Assert.Equal("more content"u8.ToArray(), content.Bytes);
        }

        [Fact]
        public void Create_NoFileName()
        {
            var id = Media.Create("some content"u8.ToArray(), "text/plain", null);
            Assert.NotEmpty(id);
            Assert.Matches("^[A-Za-z]+$", id);

            var content = Media.GetContent(id);
            Assert.NotNull(content);
            Assert.Equal(id, content.Id);
            Assert.Equal("text/plain", content.ContentType);
            Assert.Null(content.FileName);
            Assert.Equal(12, content.Bytes.Length);
            Assert.Equal("some content"u8.ToArray(), content.Bytes);
        }

        [Fact]
        public void Create_NoMetaData()
        {
            var id = Media.Create("the content"u8.ToArray(), null, null);
            Assert.NotEmpty(id);
            Assert.Matches("^[A-Za-z]+$", id);

            var content = Media.GetContent(id);
            Assert.NotNull(content);
            Assert.Equal(id, content.Id);
            Assert.Null(content.ContentType);
            Assert.Null(content.FileName);
            Assert.Equal(11, content.Bytes.Length);
            Assert.Equal("the content"u8.ToArray(), content.Bytes);
        }

        [Fact]
        public void Create_SanitizesFileName()
        {
            var id = Media.Create("This is content."u8.ToArray(), "text/plain", "../../sani.txt");
            Assert.NotEmpty(id);
            Assert.Matches("^[A-Za-z]+$", id);

            var content = Media.GetContent(id);
            Assert.NotNull(content);
            Assert.Equal(id, content.Id);
            Assert.Equal("text/plain", content.ContentType);
            Assert.Equal("sani.txt", content.FileName);
            Assert.Equal(16, content.Bytes.Length);
            Assert.Equal("This is content."u8.ToArray(), content.Bytes);
        }

        [Fact]
        public void GetContent_NonExistentContentNotFound()
        {
            const string media_id = "NotFoundHere";
            var content = Media.GetContent(media_id);

            // Content does not exist, function shall return null.
            Assert.Null(content);
        }
    }
}