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

namespace Mocktrix.RoomVersions.Tests
{
    public class SupportTests
    {
        [Fact]
        public void IsSupportedVersion_SpecVersions()
        {
            Assert.True(Support.IsSupportedVersion("1"));
            Assert.True(Support.IsSupportedVersion("2"));
            Assert.True(Support.IsSupportedVersion("3"));
            Assert.False(Support.IsSupportedVersion("4"));
            Assert.False(Support.IsSupportedVersion("5"));
            Assert.False(Support.IsSupportedVersion("6"));
            Assert.False(Support.IsSupportedVersion("7"));
            Assert.False(Support.IsSupportedVersion("8"));
            Assert.False(Support.IsSupportedVersion("9"));
            Assert.False(Support.IsSupportedVersion("10"));
            Assert.False(Support.IsSupportedVersion("11"));
        }

        [Fact]
        public void IsSupportedVersion_NonNumericVersions()
        {
            Assert.False(Support.IsSupportedVersion("1.2-experimental"));
            Assert.False(Support.IsSupportedVersion("test-version"));
            Assert.False(Support.IsSupportedVersion("some_other_version"));
            Assert.False(Support.IsSupportedVersion("bam!"));
        }

        [Fact]
        public void IsSupportedVersion_NullEmptyWhitespace()
        {
            Assert.False(Support.IsSupportedVersion(null!));
            Assert.False(Support.IsSupportedVersion(string.Empty));
            Assert.False(Support.IsSupportedVersion("              "));
        }

        [Fact]
        public void GetSupportedVersions()
        {
            var v = Support.GetSupportedVersions();
            Assert.Contains("1", v);
            Assert.Equal("stable", v["1"]);
            Assert.Contains("2", v);
            Assert.Equal("unstable", v["2"]);
            Assert.Contains("3", v);
            Assert.Equal("unstable", v["3"]);
        }
    }
}