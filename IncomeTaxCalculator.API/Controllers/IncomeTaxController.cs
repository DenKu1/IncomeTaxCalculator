using AutoMapper;
using IncomeTaxCalculator.API.ViewModels.Requests;
using IncomeTaxCalculator.API.ViewModels.Responses;
using IncomeTaxCalculator.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IncomeTaxCalculator.API.Controllers;

[Route("api/income-tax")]
[ApiController]
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
    public async Task<IActionResult> CalculateIncomeTax([FromBody] CalculateTaxRequestViewModel calculateTaxRequestViewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ResponseViewModel.ErrorResponse(ModelState));

        var result = await _incomeTaxService.CalculateIncomeTaxAsync(calculateTaxRequestViewModel.GrossAnnualSalary);

        return Ok(ResponseViewModel<CalculateTaxResponseViewModel>.SuccessResponse(
            _mapper.Map<CalculateTaxResponseViewModel>(result)));
    }
}
