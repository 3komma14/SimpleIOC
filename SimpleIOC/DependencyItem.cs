using System;
using System.Collections.Generic;

namespace SimpleIOC
{
    public class DependencyItem<TFor> : IDependencyItem where TFor : class
    {

        public DependencyItem<TFor> Use<TUse>()
            where TUse : TFor
        {
            InjectedType = typeof(TUse);
            if (Constructor == null)
            {
                var constructors = typeof(TUse).GetConstructors();
                if (constructors.Length != 1)
                {
                    throw new InvalidOperationException("Unable to select constructor. Please choose one");
                }

                var constructorInfo = constructors[0];
                var parameters = constructorInfo.GetParameters();
                ConstructorParams = new List<Type>(parameters.Length);
                foreach (var parameterInfo in parameters)
                {
                    ConstructorParams.Add(parameterInfo.ParameterType);
                }

                Constructor = (container) => { return CreateInstance(container) as TFor; };
            }
            return this;
        }

        public DependencyItem<TFor> Use<TUse>(Func<Container, TFor> create)
            where TUse : TFor
        {
            InjectedType = typeof(TUse);
            Constructor = create;
            return this;
        }

        public DependencyItem<TFor> Use<TUse>(Func<TFor> create)
            where TUse : TFor
        {
            return Use<TUse>(container => create.Invoke());
        }

        private Type InjectedType { get; set; }

        private List<Type> ConstructorParams { get; set; }

        private Func<Container, TFor> Constructor;

        public object Resolve(Container container)
        {
            return Constructor.Invoke(container);
        }

        private object CreateInstance(Container container)
        {
            var parameters = GetActivationParams(container);
            return Activator.CreateInstance(InjectedType, parameters);
        }

        private object[] GetActivationParams(Container container)
        {
            var paramaters = new List<object>(ConstructorParams.Count);
            foreach (var parameter in ConstructorParams)
            {
                paramaters.Add(container.Get(parameter));
            }
            return paramaters.ToArray();
        }

    }

}
