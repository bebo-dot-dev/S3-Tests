using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using FluentAssertions;
using NUnit.Framework;

namespace S3.Tests
{
    public class S3Tests : TestBase
    {
        [SetUp]
        public void SetUp()
        {
            ConfigureLogging();
        }
        
        [Test]
        public async Task PutObjectAsync_WhenUsingS3WithChecksumAlgorithmSHA256_ExpectChecksumSHA256Returned()
        {
            //arrange
            var request = new PutObjectRequest
            {
                BucketName = BucketName,
                Key = FileResourceName,
                InputStream = FileResourceStream,
                ContentType = "text/plain",
                ChecksumAlgorithm = ChecksumAlgorithm.SHA256
            };

            //act
            //valid AWS_ACCESS_KEY_ID and AWS_SECRET_ACCESS_KEY env vars must be set
            using var client = new AmazonS3Client(
                new EnvironmentVariablesAWSCredentials(),
                new AmazonS3Config
                {
                    RegionEndpoint = RegionEndpoint,
                    UseHttp = true,
                    ProxyHost = "127.0.0.1",
                    ProxyPort = 8866
                });
            
            var act = await client.PutObjectAsync(request);
            await TestContext.Out.WriteLineAsync("SHA-256 hash: " + act.ChecksumSHA256);
            
            //assert
            act.ChecksumSHA256.Should().Be("n4bQgYhMfWWaL+qgxVrQFaO/TxsrC4Is0V1sFbDwCgg=");
        }

        private static void ConfigureLogging()
        {
            AWSConfigs.LoggingConfig.LogTo = LoggingOptions.Console;
            AWSConfigs.LoggingConfig.LogResponses = ResponseLoggingOption.Always;
            AWSConfigs.LoggingConfig.LogMetricsCustomFormatter = new LogFormatter();
            AWSConfigs.LoggingConfig.LogMetrics = true;
        }

        private class LogFormatter : IMetricsFormatter
        {
            public string FormatMetrics(IRequestMetrics metrics)
            {
                return metrics.ToJSON();
            }
        }
    }
}