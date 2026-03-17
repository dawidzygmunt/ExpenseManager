using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesManager.Api.Controllers
{
  [ApiController]
  public class AuthController : ControllerBase
  {
    [HttpPost("login")]
    public async Task<IActionResult> LoginEndpoint([FromBody] LoginRequest request)
    {
      return Ok();
    }
  }
}