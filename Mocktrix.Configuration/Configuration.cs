/*
    This file is part of Mocktrix.
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

using System.Xml;
using System.Xml.Serialization;

namespace Mocktrix.Configuration
{
    /// <summary>
    /// Contains configuration settings for the homeserver.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Creates a configuration with default settings.
        /// </summary>
        public Configuration() {
            EnableRegistration = true;
            UploadLimit = 1024 * 1024 * 10; // 10 MB
        }


        /// <summary>
        /// Whether account registration is enabled for the homeserver.
        /// </summary>
        public bool EnableRegistration { get; set; }


        /// <summary>
        /// The upload limit in bytes for media uploaded to the homeserver's
        /// media repository.
        /// </summary>
        public ulong UploadLimit { get; set; }


        /// <summary>
        /// Saves the configuration's data to the given file.
        /// </summary>
        /// <param name="path">file name where the data shall be saved</param>
        /// <returns>Returns whether the save operation was successful.</returns>
        public bool SaveToFile(string path)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(Configuration));
                var settings = new XmlWriterSettings()
                {
                    Indent = true,
                    Encoding = System.Text.Encoding.UTF8
                };
                var writer = XmlWriter.Create(path, settings);
                serializer.Serialize(writer, this);
                writer.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// Loads the configuration's data from the given file.
        /// </summary>
        /// <param name="path">file from which the data shall be read</param>
        /// <returns>Returns whether the load operation was successful.</returns>
        public bool LoadFromFile(string path)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(Configuration));
                var stream = new FileStream(path, FileMode.Open);
                Configuration? data = (Configuration?)serializer.Deserialize(stream);
                stream.Close();
                if (data == null)
                    return false;
                EnableRegistration = data.EnableRegistration;
                UploadLimit = data.UploadLimit;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
