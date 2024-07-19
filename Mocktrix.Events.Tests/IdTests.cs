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

namespace Mocktrix.Events.Tests
{
    /// <summary>
    /// Contains tests for Id class.
    /// </summary>
    public class IdTests
    {
        [Fact]
        public void Generate_Simple()
        {
            var id = Id.Generate(new Uri("https://matrix.example.org/"));
            Assert.NotNull(id);
            Assert.NotEmpty(id);
            Assert.StartsWith("$", id);
            Assert.EndsWith(":matrix.example.org", id);
            Assert.Matches("[A-Za-z0-9]{20}", id);
        }

        [Fact]
        public void Generate_IdContainsServer()
        {
            // localhost
            {
                var id = Id.Generate(new Uri("http://localhost:8080/"));
                Assert.NotNull(id);
                Assert.NotEmpty(id);
                Assert.StartsWith("$", id);
                Assert.EndsWith(":localhost", id);
                Assert.Matches("[A-Za-z0-9]{20}", id);
            }

            // foo.example.com
            {
                var id = Id.Generate(new Uri("https://foo.example.com"));
                Assert.NotNull(id);
                Assert.NotEmpty(id);
                Assert.StartsWith("$", id);
                Assert.EndsWith(":foo.example.com", id);
                Assert.Matches("[A-Za-z0-9]{20}", id);
            }

            // some.domain.tld
            {
                var id = Id.Generate(new Uri("https://some.domain.tld"));
                Assert.NotNull(id);
                Assert.NotEmpty(id);
                Assert.StartsWith("$", id);
                Assert.EndsWith(":some.domain.tld", id);
                Assert.Matches("[A-Za-z0-9]{20}", id);
            }
        }

        [Fact]
        public void Generate_Twice()
        {
            var uri = new Uri("https://matrix.example.org/");
            var id1 = Id.Generate(uri);
            var id2 = Id.Generate(uri);

            Assert.NotNull(id1);
            Assert.NotNull(id2);

            // Ideally, both event ids are unequal, but there is a slim chance
            // to get the same id twice, because it's random and that could
            // give us the same sequence twice, although it is very unlikely.
            Assert.NotEqual(id1, id2);
        }
    }
}
