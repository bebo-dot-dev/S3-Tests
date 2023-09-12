using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;

namespace TestApi;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHttpPolicyClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<S3HttpClientFactory>();
        
        var configSection = configuration.GetSection(PolicyOptions.SectionName);
        services.AddOptions<PolicyOptions>()
            .Bind(configSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.AddHttpClient("S3HttpClient")
            .AddPolicyHandler((serviceProvider, _) => 
                GetJitteredRetryPolicy(configSection.Get<PolicyOptions>(), serviceProvider));
        
        return services;
    }
    
    /// <summary>
    /// Returns a Polly jittered retry policy configured with the given <see cref="PolicyOptions"/> 
    /// </summary>
    /// <param name="options">The <see cref="PolicyOptions"/> used to configure the <see cref="IAsyncPolicy"/></param>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/></param>
    /// <returns><see cref="IAsyncPolicy{HttpResponseMessage}"/></returns>
    private static IAsyncPolicy<HttpResponseMessage> GetJitteredRetryPolicy(PolicyOptions options, IServiceProvider serviceProvider)
    {
        var jitteredDelays = Backoff.DecorrelatedJitterBackoffV2(
            medianFirstRetryDelay: TimeSpan.FromMilliseconds(options.JitterMsecs), 
            retryCount: options.RetryCount);
    
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(jitteredDelays, onRetryAsync: (response, delay, retryCount, context) =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<S3HttpClientFactory>>();
                logger.LogWarning($"Polly retry count: {retryCount}");
                return Task.CompletedTask;
            });
    }
}