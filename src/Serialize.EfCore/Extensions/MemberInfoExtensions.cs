﻿
using System;
using System.Reflection;

namespace Serialize.EfCore.Extensions
{
    /// <summary>
    /// MemberInfo extensions methods.
    /// </summary>
    internal static class MemberInfoExtensions
    {
        /// <summary>
        /// Gets the return type of an member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException">Unable to get return type of member of type  + member.MemberType</exception>
        public static Type GetReturnType(this MemberInfo member)
        {
            switch (member)
            {
                case PropertyInfo propertyInfo:
                    return propertyInfo.PropertyType;
                case MethodInfo methodInfo:
                    return methodInfo.ReturnType;
                case FieldInfo fieldInfo:
                    return fieldInfo.FieldType;
            }

            throw new NotSupportedException("Unable to get return type of member of type " + member.GetType().Name);
        }
    }
}