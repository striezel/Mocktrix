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
    /// Contains tests for the Configuration class.
    /// </summary>
    public class ConfigurationTests
    {
        private static string GetTempFileName()
        {
            return Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        }

        /// <summary>
        /// Checks whether a configuration can be saved.
        /// </summary>
        [Fact]
        public void SaveToFile()
        {
            var conf = new Configuration();
            var path = GetTempFileName();
            try
            {
                var success = conf.SaveToFile(path);
                Assert.True(success);
            }
            catch
            {
                Assert.Fail("Attempting to save a configuration threw an exception!");
            }
            finally
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        /// <summary>
        /// Checks whether a configuration can be saved to a file.
        /// </summary>
        [Fact]
        public void SaveToFile_Failure()
        {
            var conf = new Configuration();
            var path = System.OperatingSystem.IsWindows()
                ? "C:\\Path\\Here\\Does\\Not\\Exist.tmp"
                : "/path/here/does/not/exist.tmp";
            try
            {
                var success = conf.SaveToFile(path);
                Assert.False(success);
            }
            catch
            {
                Assert.Fail("Attempting to save to a directory that does not exist threw an exception!");
            }
            finally
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        /// <summary>
        /// Checks whether a configuration can be loaded from a file.
        /// </summary>
        [Fact]
        public void LoadFromFile_Failure()
        {
            var conf = new Configuration();
            var path = "C:\\Path\\Does\\Really\\Not\\Exist.tmp";
            try
            {
                var success = conf.LoadFromFile(path);
                Assert.False(success);
            }
            catch
            {
                Assert.Fail("Attempting to load from a file that does not exist threw an exception!");
            }
            finally
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        /// <summary>
        /// Checks whether a configuration can be saved to a file and loaded
        /// from that file again.
        /// </summary>
        [Fact]
        public void SaveLoadRoundtrip()
        {
            var conf = new Configuration
            {
                EnableRegistration = false,
                UploadLimit = 555
            };
            var path = GetTempFileName();
            try
            {
                var success = conf.SaveToFile(path);
                Assert.True(success);
                var loaded = new Configuration();
                Assert.True(loaded.LoadFromFile(path));
                Assert.Equal(conf.EnableRegistration, loaded.EnableRegistration);
                Assert.Equal(conf.UploadLimit, loaded.UploadLimit);
            }
            catch
            {
                Assert.Fail("Attempting to save and re-load a configuration threw an exception!");
            }
            finally
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }
    }
}
