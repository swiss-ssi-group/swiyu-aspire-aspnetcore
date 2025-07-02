using Employee.Mgmt.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace Employee.Mgmt.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class StatusController : ControllerBase
{
    private readonly CreateIssuer _createIssuer;

    public StatusController(CreateIssuer createIssuer)
    {
        _createIssuer = createIssuer;
    }

    [HttpGet("/api/issuer/issuance-response")]
    public async Task<ActionResult> IssuanceResponseAsync()
    {
        try
        {
            string? id = Request.Query["id"];
            if (id == null)
            {
                return BadRequest(new { error = "400", error_description = "Missing argument 'id'" });
            }

            var data = await _createIssuer.GetIssuanceStatus(id);
            if (data != null)
            {
                Debug.WriteLine("check if there was a response yet: " + data);
                return new ContentResult
                {
                    ContentType = "application/json",
                    Content = JsonSerializer.Serialize(data)
                };
            }

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = "400", error_description = ex.Message });
        }
    }
}
