using FluentAssertions;
using NUnit.Framework;

namespace S3.Tests;

/// <summary>
/// S3 related object integrity hash check tests
/// 
/// Linux CLI tests:
/// 
/// sha256sum test-file.txt | cut -d " " -f1
/// 9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08
/// 
/// sha256sum test-file.txt | cut -d " " -f1  | xxd -r -p  | base64
/// n4bQgYhMfWWaL+qgxVrQFaO/TxsrC4Is0V1sFbDwCgg=
///
/// </summary>
public class HashTests : TestBase
{
    private CancellationTokenSource _cts = null!;
    
    [SetUp]
    public void SetUp()
    {
        _cts = new CancellationTokenSource();
    }

    [Test]
    public async Task GivenCalculatedBase64Sha256Hash_ExpectBase64Sha256Hash()
    {
        const string expected = "n4bQgYhMfWWaL+qgxVrQFaO/TxsrC4Is0V1sFbDwCgg=";
        
        //act
        var act = await GetFileResourceBase64Sha256Hash(_cts.Token);
        
        //assert
        act.Should().Be(expected);
    }
    
    [Test]
    public async Task GivenCalculatedHexSha256Hash_ExpectHexSha256Hash()
    {
        const string expected = "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08";
        
        //act
        var act = await GetFileResourceHexSha256Hash(_cts.Token);
        
        //assert
        act.Should().Be(expected);
    }
    
    [Test]
    public async Task GivenCalculatedHexSha256Hash_WhenConvertBackFromHexString_ExpectCanCalculateExpectedBase64Sha256Hash()
    {
        const string expected = "n4bQgYhMfWWaL+qgxVrQFaO/TxsrC4Is0V1sFbDwCgg=";
        
        //arrange
        var hexHash = await GetFileResourceHexSha256Hash(_cts.Token);
        
        //act
        var hexBytes = Convert.FromHexString(hexHash);
        var act = Convert.ToBase64String(hexBytes);
        
        //assert
        act.Should().Be(expected);
    }
}