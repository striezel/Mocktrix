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
    /// In-memory implementation of access token database.
    /// </summary>
    public class AccessTokens
    {
        /// <summary>
        /// in-memory token list
        /// </summary>
        private static readonly List<AccessToken> tokens = [];


        /// <summary>
        /// Creates a new access token for a specific user's device.
        /// If a token for that device already exists, then a new token is
        /// generated and the old token is invalidated.
        /// </summary>
        /// <param name="user_id">user id, e. g. "@alice:example.com"</param>
        /// <param name="dev_id">the device id</param>
        /// <returns>Returns the created access token.</returns>
        public static AccessToken CreateToken(string user_id, string dev_id)
        {
            AccessToken? old_token = GetToken(user_id, dev_id);
            if (old_token != null)
            {
                // Generate new token, invalidating older token.
                old_token.token = AccessToken.GenerateRandomToken();
                return old_token;
            }

            AccessToken token = new(user_id, dev_id, AccessToken.GenerateRandomToken());
            tokens.Add(token);
            return token;
        }


        /// <summary>
        /// Gets an existing token.
        /// </summary>
        /// <param name="user_id">id of the user</param>
        /// <param name="dev_id">id of the device</param>
        /// <returns>Returns a matching token, if it exists.
        /// Returns null, if no match was found.</returns>
        public static AccessToken? GetToken(string user_id, string dev_id)
        {
            return tokens.Find(element => element.user_id == user_id && element.device_id == dev_id);
        }
    }
}
