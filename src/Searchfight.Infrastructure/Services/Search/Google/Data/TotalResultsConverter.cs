using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Searchfight.Infrastructure.Services.Search.Google.Data
{
    internal class TotalResultsConverter : JsonConverter<ulong>
    {
        public override ulong Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) 
            => ulong.Parse(reader.GetString());

        public override void Write(Utf8JsonWriter writer, ulong value, JsonSerializerOptions options) 
            => value.ToString();
    }
}