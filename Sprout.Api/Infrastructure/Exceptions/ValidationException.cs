﻿// using Microsoft.AspNetCore.Http;
// using Sprout.Application.Common.Constants;
// using Sprout.Application.Common.Models;
//
// namespace Sprout.Application.Common.Exceptions;
//
// public class ValidationException : BaseException
// {
//     public List<ErrorField> Errors { get; set; }
//
//     // Constructor to handle validation errors
//     public ValidationException(IEnumerable<FluentValidation.Results.ValidationFailure> validationFailures)
//         : base(StatusCodes.Status400BadRequest, "Validation Error", "One or more validation errors occurred.")
//     {
//         Errors = validationFailures.Select(failure => new ErrorField
//         {
//             Field = failure.PropertyName,
//             Message = failure.ErrorMessage
//         }).ToList();
//     }
//
//     public ValidationException(string field, string fieldMessage)
//         : base(StatusCodes.Status400BadRequest, "Validation Error", "One or more validation errors occurred.")
//     {
//         Errors.Add(new ErrorField { Field = field, Message = fieldMessage });
//     }
// }