using System;
using System.Reflection;

namespace SimpleIOC
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Get the single constructor or throw an exception
        /// </summary>
        /// <param name="type">The type to get constructor for</param>
        /// <returns>A ConstructorInfo or throws</returns>
        public static ConstructorInfo GetSingleConstructor(this Type type)
        {
            var constructors = type.GetConstructors();
            if (constructors.Length != 1)
            {
                throw new InvalidOperationException(string.Format("Multiple constuctors found for {0}", type.Name));
            }

            return constructors[0];
        }
    }
}
