using AutoMapper;
using IncomeTaxCalculator.API.ViewModels.Requests;
using IncomeTaxCalculator.API.ViewModels.Responses;
using IncomeTaxCalculator.Domain.DomainModels;
using IncomeTaxCalculator.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace IncomeTaxCalculator.API.Controllers;

[Route("api/tax-band")]
[ApiController]
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

    [HttpPost]
    public async Task<IActionResult> AddTaxBand([FromBody] AddTaxBandRequestViewModel calculateTaxRequestViewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ResponseViewModel.ErrorResponse(ModelState));

        await _taxBandService.AddTaxBandAsync(_mapper.Map<TaxBandDomainModel>(calculateTaxRequestViewModel));

        return Ok(ResponseViewModel.SuccessResponse());
    }

    [HttpDelete("{id}")]
    public IActionResult RemoveTaxBand([FromQuery] Guid id)
    {
        _taxBandService.DeleteTaxBand(id);

        return Ok(ResponseViewModel.SuccessResponse());
    }
}
