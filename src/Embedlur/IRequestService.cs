namespace Embedlur
{
    public interface IRequestService
    {
        string Get(string url, string contentType = "application/json");
    }
}
