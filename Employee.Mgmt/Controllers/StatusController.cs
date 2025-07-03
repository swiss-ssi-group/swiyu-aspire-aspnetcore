using Employee.Mgmt.Services;
using Microsoft.AspNetCore.Mvc;

namespace Employee.Mgmt.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StatusController : ControllerBase
{
    private readonly IssuerService _issuerService;

    public StatusController(IssuerService issuerService)
    {
        _issuerService = issuerService;
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

            var statusModel = await _issuerService.GetIssuanceStatus(id);

            return Ok(statusModel);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = "400", error_description = ex.Message });
        }
    }
}
