/*
    This file is part of Mocktrix.
    Copyright (C) 2026  Dirk Stolle

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

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mocktrix.Events
{
    /// <summary>
    /// Custom converter for Matrix event classes.
    /// 
    /// This class is employed as a workaround for a breaking change in .NET 10:
    /// https://learn.microsoft.com/en-us/dotnet/core/compatibility/serialization/10/property-name-validation
    /// Before that, the JsonPolymorphic and JsonDerivedType annotations would
    /// handle events properly, but now there's a naming conflict when using
    /// "type" as type discriminator. Sad.
    /// </summary>
    public class EventJsonConverter: JsonConverter<IEvent>
    {
        public override IEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument document = JsonDocument.ParseValue(ref reader);
            JsonElement root = document.RootElement;

            string? type = root.GetProperty("type").GetString();
            IEvent? ev = type switch
            {
                "m.call.answer" => JsonSerializer.Deserialize<VoIP.CallAnswerEvent>(root.GetRawText(), options),
                "m.call.candidates" => JsonSerializer.Deserialize<VoIP.CallCandidatesEvent>(root.GetRawText(), options),
                "m.call.hangup" => JsonSerializer.Deserialize<VoIP.CallHangUpEvent>(root.GetRawText(), options),
                "m.call.invite" => JsonSerializer.Deserialize<VoIP.CallInviteEvent>(root.GetRawText(), options),
                "m.room.aliases" => JsonSerializer.Deserialize<AliasesEvent>(root.GetRawText(), options),
                "m.room.avatar" => JsonSerializer.Deserialize<RoomAvatarEvent>(root.GetRawText(), options),
                "m.room.canonical_alias" => JsonSerializer.Deserialize<CanonicalAliasEvent>(root.GetRawText(), options),
                "m.room.create" => JsonSerializer.Deserialize<CreateRoomEvent>(root.GetRawText(), options),
                "m.room.encryption" => JsonSerializer.Deserialize<EncryptionEvent>(root.GetRawText(), options),
                "m.room.guest_access" => JsonSerializer.Deserialize<GuestAccessEvent>(root.GetRawText(), options),
                "m.room.history_visibility" => JsonSerializer.Deserialize<HistoryVisibilityEvent>(root.GetRawText(), options),
                "m.room.join_rules" => JsonSerializer.Deserialize<JoinRulesEvent>(root.GetRawText(), options),
                "m.room.member" => JsonSerializer.Deserialize<MembershipEvent>(root.GetRawText(), options),
                "m.room.message.feedback" => JsonSerializer.Deserialize<FeedbackEvent>(root.GetRawText(), options),
                "m.room.name" => JsonSerializer.Deserialize<NameEvent>(root.GetRawText(), options),
                "m.room.pinned_events" => JsonSerializer.Deserialize<PinnedEventsEvent>(root.GetRawText(), options),
                "m.room.power_levels" => JsonSerializer.Deserialize<PowerLevelsEvent>(root.GetRawText(), options),
                "m.room.topic" => JsonSerializer.Deserialize<TopicEvent>(root.GetRawText(), options),
                "m.tag" => JsonSerializer.Deserialize<TagEvent>(root.GetRawText(), options),
                // If there's no match, then this could still be account config data.
                _ => JsonSerializer.Deserialize<ConfigDataEvent>(root.GetRawText(), options),
                //_ => throw new NotImplementedException("Deserialization is not implemented for event type '" + type + "'!"),
            };

            return ev!;
        }

        public override void Write(Utf8JsonWriter writer, IEvent value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
