namespace Microsoft.Content.Build.MerfToSDPForJava.Common
{
    using Microsoft.CSharp.RuntimeBinder;
    using System;
    public static class TypeUtility
    {
        /// <summary>
        /// Change type of value, support IConvertible type change, implicit/explicit cast and parse Enum from string 
        /// </summary>
        /// <typeparam name="T">Type to be changed to</typeparam>
        /// <param name="value">Value to be changed</param>
        /// <returns>Value with specified type</returns>
        public static T ChangeTypeLoose<T>(object value)
        {
            try
            {
                // To those who implement IConvertible
                return ChangeTypeWithNullable<T>(value);
            }
            catch (InvalidCastException)
            {
                if (value == null)
                {
                    throw;
                }

                try
                {
                    // To those who implement implicit/explicit cast
                    return (T)(dynamic)value;
                }
                catch (RuntimeBinderException re)
                {
                    if (typeof(T).IsEnum)
                    {
                        // Enum support
                        try
                        {
                            return (T)Enum.Parse(typeof(T), value.ToString(), true);
                        }
                        catch (ArgumentException e)
                        {
                            throw new InvalidCastException(e.Message, e);
                        }
                    }

                    throw new InvalidCastException(re.Message, re);
                }
            }
        }

        public static T ChangeTypeWithNullable<T>(object value)
        {
            // Since Convert.ChangeType can't correctly handle Nullable type, when T is Nullable type and retVal is null, return directly, 
            // See http://stackoverflow.com/questions/18015425/invalid-cast-from-system-int32-to-system-nullable1system-int32-mscorlib
            var t = typeof(T);
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (value == null)
                {
                    return default(T);
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return (T)Convert.ChangeType(value, t);
        }
    }
}
