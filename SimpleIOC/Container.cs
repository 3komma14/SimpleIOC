using System;
using System.Collections.Generic;

namespace SimpleIOC
{
    public class Container
    {
        private readonly Dictionary<Type, IDependencyItem> items = new Dictionary<Type, IDependencyItem>();

        public DependencyItem<TFor> For<TFor>() where TFor : class
        {
            var item = new DependencyItem<TFor>();
            items.Add(typeof(TFor), item);
            return item;
        }

        public T Get<T>() where T : class
        {
            return Get(typeof(T)) as T;
        }

        public object Get(Type type)
        {
            if (type.IsValueType)
            {
                // Use the default value
                return Activator.CreateInstance(type);
            }

            IDependencyItem item;
            if (!items.TryGetValue(type, out item))
            {
                throw new InvalidOperationException(string.Format("No class found for {0}", type.Name));
            }

            return item.Resolve(this);

        }

    }

}
