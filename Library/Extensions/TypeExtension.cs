using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Extensions
{
    public static class TypeExtension
    {
        /// <summary>
        /// Determine if a <see cref="Type"/ is a number>
        /// source: https://stackoverflow.com/a/1750093
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumericType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determine if a <see cref="Type"/ is an Integer32>
        /// source: https://stackoverflow.com/a/1750093
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsIntegerType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.UInt32:
                case TypeCode.Int32:
                    return true;
                default:
                    return false;
            }
        }

    }
}
