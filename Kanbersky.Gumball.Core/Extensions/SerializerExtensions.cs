using Newtonsoft.Json;

namespace Kanbersky.Gumball.Core.Extensions
{
    public static class SerializerExtensions
    {
        public static string SerializeAsJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T DeserializeAs<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
