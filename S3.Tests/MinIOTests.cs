using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using FluentAssertions;
using NUnit.Framework;
using Testcontainers.Minio;

namespace S3.Tests
{
    public class MinIOTests : TestBase
    {
        private const string MinioUsername = "minio_test";
        private const string MinioPassword = "minio_test";
        private const string MinioImage = "minio/minio:RELEASE.2023-07-21T21-12-44Z"; //MinIO 21/07/2023 latest
        private MinioContainer _minioContainer = null!;
            
        [SetUp]
        public async Task SetUp()
        {
            Environment.SetEnvironmentVariable("MINIO_ROOT_USER", MinioUsername);
            Environment.SetEnvironmentVariable("MINIO_ROOT_PASSWORD", MinioPassword);
            
            _minioContainer = new MinioBuilder()
                .WithImage(MinioImage)
                .WithUsername(MinioUsername)
                .WithPassword(MinioPassword)
                .Build();

            await _minioContainer.StartAsync();
        }
        
        [Test]
        public async Task PutObjectAsync_WhenUsingMinIOWithChecksumAlgorithmSHA256_ExpectFailure()
        {
            //arrange
            var client = new AmazonS3Client(
                new BasicAWSCredentials(_minioContainer.GetAccessKey(), _minioContainer.GetSecretKey()),
                new AmazonS3Config
                {
                    UseHttp = true, //plain old http
                    ServiceURL = _minioContainer.GetConnectionString(), //target the request at the test container MinIO instance
                    ForcePathStyle = true, //using path style addressing for MinIO
                    ProxyHost = "127.0.0.1", //route through local proxy to capture the request/response
                    ProxyPort = 8866
                });
            await client.PutBucketAsync(BucketName);
            
            var request = new PutObjectRequest
            {
                BucketName = BucketName,
                Key = FileResourceName,
                InputStream = FileResourceStream,
                ContentType = "text/plain",
                ChecksumAlgorithm = ChecksumAlgorithm.SHA256
            };

            //act
            Func<Task> act = async () => await client.PutObjectAsync(request);
            
            //assert
            await act.Should()
                .ThrowAsync<AmazonS3Exception>()
                .WithMessage("The request signature we calculated does not match the signature you provided. Check your key and signing method.");
        }
    }
}