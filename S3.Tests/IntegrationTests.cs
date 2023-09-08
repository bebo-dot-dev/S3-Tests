using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using Testcontainers.Minio;

namespace S3.Tests;

public class IntegrationTests
{
    private const string MinioUsername = "minio_test";
    private const string MinioPassword = "minio_test";
    private const string MinioImage = "minio/minio:RELEASE.2023-09-07T02-05-02Z";

    private MinioContainer _minioContainer = null!;
    private WebApplicationFactory<Program> _application = null!;
    
    [SetUp]
    public async Task SetUp()
    {
        _minioContainer = new MinioBuilder()
            .WithImage(MinioImage)
            .WithUsername(MinioUsername)
            .WithPassword(MinioPassword)
            .Build();
        
        await _minioContainer.StartAsync();
        
        _application = new WebApplicationFactory<Program>().WithWebHostBuilder(b =>
        {
            b.UseSetting("MINIO_ROOT_USER", MinioUsername);
            b.UseSetting("MINIO_ROOT_PASSWORD", MinioPassword);
            b.UseSetting("MINIO_URL", _minioContainer.GetConnectionString());
        });
        
    }

    [TearDown]
    public async Task TearDown()
    {
        await _minioContainer.StopAsync();
        await _minioContainer.DisposeAsync();
        await _application.DisposeAsync();
    }

    [Test]
    public async Task BasicTest()
    {
        var client = _application.CreateClient();
        client.BaseAddress = new Uri("http://localhost:5129");
        var responseMessage = await client.GetAsync("/Test/DoS3");
        var response = await responseMessage.Content.ReadAsStringAsync();

        response.Should().Be("API MinIO test file response");
    }
}