using System;

namespace SimpleIOC
{
    public interface IContract
    {
        object Resolve(Container container);
    }
}
