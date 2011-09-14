namespace SimpleIOC
{
    public interface IDependencyItem
    {
        object Resolve(Container container);
    }
}
