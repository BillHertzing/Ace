using System;
using System.Collections.Generic;
using System.Text;

namespace ATAP.Utilities.TypedGuids {

    //Attribution: taken from answers provided to this question: https://stackoverflow.com/questions/53748675/strongly-typed-guid-as-generic-struct
    public struct Id<T> : IEquatable<Id<T>> {
        private readonly Guid _value;
        public Id(string value) {
            var val = Guid.Parse(value);
            //CheckValue(val);
            _value=val;
        }

        public Id(Guid value) {
            //CheckValue(value);
            _value=value;
        }

        private static void CheckValue(Guid value) {
            if (value==Guid.Empty)
                throw new ArgumentException("Guid value cannot be empty", nameof(value));
        }

        public override bool Equals(object obj) {
            return obj is Id<T> id&&Equals(id);
        }

        public bool Equals(Id<T> other) {
            return _value.Equals(other._value);
        }

        public override int GetHashCode() {
            return -1939223833+EqualityComparer<Guid>.Default.GetHashCode(_value);
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
}
