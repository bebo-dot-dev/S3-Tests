using System.Reflection;
using System.Text;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace TestApi;

public class S3Gateway
{
    private const string BucketName = "test-bucket-unique";

    private readonly IConfiguration _configuration;
    private readonly string _fileResourceName;
    private readonly Stream _fileResourceStream;
    private readonly S3HttpClientFactory _httpClientFactory;

    public S3Gateway(IConfiguration configuration, S3HttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        
        _fileResourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames().First();
        _fileResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_fileResourceName)!;
    }
    
    public async Task<string> PutAndGetFileAsync()
    {
        using var client = GetClient();
        await EnsureBucketAsync(client);
        await PutObjectAsync(client);
        await using var stream = await GetObjectStreamAsync(client);
        return await ConvertStreamToFileContentAsync(stream);
    }

    private AmazonS3Client GetClient()
    {
        return new AmazonS3Client(
            new BasicAWSCredentials(_configuration["MINIO_ROOT_USER"], _configuration["MINIO_ROOT_PASSWORD"]),
            new AmazonS3Config
            {
                UseHttp = true, //plain old http
                ServiceURL = _configuration["MINIO_URL"], //target the request at the test container MinIO instance
                ForcePathStyle = true, //using path style addressing for MinIO
                HttpClientFactory = _httpClientFactory,
                MaxErrorRetry = 0 //disable AWS SDK retries if using Polly
            });
    }

    private static async Task EnsureBucketAsync(IAmazonS3 client)
    {
        var bucketsResponse = await client.ListBucketsAsync();
        if (bucketsResponse.Buckets.Count(b => b.BucketName == BucketName) == 0)
        {
            await client.PutBucketAsync(BucketName);    
        }
    }

    private async Task PutObjectAsync(IAmazonS3 client)
    {
        var putRequest = new PutObjectRequest
        {
            BucketName = BucketName,
            Key = _fileResourceName,
            InputStream = _fileResourceStream,
            ContentType = "text/plain",
            ChecksumAlgorithm = ChecksumAlgorithm.SHA256
        };
        await client.PutObjectAsync(putRequest);
    }

    private async Task<Stream> GetObjectStreamAsync(IAmazonS3 client)
    {
        var getRequest = new GetObjectRequest
        {
            BucketName = BucketName,
            Key = _fileResourceName
        };
        
        var getResponse = await client.GetObjectAsync(getRequest);
        return getResponse.ResponseStream;
    }

    private static async Task<string> ConvertStreamToFileContentAsync(Stream stream)
    {
        var memStream = new MemoryStream();
        await stream.CopyToAsync(memStream);
        return Encoding.UTF8.GetString(memStream.ToArray());   
    }
}