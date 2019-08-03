using System;
using ServiceStack.Text;

namespace ATAP.Utilities.IJSON.SS
{
    public class IJSONSS : ATAP.Utilities.IJSON.IJSON
    {
        public string Serialize(object obj, Type T)
        {
            return ServiceStack.Text.JsonSerializer.SerializeToString(obj, T);
        }
        public string Serialize<T>(T obj)
        {
            return ServiceStack.Text.JsonSerializer.SerializeToString<T>(obj);
        }
        public object Deserialize(string str, Type T)
        {
            return ServiceStack.Text.JsonSerializer.DeserializeFromString(str, T);
        }
        public T Deserialize<T>(string str)
        {
            return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(str);
        }
    }
}
