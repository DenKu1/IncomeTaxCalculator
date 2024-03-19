using AutoMapper;
using IncomeTaxCalculator.API.ViewModels.Requests;
using IncomeTaxCalculator.API.ViewModels.Responses;
using IncomeTaxCalculator.Domain.DomainModels;
using IncomeTaxCalculator.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IncomeTaxCalculator.API.Controllers;

[Route("api/tax-bands")]
[ApiController]
[ProducesResponseType(typeof(ResponseViewModel), StatusCodes.Status500InternalServerError)]
public class TaxBandController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITaxBandService _taxBandService;

    public TaxBandController(
        IMapper mapper,
        ITaxBandService taxBandService)
    {
        _mapper = mapper;
        _taxBandService = taxBandService;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all tax bands")]
    [ProducesResponseType(typeof(ResponseViewModel<IEnumerable<TaxBandResponseViewModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTaxBands()
    {
        var result = await _taxBandService.GetAllTaxBandsAsync();

        return Ok(ResponseViewModel<IEnumerable<TaxBandResponseViewModel>>.SuccessResponse(
            _mapper.Map<IEnumerable<TaxBandResponseViewModel>>(result)));
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Adds new tax band on top of the tax bands stack")]
    [ProducesResponseType(typeof(ResponseViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseViewModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PushTaxBand([FromBody] AddTaxBandRequestViewModel calculateTaxRequestViewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ResponseViewModel.ErrorResponse(ModelState));

        await _taxBandService.PushTaxBandAsync(_mapper.Map<TaxBandDomainModel>(calculateTaxRequestViewModel));

        return Ok(ResponseViewModel.SuccessResponse());
    }

    [HttpDelete]
    [SwaggerOperation(Summary = "Removes the most recently added tax band")]
    [ProducesResponseType(typeof(ResponseViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> PopTaxBand()
    {
        await _taxBandService.PopTaxBandAsync();

        return Ok(ResponseViewModel.SuccessResponse());
    }
}
