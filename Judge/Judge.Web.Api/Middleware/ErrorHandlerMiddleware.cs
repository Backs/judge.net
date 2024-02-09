using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Judge.Web.Api.Middleware;

public class ErrorHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var problemDetails = CreateProblemDetails(context, exception);
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }

    private static ProblemDetails CreateProblemDetails(in HttpContext context, in Exception exception)
    {
        var statusCode = context.Response.StatusCode;
        var reasonPhrase = ReasonPhrases.GetReasonPhrase(statusCode);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = reasonPhrase,
            Detail = exception.ToString(),
            Extensions =
            {
                ["traceId"] = Activity.Current?.Id ?? context.TraceIdentifier,
                ["data"] = exception.Data
            }
        };


        return problemDetails;
    }
}