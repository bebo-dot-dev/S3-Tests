using System.Reflection;
using System.Security.Cryptography;
using Amazon;
using NUnit.Framework;

namespace S3.Tests;

public abstract class TestBase
{
    protected const string BucketName = "test-bucket-unique-unique"; //set to a valid S3 bucket name
    protected static readonly RegionEndpoint RegionEndpoint = RegionEndpoint.EUWest1; //set to the region where the S3 bucket exists
    
    protected string FileResourceName = null!;
    protected Stream FileResourceStream = null!;
        
    [SetUp]
    public void BaseSetUp()
    {
        FileResourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames().First();
        FileResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(FileResourceName)!;
    }

    protected async Task<byte[]> GetFileResourceSha256HashBytes(CancellationToken cancellationToken)
    {
        FileResourceStream.Position = 0;
        
        var hasher = SHA256.Create();
        var hashBytes = await hasher.ComputeHashAsync(FileResourceStream, cancellationToken);
        return hashBytes;
    }

    protected async Task<string> GetFileResourceBase64Sha256Hash(CancellationToken cancellationToken)
    {
        var base64Hash = Convert.ToBase64String(await GetFileResourceSha256HashBytes(cancellationToken));
        return base64Hash;
    }
    
    protected async Task<string> GetFileResourceHexSha256Hash(CancellationToken cancellationToken)
    {
        var base64Hash = Convert.ToHexString(await GetFileResourceSha256HashBytes(cancellationToken));
        return base64Hash.ToLowerInvariant();
    }
}