using System.Reflection;
using Amazon;
using NUnit.Framework;

namespace S3.Tests;

public abstract class TestBase
{
    protected const string BucketName = "test-bucket-unique-unique"; //set to a valid S3 bucket name
    protected static readonly RegionEndpoint RegionEndpoint = RegionEndpoint.EUWest1; //set to the region where the S3 bucket exists
    
    protected string FileResourceName = null!;
    protected Stream FileResourceStream = null!;
        
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        FileResourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames().First();
        FileResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(FileResourceName)!;
    }
}