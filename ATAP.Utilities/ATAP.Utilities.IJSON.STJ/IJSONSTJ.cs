using System;

namespace ATAP.Utilities.IJSON.STJ
{
    public class IJSONSTJ : IJSON
    {
        public string Serialize(object obj, Type T)
        {
            return System.Text.Json.JsonSerializer.Serialize(obj, T);
        }
        public string Serialize<T>(T obj)
        {
            return System.Text.Json.JsonSerializer.Serialize<T>(obj);
        }
        public object Deserialize(string str, Type T)
        {
            return System.Text.Json.JsonSerializer.Deserialize(str, T);
        }
        public T Deserialize<T>(string str)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(str);
        }
    }
}
