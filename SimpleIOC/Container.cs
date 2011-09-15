using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleIOC
{
    public class Container
    {
        /// <summary>
        /// The container items
        /// </summary>
        private readonly Dictionary<Type, IContract> _items = new Dictionary<Type, IContract>();

        /// <summary>
        /// Setup a contract for a type
        /// </summary>
        /// <typeparam name="TFor">The type to setup</typeparam>
        /// <returns>a contract for the type</returns>
        public Contract<TFor> For<TFor>() where TFor : class
        {
            var item = new Contract<TFor>();
            _items.Add(typeof(TFor), item);
            return item;
        }

        /// <summary>
        /// Resolves a type
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <returns>a object </returns>
        public T Get<T>() where T : class
        {
            return Get(typeof(T)) as T;
        }

        /// <summary>
        /// Resolves a type
        /// </summary>
        /// <param name="type">The type to resolve</param>
        /// <returns>a object</returns>
        public object Get(Type type)
        {
            // Get from the container
            IContract item;
            if (_items.TryGetValue(type, out item))
            {
                return Resolve(item); 
            }

            // It's a valuetype (string, int...)
            if (type.IsValueType)
            {
                // Use the default value
                return Activator.CreateInstance(type);
            }

            // It's a concrete class
            if (!type.IsAbstract || !type.IsInterface)
            {
                var ctor = type.GetSingleConstructor();
                var ctorParamTypes = ctor.GetParamTypes();
                var resolvedCtorParams = Get(ctorParamTypes).ToArray();
                return Activator.CreateInstance(type, resolvedCtorParams);
            }

            // This sucks, nothing found
            throw new InvalidOperationException(string.Format("No class found for {0}", type.Name));
        }

        /// <summary>
        /// Resolves a list of types
        /// </summary>
        /// <param name="types">The types to resolve</param>
        /// <returns>a list of resolved objects</returns>
        public IEnumerable<object> Get(IEnumerable<Type> types)
        {
            return types.Select(Get);
        }

        /// <summary>
        /// Resolve a contract
        /// </summary>
        /// <param name="contract">The contract to resolve</param>
        /// <returns>a object or null</returns>
        private object Resolve(IContract contract)
        {
            return contract != null ? contract.Resolve(this) : null;
        }
    }

}
