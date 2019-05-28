using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATAP.Utilities.TypedGuids {

    //Attribution: taken from answers provided to this question: https://stackoverflow.com/questions/53748675/strongly-typed-guid-as-generic-struct
    // Modifications: CheckValue and all references removed, because our use case requries Guid.Empty to be a valid value
    public struct Id<T> : IEquatable<Id<T>> {
        private readonly Guid _value;

            public Id(string value) {
            bool success;
            Guid newValue ;
            string iValue;
            if (string.IsNullOrEmpty(value)) {
                _value=Guid.NewGuid();
            } else {
                // Hack, used becasue only ServiceStack Json serializers add extra enclosing ".
                //  but, neither simpleJson nor netwtonsoft will serialze this at all
                iValue=value.Trim('"');
                success=Guid.TryParse(iValue, out newValue);
                if (!success) { throw new NotSupportedException($"Guid.TryParse failed,, newValue {value} cannot be parsed as a GUID"); }
                _value=newValue;
            }
        }

        public Id(Guid value) {
            _value=value;
        }

        public override bool Equals(object obj) {
            return obj is Id<T> id&&Equals(id);
        }

        public bool Equals(Id<T> other) {
            return _value.Equals(other._value);
        }

        public override int GetHashCode() {
            return _value.GetHashCode();
        }

        public override string ToString() {
            return _value.ToString();
        }

        public static bool operator ==(Id<T> left, Id<T> right) {
            return left.Equals(right);
        }

        public static bool operator !=(Id<T> left, Id<T> right) {
            return !(left==right);
        }
    }
    /*
        public class ResultConverter<T> : JsonConverter {
        public override bool CanWrite => false;
        public override bool CanRead => true;
        public override bool CanConvert(Type objectType) {
            return objectType==typeof(Id<T>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            var jsonObject = JObject.Load(reader);

if(System.Diagnostics.Debugger.IsAttached)
  System.Diagnostics.Debugger.Break();
            Id<T> result = new Id<T> {
                //_value=jsonObject["_value"].Value();

            };
            return result;
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            throw new InvalidOperationException("Use default serialization.");
        }
    }
    */
}