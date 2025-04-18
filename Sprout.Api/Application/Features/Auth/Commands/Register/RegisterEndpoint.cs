﻿using MediatR;
using Sprout.Api.Application.Features.Auth.Commands.Login;
using Sprout.Api.Application.Shared.Models;

namespace Sprout.Api.Application.Features.Auth.Commands.Register;

public static class RegisterEndpoint
{
    public static void MapRegisterEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/auth/register", async (RegisterCommand command, IMediator mediator) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(ApiResponse<LoginResponse>.SuccessWithData(response));
            })
            .WithName("Register")
            .WithTags("Auth")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK);
    }
}