## S3.Tests
A simple .NET6 AWS S3 client console application

### Prerequisites
* An active AWS account (free tier is enough)
* A valid access key/secret configured for an IAM user
* A created S3 bucket
* `AWS_ACCESS_KEY_ID` and `AWS_SECRET_ACCESS_KEY` environment variables set to the valid access key/secret

### Build requirements
* .NET6 SDK
* Optional: an IDE i.e. Visual Studio Code / Rider / Visual Studio

### Running the application
Build / run the application

`dotnet run --project ./S3.Tests/S3.Tests.csproj`

The `resources/test-file.txt` embedded resources file is uploaded to S3

### Output logging
The S3 side calculated SHA-256 hash is output to the console

`SHA-256 hash: n4bQgYhMfWWaL+qgxVrQFaO/TxsrC4Is0V1sFbDwCgg=`

AWSS3Client logging is configured to log to the console
```
{
  "properties": {
    "AsyncCall": "True",
    "CanonicalRequest": "PUT\n/S3.Tests.resources.test-file.txt\n\ncontent-length:334\ncontent-type:text/plain\nhost:test-bucket-unique-unique.s3.eu-west-1.amazonaws.com\nuser-agent:aws-sdk-dotnet-coreclr/3.7.108.0 aws-sdk-dotnet-core/3.7.108.2 .NET_Core/6.0.20 OS/Linux_5.15.0-76-generic_#83-Ubuntu_SMP_Thu_Jun_15_19:16:32_UTC_2023 ClientAsync\nx-amz-content-sha256:STREAMING-AWS4-HMAC-SHA256-PAYLOAD-TRAILER\nx-amz-date:20230715T202016Z\nx-amz-decoded-content-length:4\nx-amz-sdk-checksum-algorithm:SHA256\nx-amz-trailer:x-amz-checksum-sha256\n\ncontent-length;content-type;host;user-agent;x-amz-content-sha256;x-amz-date;x-amz-decoded-content-length;x-amz-sdk-checksum-algorithm;x-amz-trailer\nSTREAMING-AWS4-HMAC-SHA256-PAYLOAD-TRAILER",
    "StringToSign": "AWS4-HMAC-SHA256\n20230715T202016Z\n20230715/eu-west-1/s3/aws4_request\nb578abeb0f960d0a212884b956d5955ed8b391f19558a995c968a5098b927c35",
    "ServiceName": "AmazonS3",
    "ServiceEndpoint": "https://test-bucket-unique-unique.s3.eu-west-1.amazonaws.com/",
    "MethodName": "PutObjectRequest",
    "AmzId2": "K/tkG3X8rsgUOglFdzNFyrF3UjvVKDiQrOei7ZBA8U11udpuG4B5ixtMz+qeoXcJQiestAh+1Ik=",
    "StatusCode": "OK",
    "BytesProcessed": "0",
    "AWSRequestID": "SRTR24V3E5BZX3T0"
  },
  "timings": {
    "CredentialsRequestTime": 1.5848,
    "RequestSigningTime": 77.7942,
    "HttpRequestTime": 577.0956,
    "ResponseUnmarshallTime": 2.2331,
    "ResponseProcessingTime": 9.5776,
    "ClientExecuteTime": 832.8429
  },
  "counters": {}
}
```

```
PUT
/S3.Tests.resources.test-file.txt

content-length:334
content-type:text/plain
host:test-bucket-unique-unique.s3.eu-west-1.amazonaws.com
user-agent:aws-sdk-dotnet-coreclr/3.7.108.0 aws-sdk-dotnet-core/3.7.108.2 .NET_Core/6.0.20 OS/Linux_5.15.0-76-generic_#83-Ubuntu_SMP_Thu_Jun_15_19:16:32_UTC_2023 ClientAsync
x-amz-content-sha256:STREAMING-AWS4-HMAC-SHA256-PAYLOAD-TRAILER
x-amz-date:20230715T202016Z
x-amz-decoded-content-length:4
x-amz-sdk-checksum-algorithm:SHA256
x-amz-trailer:x-amz-checksum-sha256

content-length;content-type;host;user-agent;x-amz-content-sha256;x-amz-date;x-amz-decoded-content-length;x-amz-sdk-checksum-algorithm;x-amz-trailer
STREAMING-AWS4-HMAC-SHA256-PAYLOAD-TRAILER
```