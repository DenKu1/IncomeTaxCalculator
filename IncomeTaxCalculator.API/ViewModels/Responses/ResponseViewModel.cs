﻿namespace IncomeTaxCalculator.API.ViewModels.Responses;

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
