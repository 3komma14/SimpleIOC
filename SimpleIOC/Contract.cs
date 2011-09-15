using System;
using System.Linq;
using System.Collections.Generic;

namespace SimpleIOC
{
    public class Contract<TFor> : IContract where TFor : class
    {
        private IList<Type> ConstructorParams { get; set; }

        private Func<Container, TFor> _constructor;

        /// <summary>
        /// Setup the contract. Add the injected type
        /// </summary>
        /// <typeparam name="TUse">The type to inject</typeparam>
        /// <returns>The contract</returns>
        public Contract<TFor> Use<TUse>()
            where TUse : TFor
        {
            if (_constructor == null)
            {
                var constructorInfo = typeof (TUse).GetSingleConstructor();
                ConstructorParams = constructorInfo.GetParamTypes().ToList();

                _constructor = (container) => { return CreateInstance(container, typeof(TUse)) as TFor; };
            }
            return this;
        }

        /// <summary>
        /// Setup the contract. Add the constructor lambda for the injected type
        /// </summary>
        /// <typeparam name="TUse">The type to inject</typeparam>
        /// <param name="create">The construction expression</param>
        /// <returns>The contract</returns>
        public Contract<TFor> Use<TUse>(Func<Container, TFor> create)
            where TUse : TFor
        {
            _constructor = create;
            return this;
        }

        /// <summary>
        /// Setup the contract. Add the constructor lambda for the injected type
        /// </summary>
        /// <typeparam name="TUse">The type to inject</typeparam>
        /// <param name="create">The construction expression</param>
        /// <returns>The contract</returns>
        public Contract<TFor> Use<TUse>(Func<TFor> create)
            where TUse : TFor
        {
            return Use<TUse>(container => create.Invoke());
        }

        /// <summary>
        /// Setup the contract. Add the instance to inject
        /// </summary>
        /// <param name="instance">The instance to inject</param>
        /// <returns>The contract</returns>
        public Contract<TFor> Use(TFor instance)
        {
            _constructor = (container) => { return instance; };
            return this;
        }

        /// <summary>
        /// Resolve the contract
        /// </summary>
        /// <param name="container">The container to use for dependencies</param>
        /// <returns>a object</returns>
        public object Resolve(Container container)
        {
            return _constructor.Invoke(container);
        }

        /// <summary>
        /// Creates an instance of the InjectedType
        /// </summary>
        /// <param name="container">The container to use for dependencies</param>
        /// <param name="injectedType">The type to inject</param>
        /// <returns>a object</returns>
        private object CreateInstance(Container container, Type injectedType)
        {
            var resolvedParameters = container.Get(ConstructorParams).ToArray();
            return Activator.CreateInstance(injectedType, resolvedParameters);
        }

    }

}
