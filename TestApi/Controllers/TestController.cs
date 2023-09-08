using Microsoft.AspNetCore.Mvc;

namespace TestApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TestController : ControllerBase
{
    private readonly S3Gateway _gateway;

    public TestController(S3Gateway gateway)
    {
        _gateway = gateway;
    }

    [HttpGet]
    public async Task<IActionResult> DoS3()
    {
        var response = await _gateway.PutAndGetFileAsync();
        return Ok(response);
    }
}