using Employee.Mgmt.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace Employee.Mgmt.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StatusController : ControllerBase
{
    private readonly CreateIssuer _createIssuer;

    public StatusController(CreateIssuer createIssuer)
    {
        _createIssuer = createIssuer;
    }

    [HttpGet("issuance-response")]
    public async Task<ActionResult> IssuanceResponseAsync()
    {
        try
        {
            string? id = Request.Query["id"];
            if (id == null)
            {
                return BadRequest(new { error = "400", error_description = "Missing argument 'id'" });
            }

            var statusModel = await _createIssuer.GetIssuanceStatus(id);

            return Ok(statusModel);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = "400", error_description = ex.Message });
        }
    }
}
