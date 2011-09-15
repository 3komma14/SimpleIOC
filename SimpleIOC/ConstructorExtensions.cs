using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleIOC
{
    public static class ConstructorExtensions
    {
        /// <summary>
        /// Gets the param types for a constructor
        /// </summary>
        /// <param name="constructorInfo">The constructor handle</param>
        /// <returns>A list of the param types</returns>
        public static IEnumerable<Type> GetParamTypes(this ConstructorInfo constructorInfo)
        {
            var parameters = constructorInfo.GetParameters();
            return parameters.Select(parameterInfo => parameterInfo.ParameterType);
        }
    }
}
