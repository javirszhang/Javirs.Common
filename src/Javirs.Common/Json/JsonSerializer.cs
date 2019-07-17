using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Javirs.Common.Json
{
    /// <summary>
    /// Json serializer class
    /// </summary>
    public class JsonSerializer
    {
        /// <summary>
        /// use NewtonSoft.Json serialize an object to json data!
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string JsonSerialize(object arg)
        {
            var settings = new JsonSerializerSettings();
            if (settings.Converters == null)
            {
                settings.Converters = new List<Newtonsoft.Json.JsonConverter>();
            }
            IsoDateTimeConverter dateConverter = GetJsonDateFormat();
            settings.Converters.Add(dateConverter);
            return Newtonsoft.Json.JsonConvert.SerializeObject(arg, settings);
        }
        /// <summary>
        /// use Newtonsoft.Json to serialize object,specific serialize settings
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string JsonSerialize(object arg, Newtonsoft.Json.JsonSerializerSettings settings)
        {
            if (settings == null)
            {
                settings = new JsonSerializerSettings();
            }
            if (settings.Converters == null)
            {
                settings.Converters = new List<JsonConverter>();
            }
            var dateFormat = GetJsonDateFormat();
            if (!IsExist(settings.Converters, dateFormat))
            {
                settings.Converters.Add(dateFormat);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(arg, settings);
        }
        private static bool IsExist(IList<JsonConverter> list, JsonConverter converter)
        {
            if (list == null || list.Count <= 0 || converter == null)
            {
                return false;
            }
            bool isExist = false;
            foreach (JsonConverter jc in list)
            {
                if (jc.GetType() == converter.GetType())
                {
                    isExist = true;
                    break;
                }
            }
            return isExist;
        }
        private static IsoDateTimeConverter GetJsonDateFormat()
        {
            IsoDateTimeConverter dateConverter = new IsoDateTimeConverter();
            dateConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return dateConverter;
        }
        /// <summary>
        /// use NewtonSoft.Json deserialize a json data to an object!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserializer<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static T Deserializer<T>(string json, JsonSerializerSettings setting)
        {
            return JsonConvert.DeserializeObject<T>(json, setting);
        }
    }
}
