using IncomeTaxCalculator.API.ViewModels.Requests;
using IncomeTaxCalculator.API.ViewModels.Responses;
using Microsoft.AspNetCore.Mvc;

namespace IncomeTaxCalculator.API.Controllers;

[Route("api/tax")]
[ApiController]
public class TaxController : ControllerBase
{
    [HttpPost("calculate")]
    public async Task<IActionResult> CalculateTax(CalculateTaxRequestViewModel calculateTaxRequestViewModel)
    {
        return Ok(ResponseViewModel.SuccessResponse());
    }
}
