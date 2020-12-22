using System;

namespace DatabaseLayer
{
    /// <summary>
    ///     Miscelaneous functions
    /// </summary>
    class Helpers
    {
        /// <summary>
        ///     Converts a value to string
        /// </summary>
        /// <typeparam name="T"> Type of the value </typeparam>
        /// <param name="value"> Value to be converted </param>
        /// <returns> String representation of the value </returns>
        public static string StringifyValue<T>(T value)
        {
            if (value != null)
            {
                if (value is DateTime)
                    return $"'{ ((DateTime)Convert.ChangeType(value, typeof(DateTime))).ToString("yyyy-MM-dd hh:mm:ss") }'";

                if (value is string || value is char)
                    return $"'{value.ToString()}'";

                if (value is bool)
                    return ((bool)Convert.ChangeType(value, typeof(bool))) ? "'Y'" : "'N'";

                return value.ToString();
            }

            return "null";
        }

        /// <summary>
        ///     Converts a string/object to any other object
        ///     Special cases for DateTime/Bool
        /// </summary>
        /// <param name="obj"> Value to be converted </param>
        /// <param name="t"> Type to be converted to </param>
        /// <returns> Converted value </returns>
        public static object ConvertType(object obj, Type t)
        {
            t = Nullable.GetUnderlyingType(t) ?? t;

            if (t == typeof(DateTime) && obj.GetType() == typeof(string))
                return ((string)obj) != String.Empty ? DateTime.Parse((string)obj) : (DateTime?)null;

            if (t == typeof(bool) && obj.GetType() == typeof(string))
                return ((string)obj) == "Y" ? true : false;

            if (t != typeof(string) && obj.GetType() == typeof(string) && (string)obj == "")
                return null;

            return Convert.ChangeType(obj, t);
        }

        /// <summary>
        ///     Generic wrapper around the non-generic ConvertType
        /// </summary>
        /// <typeparam name="T"> Type to be converted to </typeparam>
        /// <param name="obj"> Value to be converted </param>
        /// <returns> Converted value type-casted to T </returns>
        public static T ConvertType<T>(object obj)
        {
            return (T)ConvertType(obj, typeof(T));
        }
    }
}
