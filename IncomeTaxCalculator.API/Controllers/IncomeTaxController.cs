using AutoMapper;
using IncomeTaxCalculator.API.ViewModels.Requests;
using IncomeTaxCalculator.API.ViewModels.Responses;
using IncomeTaxCalculator.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IncomeTaxCalculator.API.Controllers;

[Route("api/taxes/income-tax")]
[ApiController]
[ProducesResponseType(typeof(ResponseViewModel), StatusCodes.Status500InternalServerError)]
public class IncomeTaxController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IIncomeTaxService _incomeTaxService;

    public IncomeTaxController(
        IMapper mapper,
        IIncomeTaxService incomeTaxService)
    {
        _mapper = mapper;
        _incomeTaxService = incomeTaxService;
    }

    [HttpPost("calculate")]
    [SwaggerOperation(
        Summary = "Calculates UK Income tax according to tax bands",
        Description = "Tax within each band is calculated based on the amount of the salary falling within a band. " +
        "The total tax is the sum of the tax paid within all bands. " +
        "Each band has an optional upper and mandatory lower limit and a percentage rate of tax")]
    [ProducesResponseType(typeof(ResponseViewModel<CalculateIncomeTaxResponseViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseViewModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CalculateIncomeTax([FromBody] CalculateIncomeTaxRequestViewModel calculateTaxRequestViewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ResponseViewModel.ErrorResponse(ModelState));

        var result = await _incomeTaxService.CalculateIncomeTaxAsync(calculateTaxRequestViewModel.GrossAnnualSalary);

        return Ok(ResponseViewModel<CalculateIncomeTaxResponseViewModel>.SuccessResponse(
            _mapper.Map<CalculateIncomeTaxResponseViewModel>(result)));
    }
}
