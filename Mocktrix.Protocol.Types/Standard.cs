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

namespace Mocktrix.Protocol.Types
{
    /// <summary>
    /// The "standard error response" type from the Matrix specification.
    /// </summary>
    public class ErrorResponse
    {
#pragma warning disable IDE1006 // naming style
        /// <summary>
        /// Unique string used in handling the error, e. g. "M_FORBIDDEN".
        /// </summary>

        public required string errcode { get; set; }


        /// <summary>
        /// Human-readable error message.
        /// </summary>
        public required string error { get; set; }
#pragma warning restore IDE1006 // naming style
    }
}
