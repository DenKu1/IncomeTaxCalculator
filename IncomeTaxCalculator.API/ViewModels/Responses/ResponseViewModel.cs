using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IncomeTaxCalculator.API.ViewModels.Responses;

public class ResponseViewModel
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }

    public static ResponseViewModel SuccessResponse(string message = "") => new()
    {
        IsSuccess = true,
        Message = message
    };

    public static ResponseViewModel ErrorResponse(string message = "") => new()
    {
        IsSuccess = false,
        Message = message
    };

    public static ResponseViewModel ErrorResponse(ModelStateDictionary modelState) => new()
    {
        IsSuccess = false,
        Message = string.Join(" | ", modelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage))
    };
}

public class ResponseViewModel<T> : ResponseViewModel where T : class
{
    public T Result { get; set; }

    public static ResponseViewModel<T> SuccessResponse(T result, string message = "") => new()
    {
        IsSuccess = true,
        Message = message,
        Result = result
    };
}
