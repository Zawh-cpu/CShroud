using System.Security.Claims;
using CShroudGateway.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CShroudGateway.Presentation.Api.v1.Controllers;

public partial class KeyController
{
    [HttpGet("")]
    public async Task<IActionResult> GetMyKeysAsync([FromQuery] int size = 0, [FromQuery] int page = 0)
    {
        if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid jti)) return NotFound();
        
        // THINK ABOUT OPTIMIZE
        
        var keys = await _baseRepository.GetKeysByExpressionAsync(key => key.UserId == jti);
        
        Response.Headers.Append("X-Total-Count", keys.Length.ToString());
        Response.Headers.Append("X-Enabled-Count", keys.Count(key => key.Status == KeyStatus.Enabled).ToString());
        
        return Ok(keys.OrderBy(k => k.Id).Skip(page * size).Take(size));
    }
}