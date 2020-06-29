namespace Embedlur
{
    public interface IProviderResolver
    {
        IProvider Resolve(string url);

        IProvider ResolveByName(string name);
    }
}
