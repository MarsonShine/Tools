using System.Linq;

namespace Http {
    public class QueryStringHelper {
        public static string ToQueryString(object obj) {
            if (obj == null) return "";
            var parameters = obj.GetType().GetProperties()
                .Where(p => null != p.GetValue(obj, null))
                .Select(p => p.Name + "=" + p.GetValue(obj, null));

            return string.Join('&', parameters);
        }
    }
}