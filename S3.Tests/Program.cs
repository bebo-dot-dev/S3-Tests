using System.Reflection;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

ConfigureLogging();

const string bucketName = "test-bucket-unique-unique"; //set to a valid S3 bucket name
var regionEndpoint = RegionEndpoint.EUWest1; //set to the region where the S3 bucket exists

var fileResourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames().First();
await using var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fileResourceName);

var request = new PutObjectRequest
{
    BucketName = bucketName,
    Key = fileResourceName,
    InputStream = resourceStream,
    ContentType = "text/plain",
    ChecksumAlgorithm = ChecksumAlgorithm.SHA256
};

//valid AWS_ACCESS_KEY_ID and AWS_SECRET_ACCESS_KEY env vars must be set
using var client = new AmazonS3Client(
    new EnvironmentVariablesAWSCredentials(),
    regionEndpoint);
var response = await client.PutObjectAsync(request);

Console.WriteLine("SHA-256 hash: {0}", response.ChecksumSHA256);

void ConfigureLogging()
{
    AWSConfigs.LoggingConfig.LogTo = LoggingOptions.Console;
    AWSConfigs.LoggingConfig.LogResponses = ResponseLoggingOption.Always;
    AWSConfigs.LoggingConfig.LogMetricsCustomFormatter = new LogFormatter();
    AWSConfigs.LoggingConfig.LogMetrics = true;
}

internal class LogFormatter : IMetricsFormatter
{
    public string FormatMetrics(IRequestMetrics metrics)
    {
        return metrics.ToJSON();
    }
}