using System;

namespace ATAP.Utilities.IJSON
{
    public interface IJSON
    {
        public string Serialize(object obj, Type T);
        public string Serialize<T>(T obj);
        public object Deserialize(string str, Type T);
        public T Deserialize<T>(string str);
    }
}
