using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using BuildingBlocks.Framework.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using static WebApi.Core.Domain.Exceptions.DomainExceptions;

namespace Endpoints.WebApi.Extensions.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IConfiguration configuration)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex, configuration);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, IConfiguration configuration)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            ErrorDetails result;

            if (exception is UnauthorizedAccessException)
            {
                result = new ErrorDetails(
                    context.Response.StatusCode,
                    "Access denied",
                    exception.StackTrace
                );
            }
            else if (context.Response.StatusCode == 403)
            {
                result = new ErrorDetails(
                    context.Response.StatusCode,
                    "You're not authorized",
                    exception.StackTrace
                );
            }
            else if (exception is InvalidEntityState stateEntity)
            {
                context.Response.StatusCode = 400;
                result = new ErrorDetails(
                    400,
                    "Validation Errors",
                    context.Request.Path,
                    stateEntity.Errors
                    );
            }
            else
            {
                result = new ErrorDetails(
                    context.Response.StatusCode,
                    bool.Parse(configuration["DisplayError"]) ? exception.GetBaseException().Message : "System has problem",
                    context.Request.Path
                );
                
            }
            return context.Response.WriteAsync(JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            }));
        }
    }
}
