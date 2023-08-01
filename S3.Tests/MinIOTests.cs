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
        private const string LatestMinioImage = "minio/minio:RELEASE.2023-07-21T21-12-44Z"; //MinIO 21/07/2023 latest
        private const string LatestMinioImageErrorMessage = "The request signature we calculated does not match the signature you provided. Check your key and signing method.";
        private const string OlderMinioImage = "minio/minio:RELEASE.2023-04-13T03-08-07Z.fips"; //MinIO 13/04/2023 older
        private const string OlderMinioImageErrorMessage = "The provided 'x-amz-content-sha256' header does not match what was computed.";
        
        [SetUp]
        public void SetUp()
        {
            Environment.SetEnvironmentVariable("MINIO_ROOT_USER", MinioUsername);
            Environment.SetEnvironmentVariable("MINIO_ROOT_PASSWORD", MinioPassword);
        }
        
        [Test]
        public async Task PutObjectAsync_WhenUsingLatestMinIOWithChecksumAlgorithmSHA256_ExpectFailure()
        {
            //arrange
            var latestMinio = new MinioBuilder()
                .WithImage(LatestMinioImage)
                .WithUsername(MinioUsername)
                .WithPassword(MinioPassword)
                .Build();

            await latestMinio.StartAsync();
            
            var client = new AmazonS3Client(
                new BasicAWSCredentials(latestMinio.GetAccessKey(), latestMinio.GetSecretKey()),
                new AmazonS3Config
                {
                    UseHttp = true, //plain old http
                    ServiceURL = latestMinio.GetConnectionString(), //target the request at the test container MinIO instance
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
            var act = async () => await client.PutObjectAsync(request);
            
            //assert
            await act.Should()
                .ThrowAsync<AmazonS3Exception>()
                .WithMessage(LatestMinioImageErrorMessage);
        }
        
        [Test]
        public async Task PutObjectAsync_WhenUsingOlderMinIOWithChecksumAlgorithmSHA256_ExpectFailure()
        {
            //arrange
            var olderMinio = new MinioBuilder()
                .WithImage(OlderMinioImage)
                .WithUsername(MinioUsername)
                .WithPassword(MinioPassword)
                .Build();

            await olderMinio.StartAsync();
            
            var client = new AmazonS3Client(
                new BasicAWSCredentials(olderMinio.GetAccessKey(), olderMinio.GetSecretKey()),
                new AmazonS3Config
                {
                    UseHttp = true, //plain old http
                    ServiceURL = olderMinio.GetConnectionString(), //target the request at the test container MinIO instance
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
            var act = async () => await client.PutObjectAsync(request);
            
            //assert
            await act.Should()
                .ThrowAsync<AmazonS3Exception>()
                .WithMessage(OlderMinioImageErrorMessage);
        }
    }
}