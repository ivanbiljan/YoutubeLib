using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using YoutubeLib.Videos;

namespace YoutubeLib.JsonConverters
{
    internal sealed class VideoConverter : JsonConverter
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => false;

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType) => objectType == typeof(Video);
    }
}
