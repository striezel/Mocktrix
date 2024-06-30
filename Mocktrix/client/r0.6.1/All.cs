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

namespace Mocktrix.client.r0_6_1
{
    /// <summary>
    /// Provides convenience methods for use of the client-server API.
    /// </summary>
    public static class All
    {
        /// <summary>
        /// Adds all implemented endpoints of this version of the client-server
        /// API to the web application.
        /// </summary>
        /// <param name="app">the app to which the endpoint shall be added</param>
        public static void AddEndpoints(WebApplication app)
        {
            Versions.AddEndpoints(app);
            ServerDiscovery.AddEndpoints(app);
            Login.AddEndpoints(app);
            Account.AddEndpoints(app);
            Capabilities.AddEndpoints(app);
            Registration.AddEndpoints(app);
            DeviceManagement.AddEndpoints(app);
            Media.AddEndpoints(app);
            Profile.AddEndpoints(app);
            Syncing.AddEndpoints(app);
        }
    }
}
