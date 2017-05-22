using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FakePoll.Helpers
{
    public static class JsonHelpers
    {
        public static Task<T> DeserializeObjectAsync<T>(string value)
        {
            return Task.Run(() => JsonConvert.DeserializeObject<T>(value));
        }
        public static Task<object> DeserializeObjectAsync(string value)
        {
            return Task.Run(() => JsonConvert.DeserializeObject(value));
        }

        public static Task<JObject> DeserializeJObjectAsync(string value)
        {
            return Task.Run(() => JObject.Parse(value));
        }
    }
}