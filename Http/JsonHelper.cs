using Newtonsoft.Json;

namespace Http {
    public class JsonHelper {
        public static string Serialize(object obj) => JsonConvert.SerializeObject(obj);

        public static T Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json);
    }
}