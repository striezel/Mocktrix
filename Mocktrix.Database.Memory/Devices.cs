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

using Mocktrix.Data;

namespace Mocktrix.Database.Memory
{
    /// <summary>
    /// In-memory implementation of device database.
    /// </summary>
    public static class Devices
    {
        /// <summary>
        /// in-memory device list
        /// </summary>
        private static readonly List<Device> devices = [];


        /// <summary>
        /// Creates and adds a new device for a specific user.
        /// </summary>
        /// <param name="dev_id">the device id</param>
        /// <param name="user_id">user id, e.g. "@alice:example.com"</param>
        /// <param name="display_name">display name of the device</param>
        /// <returns>Returns the created device.</returns>
        public static Device CreateDevice(string dev_id, string user_id, string? display_name = null)
        {
            Device dev = new(dev_id, user_id, display_name);
            devices.Add(dev);
            return dev;
        }


        /// <summary>
        /// Gets an existing device.
        /// </summary>
        /// <param name="dev_id">id of the device</param>
        /// <param name="user_id">id of the user</param>
        /// <returns>Returns a device with the matching ids, if it exists.
        /// Returns null, if no match was found.</returns>
        public static Device? GetDevice(string dev_id, string user_id)
        {
            return devices.Find(dev => dev.device_id == dev_id && dev.user_id == user_id);
        }


        /// <summary>
        /// Gets all devices of a user.
        /// </summary>
        /// <param name="user_id">Matrix id of the user</param>
        /// <returns>Returns a list of all available devices.</returns>
        public static List<Device> GetDevicesOfUser(string user_id)
        {
            return devices.FindAll(dev => dev.user_id == user_id);
        }


        /// <summary>
        /// Removes a device with a given device id.
        /// </summary>
        /// <param name="dev_id">id of the device to remove</param>
        /// <param name="user_id">id of the user that the device belongs to</param>
        /// <returns>Returns true, if the device was removed.
        /// Returns false, if no match was found.</returns>
        public static bool Remove(string dev_id, string user_id)
        {
            return devices.RemoveAll(dev => dev.device_id == dev_id && dev.user_id == user_id) > 0;
        }
    }
}
