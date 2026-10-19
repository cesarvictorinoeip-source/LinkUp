using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace LinkUp.Core.Application.Helpers
{
    public static class SessionHelper
    {
        // Almacena un objeto en la sesión convirtiéndolo a JSON
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        // Obtiene un objeto de la sesión deserializando el JSON
        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}