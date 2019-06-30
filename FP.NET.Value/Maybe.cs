using System;

namespace FP.NET.Value
{
    public struct Maybe<T> : IEquatable<Maybe<T>> where T : class
    {
        private readonly MaybeValueWrapper _value;
        public T Value
        {
            get
            {
                if (HasValue)
                    throw new InvalidOperationException();

                return _value.Value;
            }
        }

        public bool HasNoValue => !HasValue;
        public bool HasValue => _value != null;
        private Maybe(T value) => _value = value == null ? null : new MaybeValueWrapper(value);

        public static implicit operator Maybe<T>(T value)
        {
            if (value?.GetType() == typeof(Maybe<T>))
            {
                return (Maybe<T>)(object)value;
            }

            return new Maybe<T>(value);
        }

        public static bool operator ==(Maybe<T> maybe, T value)
        {
            if (value is Maybe<T>)
                return maybe.Equals(value);

            if (maybe.HasNoValue)
                return false;

            return maybe.Value.Equals(value);
        }

        public static bool operator !=(Maybe<T> maybe, T value)
        {
            return !(maybe == value);
        }

        public static bool operator ==(Maybe<T> first, Maybe<T> second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(Maybe<T> first, Maybe<T> second)
        {
            return !(first == second);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != typeof(Maybe<T>))
            {
                if (obj is T)
                {
                    obj = new Maybe<T>((T)obj);
                }

                if (!(obj is Maybe<T>))
                    return false;
            }

            var other = (Maybe<T>)obj;
            return Equals(other);
        }

        public bool Equals(Maybe<T> other)
        {
            if (HasNoValue && other.HasNoValue)
                return true;

            if (HasNoValue || other.HasNoValue)
                return false;

            return _value.Equals(other._value);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            if (HasNoValue)
                return string.Empty;

            return Value.ToString();
        }

        public T Unwrap(T defaultValue = default(T))
        {
            if (HasValue)
                return Value;

            return defaultValue;
        }

        [Serializable]
        private class MaybeValueWrapper
        {
            public MaybeValueWrapper(T value)
            {
                Value = value;
            }

            internal readonly T Value;
        }
    }
}