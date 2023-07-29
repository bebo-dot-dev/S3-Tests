using System.Reflection;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using FluentAssertions;
using NUnit.Framework;

namespace S3.Tests
{
    public class S3Tests
    {
        private const string BucketName = "test-bucket-unique-unique"; //set to a valid S3 bucket name
        private static readonly RegionEndpoint RegionEndpoint = RegionEndpoint.EUWest1; //set to the region where the S3 bucket exists

        private string _fileResourceName = null!;
        private Stream _fileResourceStream = null!;
        
        [SetUp]
        public void SetUp()
        {
            _fileResourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames().First();
            _fileResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_fileResourceName)!;
            
            ConfigureLogging();
        }
        
        [Test]
        public async Task PutObjectAsync_WhenUsingS3WithChecksumAlgorithmSHA256_ExpectChecksumSHA256Returned()
        {
            //arrange
            var request = new PutObjectRequest
            {
                BucketName = BucketName,
                Key = _fileResourceName,
                InputStream = _fileResourceStream,
                ContentType = "text/plain",
                ChecksumAlgorithm = ChecksumAlgorithm.SHA256
            };

            //act
            //valid AWS_ACCESS_KEY_ID and AWS_SECRET_ACCESS_KEY env vars must be set
            using var client = new AmazonS3Client(
                new EnvironmentVariablesAWSCredentials(),
                RegionEndpoint);
            
            var act = await client.PutObjectAsync(request);
            await TestContext.Out.WriteLineAsync("SHA-256 hash: " + act.ChecksumSHA256);
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