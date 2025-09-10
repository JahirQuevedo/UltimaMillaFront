using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ALOG.Modelos.Utilerias
{
    /*
     Clase auxiliar cuya finalidad es eliminar los nodos $id y $values de las respuestas JSON 
    y así tener unos modelos más limpios y reutillizabls
     */
    public class JsonConvertCustom<T> : JsonConverter
    {

        public override bool CanConvert(Type objectType) => objectType == typeof(T);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Cargar el JSON como un JObject para manipularlo
            var jObject = JObject.Load(reader);

            // Eliminar las propiedades no deseadas como $id y $values
            jObject.Remove("$id");
            JArray valuesArray = (JArray)jObject["$values"];
            if (valuesArray != null)
            {
                // Reemplazar $values con el contenido del array si existe
                return valuesArray.ToObject<List<T>>();
            }

            // Si no es un array con $values, deserializar el objeto directamente
            return jObject.ToObject<T>();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Serialización estándar
            serializer.Serialize(writer, value);
        }


    }
}
