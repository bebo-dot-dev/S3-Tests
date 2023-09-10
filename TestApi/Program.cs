using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using TestApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<S3Gateway>();
builder.Services.AddScoped<S3HttpClientFactory>();

builder.Services.AddHttpClient("S3HttpClient")
    .AddPolicyHandler(GetJitteredRetryPolicy());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();

//Returns a Polly jittered retry policy
static IAsyncPolicy<HttpResponseMessage> GetJitteredRetryPolicy()
{
    var jitteredDelays = Backoff.DecorrelatedJitterBackoffV2(
        medianFirstRetryDelay: TimeSpan.FromSeconds(1), 
        retryCount: 5,
        seed: 100);
    
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(jitteredDelays);
}

public partial class Program { } //WebApplicationFactory test support