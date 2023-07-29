## S3.Tests
.NET6 AWS S3 client integration tests

### Prerequisites
* An active AWS account (free tier is enough)
* A valid access key/secret configured for an IAM user
* A created S3 bucket
* `AWS_ACCESS_KEY_ID` and `AWS_SECRET_ACCESS_KEY` environment variables set to a valid access key/secret

### Nuget packages used
```
<PackageReference Include="AWSSDK.S3" Version="3.7.108" />
<PackageReference Include="FluentAssertions" Version="6.11.0" />
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.6.3" />
<PackageReference Include="NUnit" Version="3.13.3" />
<PackageReference Include="NUnit3TestAdapter" Version="4.5.0" />
```

### Build requirements
* .NET6 SDK
* Optional: an IDE i.e. Visual Studio Code / Rider / Visual Studio

### Running the tests

```
$ dotnet test ./S3.Tests/S3.Tests.csproj
  Determining projects to restore...
  All projects are up-to-date for restore.
  S3.Tests -> /home/joe/Code/git-repos/S3.Tests/S3.Tests/bin/Debug/net6.0/S3.Tests.dll
Test run for /home/joe/Code/git-repos/S3.Tests/S3.Tests/bin/Debug/net6.0/S3.Tests.dll (.NETCoreApp,Version=v6.0)
Microsoft (R) Test Execution Command Line Tool Version 17.0.3+cc7fb0593127e24f55ce016fb3ac85b5b2857fec
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.
EnvironmentVariablesAWSCredentials 1|2023-07-29T14:58:29.772Z|INFO|Credentials found using environment variables.
EnvironmentVariableInternalConfiguration 2|2023-07-29T14:58:29.797Z|INFO|The environment variable AWS_ENABLE_ENDPOINT_DISCOVERY was not set with a value.
EnvironmentVariableInternalConfiguration 3|2023-07-29T14:58:29.797Z|INFO|The environment variable AWS_MAX_ATTEMPTS was not set with a value.
EnvironmentVariableInternalConfiguration 4|2023-07-29T14:58:29.797Z|INFO|The environment variable AWS_RETRY_MODE was not set with a value.
EnvironmentVariableInternalConfiguration 5|2023-07-29T14:58:29.798Z|INFO|The environment variable AWS_EC2_METADATA_SERVICE_ENDPOINT was not set with a value.
EnvironmentVariableInternalConfiguration 6|2023-07-29T14:58:29.798Z|INFO|The environment variable AWS_EC2_METADATA_SERVICE_ENDPOINT_MODE was not set with a value.
EnvironmentVariableInternalConfiguration 7|2023-07-29T14:58:29.798Z|INFO|The environment variable AWS_USE_DUALSTACK_ENDPOINT was not set with a value.
EnvironmentVariableInternalConfiguration 8|2023-07-29T14:58:29.798Z|INFO|The environment variable AWS_USE_FIPS_ENDPOINT was not set with a value.
EnvironmentVariableInternalConfiguration 9|2023-07-29T14:58:29.798Z|INFO|The environment variable AWS_IGNORE_CONFIGURED_ENDPOINT_URLS was not set with a value.
UserCrypto 10|2023-07-29T14:58:29.805Z|INFO|UserCrypto is not supported.  This may be due to use of a non-Windows operating system or Windows Nano Server, or the current user account may not have its profile loaded. Unable to load shared library 'Crypt32.dll' or one of its dependencies. In order to help diagnose loading problems, consider setting the LD_DEBUG environment variable: libCrypt32.dll: cannot open shared object file: No such file or directory
ProfileInternalConfiguration 11|2023-07-29T14:58:29.826Z|INFO|Unable to find a profile named 'default' in store Amazon.Runtime.CredentialManagement.CredentialProfileStoreChain
DefaultConfigurationProvider 12|2023-07-29T14:58:29.837Z|INFO|Resolved DefaultConfigurationMode for RegionEndpoint [eu-west-1] to [Legacy].
EnvironmentVariablesAWSCredentials 13|2023-07-29T14:58:30.004Z|INFO|Credentials found using environment variables.
AWSSDKUtils 14|2023-07-29T14:58:30.050Z|DEBUG|Single encoded /S3.Tests.resources.test-file.txt with endpoint https://test-bucket-unique-unique.s3.eu-west-1.amazonaws.com/ for canonicalization: /S3.Tests.resources.test-file.txt
AmazonS3Client 15|2023-07-29T14:58:30.676Z|DEBUG|Received response (truncated to 1024 bytes): []
AmazonS3Client 16|2023-07-29T14:58:30.695Z|INFO|Request metrics: {"properties":{"AsyncCall":"True","CanonicalRequest":"PUT\n/S3.Tests.resources.test-file.txt\n\ncontent-length:334\ncontent-type:text/plain\nhost:test-bucket-unique-unique.s3.eu-west-1.amazonaws.com\nuser-agent:aws-sdk-dotnet-coreclr/3.7.108.0 aws-sdk-dotnet-core/3.7.108.2 .NET_Core/6.0.20 OS/Linux_5.15.0-76-generic_#83-Ubuntu_SMP_Thu_Jun_15_19:16:32_UTC_2023 ClientAsync\nx-amz-content-sha256:STREAMING-AWS4-HMAC-SHA256-PAYLOAD-TRAILER\nx-amz-date:20230729T135830Z\nx-amz-decoded-content-length:4\nx-amz-sdk-checksum-algorithm:SHA256\nx-amz-trailer:x-amz-checksum-sha256\n\ncontent-length;content-type;host;user-agent;x-amz-content-sha256;x-amz-date;x-amz-decoded-content-length;x-amz-sdk-checksum-algorithm;x-amz-trailer\nSTREAMING-AWS4-HMAC-SHA256-PAYLOAD-TRAILER","StringToSign":"AWS4-HMAC-SHA256\n20230729T135830Z\n20230729/eu-west-1/s3/aws4_request\nd03a0a1ef5d85079110970369f4a16ef68a88c9ef6e4019b31da327430caf704","ServiceName":"AmazonS3","ServiceEndpoint":"https://test-bucket-unique-unique.s3.eu-west-1.amazonaws.com/","MethodName":"PutObjectRequest","AmzId2":"x6Xcd4v/8DffrZPORMf0yCX8iEsVX6W+QkAugoFmfzVPrcp0p/eNBHxEjeD7VWafvo3mPzsiUvk=","StatusCode":"OK","BytesProcessed":"0","AWSRequestID":"KS6K89REJB4FBPNJ"},"timings":{"CredentialsRequestTime":1.8105,"RequestSigningTime":62.3623,"HttpRequestTime":566.2856,"ResponseUnmarshallTime":2.211,"ResponseProcessingTime":7.9126,"ClientExecuteTime":818.7543},"counters":{}}
SHA-256 hash: n4bQgYhMfWWaL+qgxVrQFaO/TxsrC4Is0V1sFbDwCgg=

Passed!  - Failed:     0, Passed:     1, Skipped:     0, Total:     1, Duration: 960 ms - /home/joe/Code/git-repos/S3.Tests/S3.Tests/bin/Debug/net6.0/S3.Tests.dll (net6.0)
```

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