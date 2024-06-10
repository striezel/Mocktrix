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

namespace Mocktrix.Configuration.Tests
{
    /// <summary>
    /// Contains tests for the ConfigurationManager class.
    /// </summary>
    public class ConfigurationManagerTests
    {
        /// <summary>
        /// Checks whether the default instance is not null.
        /// </summary>
        [Fact]
        public void Default()
        {
            var def = ConfigurationManager.Default;
            Assert.NotNull(def);
        }


        /// <summary>
        /// Checks whether the current instance is not null and always returns
        /// the same instance.
        /// </summary>
        [Fact]
        public void Current()
        {
            var cur1 = ConfigurationManager.Current;
            Assert.NotNull(cur1);

            var cur2 = ConfigurationManager.Current;
            Assert.NotNull(cur2);

            Assert.True(ReferenceEquals(cur1, cur2));
        }


        /// <summary>
        /// Checks that current and default instance are not the same instance.
        /// </summary>
        [Fact]
        public void CurrentIsNotSameAsDefault()
        {
            var cur = ConfigurationManager.Current;
            var def = ConfigurationManager.Default;
            Assert.False(ReferenceEquals(cur, def));
        }
    }
}
