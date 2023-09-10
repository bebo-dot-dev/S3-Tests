using Amazon.Runtime;

namespace TestApi;

/// <summary>
/// The delegated Amazon.Runtime.HttpClientFactory passed to AmazonS3Config.HttpClientFactory
/// to enable a named client with a Polly retry policy handler
/// </summary>
public class S3HttpClientFactory : HttpClientFactory
{
    private readonly IHttpClientFactory _factory;
    
    public S3HttpClientFactory(IHttpClientFactory factory)
    {
        _factory = factory;
    }
    
    public override HttpClient CreateHttpClient(IClientConfig clientConfig)
    {
        return _factory.CreateClient("S3HttpClient");
    }
}