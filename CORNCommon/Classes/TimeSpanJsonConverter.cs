using System;
using System.Collections;
using System.Xml;
using Newtonsoft.Json;

namespace CORNCommon.Classes
{
    public class TimeSpanJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var time = (TimeSpan?)value;
            if (time.HasValue)
            {
                serializer.Serialize(writer, XmlConvert.ToString(time.Value));
            }
        }


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            try
            {
                if (existingValue is string)
                {
                    string xmlTime = Convert.ToString(existingValue);
                    if (xmlTime.Contains("PT"))
                        return XmlConvert.ToTimeSpan(xmlTime);
                }
            }
            catch (Exception)
            {
            }
            return existingValue;

            //JToken jt = JToken.ReadFrom(reader);
            //return jt.V
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(TimeSpan) == objectType || typeof(TimeSpan?) == objectType;
        }
    }
}
