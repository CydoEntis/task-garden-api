﻿// // using TaskGarden.Application.Common.Exceptions;
// // using TaskGarden.Application.Common.Models;
// // using TaskGarden.Infrastructure.Models;
// //
// // namespace TaskGarden.Api.Middleware;
// //
// // public class ExceptionHandlingMiddleware
// // {
// //     private readonly RequestDelegate _next;
// //     private readonly ILogger<ExceptionHandlingMiddleware> _logger;
// //
// //     public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
// //     {
// //         _next = next;
// //         _logger = logger;
// //     }
// //
// //     public async Task InvokeAsync(HttpContext context)
// //     {
// //         try
// //         {
// //             await _next(context);
// //         }
// //         catch (AlreadyExistsException ex)
// //         {
// //             LogException(context, ex, "Already exists error occurred.");
// //             await HandleExceptionAsync(context, ex.Title, ex.StatusCode, ex.Errors);
// //         }
// //         catch (ValidationException ex)
// //         {
// //             LogException(context, ex, "Validation error occurred.");
// //             await HandleExceptionAsync(context, ex.Title, ex.StatusCode, ex.Errors);
// //         }
// //         catch (NotFoundException ex)
// //         {
// //             LogException(context, ex, "Resource not found.");
// //             await HandleExceptionAsync(context, ex.Title, ex.StatusCode, new List<ErrorField>
// //             {
// //                 new ErrorField { Field = "not_found", Message = ex.Message }
// //             });
// //         }
// //         catch (InvalidTokenException ex)
// //         {
// //             LogException(context, ex, "Invalid token error.");
// //             await HandleExceptionAsync(context, ex.Title, ex.StatusCode, new List<ErrorField>
// //             {
// //                 new ErrorField { Field = "invalid_token", Message = ex.Message }
// //             });
// //         }
// //         catch (IsRequiredException ex)
// //         {
// //             LogException(context, ex, "Required field error.");
// //             await HandleExceptionAsync(context, ex.Title, ex.StatusCode, new List<ErrorField>
// //             {
// //                 new ErrorField { Field = "required", Message = ex.Message }
// //             });
// //         }
// //         catch (ConflictException ex)
// //         {
// //             LogException(context, ex, "Conflict error.");
// //             await HandleExceptionAsync(context, ex.Title, ex.StatusCode, new List<ErrorField>
// //             {
// //                 new ErrorField { Field = "requirement_not_met", Message = ex.Message }
// //             });
// //         }
// //         catch (OperationException ex)
// //         {
// //             LogException(context, ex, "Invalid operation error.");
// //             await HandleExceptionAsync(context, ex.Title, ex.StatusCode, new List<ErrorField>
// //             {
// //                 new ErrorField { Field = "invalid_operation", Message = ex.Message }
// //             });
// //         }
// //         catch (UnauthorizedAccessException ex)
// //         {
// //             LogException(context, ex, "Unauthorized access error.");
// //             await HandleExceptionAsync(context, "Unauthorized", StatusCodes.Status401Unauthorized,
// //                 new List<ErrorField>
// //                 {
// //                     new ErrorField
// //                         { Field = "permission", Message = "User does not have permission to access this resource." }
// //                 });
// //         }
// //         catch (UnauthorizedException ex)
// //         {
// //             LogException(context, ex, "Unauthorized error.");
// //             await HandleExceptionAsync(context, "Unauthorized", StatusCodes.Status401Unauthorized,
// //                 new List<ErrorField>
// //                 {
// //                     new ErrorField
// //                         { Field = "permission", Message = "User does not have permission to access this resource." }
// //                 });
// //         }
// //         catch (Exception ex)
// //         {
// //             LogException(context, ex, "An unexpected error occurred.");
// //             await HandleExceptionAsync(context, "Internal Server Error", StatusCodes.Status500InternalServerError,
// //                 new List<ErrorField>
// //                 {
// //                     new ErrorField
// //                         { Field = "error", Message = "An unexpected error occurred. Please try again later." }
// //                 });
// //         }
// //     }
// //
// //     private async Task HandleExceptionAsync(HttpContext context, string title, int statusCode,
// //         List<ErrorField>? errors)
// //     {
// //         var errorDictionary =
// //             errors?.ToDictionary(e => e.Field, e => e.Message) ?? new Dictionary<string, string>();
// //         var apiError = ApiResponse<object>.ErrorResponse(title, statusCode, errorDictionary);
// //
// //         context.Response.StatusCode = statusCode;
// //         context.Response.ContentType = "application/json";
// //         await context.Response.WriteAsJsonAsync(apiError);
// //     }
// //
// //     private void LogException(HttpContext context, Exception ex, string message)
// //     {
// //         var innerExceptionMessages = GetInnerExceptions(ex);
// //
// //         _logger.LogError(ex,
// //             "{Message} Path: {Path}, Method: {Method}, User: {User}, Query: {Query}, InnerExceptions: {InnerExceptions}",
// //             message,
// //             context.Request.Path,
// //             context.Request.Method,
// //             context.User.Identity?.Name ?? "Anonymous",
// //             context.Request.QueryString.ToString(),
// //             innerExceptionMessages);
// //     }
// //
// //     private string GetInnerExceptions(Exception ex)
// //     {
// //         var innerExceptions = new List<string>();
// //         while (ex.InnerException != null)
// //         {
// //             ex = ex.InnerException;
// //             innerExceptions.Add(ex.Message);
// //         }
// //
// //         return string.Join(" -> ", innerExceptions);
// //     }
// // }
//
//
// using Microsoft.AspNetCore.Mvc;
// using TaskGarden.Application.Common.Exceptions;
// using TaskGarden.Application.Common.Models;
//
// namespace TaskGarden.Api.Middleware
// {
//     public class ExceptionHandlingMiddleware
//     {
//         private readonly RequestDelegate _next;
//         private readonly ILogger<ExceptionHandlingMiddleware> _logger;
//
//         public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
//         {
//             _next = next;
//             _logger = logger;
//         }
//
//         public async Task InvokeAsync(HttpContext context)
//         {
//             try
//             {
//                 await _next(context);
//             }
//             catch (Exception ex)
//             {
//                 await HandleExceptionAsync(context, ex);
//             }
//         }
//
//         private async Task HandleExceptionAsync(HttpContext context, Exception ex)
//         {
//             var statusCode = StatusCodes.Status500InternalServerError;
//             string title = "An unexpected error occurred.";
//             var details = ex.Message;
//             var errors = new List<ErrorField>(); 
//
//             switch (ex)
//             {
//                 case AlreadyExistsException aeEx:
//                     statusCode = StatusCodes.Status409Conflict;
//                     title = "Already exists";
//                     errors.Add(new ErrorField { Field = "general", Message = aeEx.Message });
//                     break;
//
//                 case ValidationException veEx:
//                     statusCode = StatusCodes.Status400BadRequest;
//                     title = "Validation error";
//                     errors = veEx.Errors
//                         .Select(e => new ErrorField { Field = e.Field, Message = e.Message })
//                         .ToList();
//                     break;
//                 case FluentValidation.ValidationException veEx:
//                     statusCode = StatusCodes.Status400BadRequest;
//                     title = "Validation error";
//                     errors = veEx.Errors
//                         .Select(e => new ErrorField { Field = e.PropertyName, Message = e.ErrorMessage })
//                         .ToList();
//                     break;
//
//                 case UnauthorizedAccessException uaeEx:
//                     statusCode = StatusCodes.Status401Unauthorized;
//                     title = "Unauthorized";
//                     errors.Add(new ErrorField { Field = "authorization", Message = "User is unauthorized." });
//                     break;
//
//                 case NotFoundException nfEx:
//                     statusCode = StatusCodes.Status404NotFound;
//                     title = "Not Found";
//                     errors.Add(new ErrorField { Field = "general", Message = nfEx.Message });
//                     break;
//
//                 case ConflictException cEx:
//                     statusCode = StatusCodes.Status409Conflict;
//                     title = "Conflict error";
//                     errors.Add(new ErrorField { Field = "general", Message = cEx.Message });
//                     break;
//
//                 default:
//                     _logger.LogError(ex, "An unexpected error occurred.");
//                     break;
//             }
//
//             var problemDetails = new ProblemDetails
//             {
//                 Status = statusCode,
//                 Title = title,
//                 Detail = details,
//                 Instance = context.Request.Path,
//             };
//
//             if (errors.Any())
//             {
//                 problemDetails.Extensions.Add("errors", errors);
//             }
//
//             problemDetails.Extensions.Add("requestId", context.TraceIdentifier);
//
//             context.Response.StatusCode = statusCode;
//             context.Response.ContentType = "application/problem+json";
//             await context.Response.WriteAsJsonAsync(problemDetails);
//         }
//
//
//         private void LogException(HttpContext context, Exception ex, string message)
//         {
//             var innerExceptionMessages = GetInnerExceptions(ex);
//             _logger.LogError(ex,
//                 "{Message} Path: {Path}, Method: {Method}, User: {User}, Query: {Query}, InnerExceptions: {InnerExceptions}",
//                 message,
//                 context.Request.Path,
//                 context.Request.Method,
//                 context.User.Identity?.Name ?? "Anonymous",
//                 context.Request.QueryString.ToString(),
//                 innerExceptionMessages);
//         }
//
//         private string GetInnerExceptions(Exception ex)
//         {
//             var innerExceptions = new List<string>();
//             while (ex.InnerException != null)
//             {
//                 ex = ex.InnerException;
//                 innerExceptions.Add(ex.Message);
//             }
//
//             return string.Join(" -> ", innerExceptions);
//         }
//     }
// }