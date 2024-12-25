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

using System.Text.Json.Nodes;

namespace Mocktrix.Database.Memory.Tests
{
    public class ConfigDataTests
    {
        [Fact]
        public void CreateDatum()
        {
            const string user_id = "@alice:matrix.example.com";
            const string type = "foo.bar.baz";
            JsonNode? node = JsonNode.Parse("{\"a\": 1, \"b\": 2}");
            Assert.NotNull(node);
            
            var data = ConfigData.Create(user_id, type, node);

            Assert.NotNull(data);
            Assert.Equal(user_id, data.UserId);
            Assert.Equal(type, data.Type);
            Assert.Equal("{\"a\":1,\"b\":2}", data.Data.ToJsonString());
        }

        [Fact]
        public void GetDatum_NonExistentEntryNotFound()
        {
            const string user_id = "@alice:matrix.example.com";
            const string type = "does.not.exist";
            var entry = ConfigData.GetDatum(user_id, type);

            // Entry does not exist, function shall return null.
            Assert.Null(entry);
        }

        [Fact]
        public void GetDatum_ExistentEntry()
        {
            const string user_id = "@alice:matrix.example.com";
            const string type = "test.entry.exists";
            JsonNode? node = JsonNode.Parse("{\"c\": 33, \"d\": 42}");
            Assert.NotNull(node);
            // Create an entry.
            var entry_of_alice = ConfigData.Create(user_id, type, node);
            // Query the created entry.
            var entry = ConfigData.GetDatum(user_id, type);
            Assert.NotNull(entry);
            // Values of created entry and queried entry must match.
            Assert.Equal(entry_of_alice.UserId, entry.UserId);
            Assert.Equal(entry_of_alice.Type, entry.Type);
            Assert.Equal(entry_of_alice.Data.ToJsonString(), entry.Data.ToJsonString());
            // As a special property of this implementation, both objects refer
            // to the same instance.
            Assert.True(ReferenceEquals(entry, entry_of_alice));
        }

        [Fact]
        public void GetDatum_OnlyMatchingUser()
        {
            const string alice_id = "@alice:matrix.example.com";
            const string bob_id = "@bob:matrix.example.com";
            const string type = "test.entry.user-matching";
            JsonNode? node = JsonNode.Parse("{\"c\": 44, \"d\": 55}");
            Assert.NotNull(node);
            // Create an entry.
            _ = ConfigData.Create(alice_id, type, node);
            // Query the created entry for alice.
            var entry_of_alice = ConfigData.GetDatum(alice_id, type);
            Assert.NotNull(entry_of_alice);

            // Query the same entry type for bob.
            var entry_of_bob = ConfigData.GetDatum(bob_id, type);
            Assert.Null(entry_of_bob);
        }

        [Fact]
        public void GetAllConfigData_NonExistentEntryNotFound()
        {
            const string user_id = "@user_does_not_exist:matrix.example.com";
            var data = ConfigData.GetAllConfigData(user_id);

            // User (and thus config data) does not exist, function shall return empty list.
            Assert.NotNull(data);
            Assert.Empty(data);
        }

        [Fact]
        public void GetAllConfigData_ExistingEntriesFound()
        {
            const string user_id = "@test-user-id-config-data-all:matrix.example.com";
            const string type_one = "one.type.tld";
            const string type_two = "two.type.tld";
            JsonNode? node_one = JsonNode.Parse("{\"c\": 33, \"d\": 42}");
            Assert.NotNull(node_one);
            JsonNode? node_two = JsonNode.Parse("{\"two\": 2, \"data\": 42}");
            Assert.NotNull(node_two);

            ConfigData.Create(user_id, type_one, node_one);
            ConfigData.Create(user_id, type_two, node_two);

            var data = ConfigData.GetAllConfigData(user_id);

            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
            var one = data.Find(x => x.Type == type_one);
            Assert.NotNull(one);
            Assert.Equal(user_id, one.UserId);
            Assert.Equal("{\"c\":33,\"d\":42}", one.Data.ToJsonString());
            var two = data.Find(x => x.Type == type_two);
            Assert.NotNull(two);
            Assert.Equal(user_id, two.UserId);
            Assert.Equal("{\"two\":2,\"data\":42}", two.Data.ToJsonString());
        }

        [Fact]
        public void GetAllConfigData_OnlyEntriesOfGivenUserAreFound()
        {
            const string user_id = "@test-user-id-config-data-all-2:matrix.example.com";
            const string type_one = "one.type.tld";
            const string type_two = "two.type.tld";
            const string type_three = "three.type.tld";
            const string type_four = "four.type.tld";
            JsonNode? node = JsonNode.Parse("{\"c\": 33, \"d\": 42}");
            Assert.NotNull(node);
            
            const string user_id_other = "@some-other-test-id:matrix.example.com";

            ConfigData.Create(user_id, type_one, node);
            ConfigData.Create(user_id, type_two, node);
            ConfigData.Create(user_id_other, type_three, node);
            ConfigData.Create(user_id_other, type_four, node);

            // user one
            {
                var data = ConfigData.GetAllConfigData(user_id);

                Assert.NotNull(data);
                Assert.Equal(2, data.Count);
                var one = data.Find(x => x.Type == type_one);
                Assert.NotNull(one);
                Assert.Equal(user_id, one.UserId);

                var two = data.Find(x => x.Type == type_two);
                Assert.NotNull(two);
                Assert.Equal(user_id, two.UserId);
            }

            // other user
            {
                var data = ConfigData.GetAllConfigData(user_id_other);

                Assert.NotNull(data);
                Assert.Equal(2, data.Count);
                var three = data.Find(x => x.Type == type_three);
                Assert.NotNull(three);
                Assert.Equal(user_id_other, three.UserId);

                var four = data.Find(x => x.Type == type_four);
                Assert.NotNull(four);
                Assert.Equal(user_id_other, four.UserId);
            }
        }
    }
}
