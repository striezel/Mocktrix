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

namespace Mocktrix.Database.Memory.Tests
{
    public class DevicesTests
    {
        [Fact]
        public void CreateDevice_WithDisplayName()
        {
            const string dev_id = "SomeRandomText";
            const string user_id = "@alice:matrix.example.com";
            const string display_name = "Not the tablet";
            var dev = Devices.CreateDevice(dev_id, user_id, display_name);

            Assert.NotNull(dev);
            Assert.Equal(dev_id, dev.device_id);
            Assert.Equal(user_id, dev.user_id);
            Assert.Equal(display_name, dev.display_name);
        }


        [Fact]
        public void CreateDevice_WithoutDisplayName()
        {
            const string dev_id = "SomeRandomText";
            const string user_id = "@alice:matrix.example.com";
            var dev = Devices.CreateDevice(dev_id, user_id);

            Assert.NotNull(dev);
            Assert.Equal(dev_id, dev.device_id);
            Assert.Equal(user_id, dev.user_id);
            Assert.Null(dev.display_name);
        }


        [Fact]
        public void GetDevice_NonExistentDeviceNotFound()
        {
            const string dev_id = "NotFoundHere";
            const string user_id = "@mango-eating_mungo:matrix.example.com";
            var device = Devices.GetDevice(dev_id, user_id);

            // Device does not exist, function shall return null.
            Assert.Null(device);
        }


        [Fact]
        public void GetDevice_ExistentDevice()
        {
            const string dev_id = "TheIdIsReallyHere";
            const string user_id = "@bob:matrix.example.com";
            const string name = "Bob's shiny device";
            
            // Create a device.
            var shiny_device = Devices.CreateDevice(dev_id, user_id, name);
            // Query the created device.
            var queried_device = Devices.GetDevice(dev_id, user_id);
            Assert.NotNull(queried_device);
            // Values of created device and queried device must match.
            Assert.Equal(shiny_device.device_id, queried_device.device_id);
            Assert.Equal(shiny_device.user_id, queried_device.user_id);
            Assert.Equal(shiny_device.display_name, queried_device.display_name);
        }


        [Fact]
        public void GetDevice_OneOfSeveralExistentDevices()
        {
            const string user_id = "@bob:matrix.example.com";

            // Create first device.
            var first_device = Devices.CreateDevice("SomeDeviceId", user_id, "Bob's old device");

            // Create second device.
            const string dev_id = "TheIdIsReallyHere";
            const string name = "Bob's shiny device";
            var second_device = Devices.CreateDevice(dev_id, user_id, name);
            // Query the created device.
            var device = Devices.GetDevice(dev_id, user_id);
            Assert.NotNull(device);
            // Values must not match old device.
            Assert.NotEqual(first_device.device_id, device.device_id);
            // Values of second device and queried device must match.
            Assert.Equal(second_device.device_id, device.device_id);
            Assert.Equal(second_device.user_id, device.user_id);
            Assert.Equal(second_device.display_name, device.display_name);
        }


        [Fact]
        public void GetDevice_WithWrongDeviceId()
        {
            const string user_id = "@alice:matrix.example.com";
            const string dev_id = "JustTheDeviceId";

            // Create device.
            _ = Devices.CreateDevice(dev_id, user_id, "Alice's test device");

            // Query the created device with wrong device id.
            const string wrong_dev_id = "NotTheDeviceId";
            var wrong_device = Devices.GetDevice(wrong_dev_id, user_id);
            // Device must be null, because users may have several different devices.
            Assert.Null(wrong_device);
        }


        [Fact]
        public void GetDevice_WithWrongUserId()
        {
            const string alice_id = "@alice:matrix.example.com";
            const string bob_id = "@bob:matrix.example.com";
            const string dev_id = "JustSomeDeviceId";

            // Create device.
            _ = Devices.CreateDevice(dev_id, bob_id, "Bob's random device");

            // Query the created device with wrong user id.
            var alice_device = Devices.GetDevice(dev_id, alice_id);
            // Device must be null, because other users may have the same device
            // id (device ids can be supplied by the user) by accident. However,
            // that is then a different device.
            Assert.Null(alice_device);
        }


        [Fact]
        public void GetDevicesOfUser_NonExistentUserNotFound()
        {
            const string user_id = "@mango-eating_mungo:matrix.example.com";
            var devices = Devices.GetDevicesOfUser(user_id);

            // User does not exist, function shall return empty list.
            Assert.NotNull(devices);
            Assert.Empty(devices);
        }


        [Fact]
        public void GetDevicesOfUser_ExistingUserFound()
        {
            const string alice_id = "@alice:matrix.example.com";
            const string bob_id = "@bob:matrix.example.com";
            const string dev_id = "DeviceIdForDevicesRetrievalTestCase";

            // Create device.
            _ = Devices.CreateDevice(dev_id, alice_id, "Alice's random device");
            _ = Devices.CreateDevice(dev_id, bob_id, "Bob's random device");
            var devices = Devices.GetDevicesOfUser(alice_id);

            Assert.NotNull(devices);
            Assert.NotEmpty(devices);
            // List must contain at least the one device from the mock data.
            var item = devices.Find(dev => dev.device_id == dev_id);
            Assert.NotNull(item);
            Assert.Equal(dev_id, item.device_id);
            Assert.Equal(alice_id, item.user_id);
            Assert.Equal("Alice's random device", item.display_name);
        }


        [Fact]
        public void Remove_NonExistentDeviceNotRemoved()
        {
            const string dev_id = "NotFoundHere";
            const string user_id = "@bob:matrix.example.com";
            // Device does not exist, function shall return false.
            Assert.False(Devices.Remove(dev_id, user_id));
        }


        [Fact]
        public void Remove_ExistentDevice()
        {
            const string dev_id = "TheDeviceWillBeRemovedHere";
            const string user_id = "@bob:matrix.example.com";
            const string name = "Bob's removable device";

            // Create a device.
            _ = Devices.CreateDevice(dev_id, user_id, name);
            // Query the created device.
            Assert.NotNull(Devices.GetDevice(dev_id, user_id));
            // Remove the device.
            Assert.True(Devices.Remove(dev_id, user_id));
            // Device shall not be found anymore.
            Assert.Null(Devices.GetDevice(dev_id, user_id));
        }


        [Fact]
        public void Remove_OneOfSeveralExistentDevices()
        {
            const string user_id = "@bob:matrix.example.com";

            // Create first device.
            var first_device = Devices.CreateDevice("NonRemovedDeviceId", user_id, "Bob's permanent device");

            // Create second device.
            const string dev_id = "IdOfDeviceToBeRemoved";
            const string name = "Bob's removed device";
            var second_device = Devices.CreateDevice(dev_id, user_id, name);
            // Query the created device - must exist.
            Assert.NotNull(Devices.GetDevice(dev_id, user_id));
            // Remove device - shall succeed.
            Assert.True(Devices.Remove(dev_id, second_device.user_id));
            // Removed device shall no longer be found.
            Assert.Null(Devices.GetDevice(dev_id, second_device.user_id));
            // But the first device must still be there.
            Assert.NotNull(Devices.GetDevice(first_device.device_id, first_device.user_id));
        }


        [Fact]
        public void Remove_DeviceWithWrongUserIdNotDeleted()
        {
            const string alice_id = "@alice:matrix.example.org";
            const string bob_id = "@bob:matrix.example.org";
            const string dev_id = "DeviceOfBobOrIsIt?";

            // Create Bob's device.
            var bob_device = Devices.CreateDevice(dev_id, bob_id, "Bob's permanent device");
            // Create second device with different user id.
            var alice_device = Devices.CreateDevice(dev_id, alice_id, "Alices's non-permanent device");
            // Remove device - shall succeed.
            Assert.True(Devices.Remove(dev_id, alice_id));
            // Removed device shall no longer be found.
            Assert.Null(Devices.GetDevice(dev_id, alice_device.user_id));
            // But the first device must still be there.
            Assert.NotNull(Devices.GetDevice(bob_device.device_id, bob_device.user_id));
        }
    }
}