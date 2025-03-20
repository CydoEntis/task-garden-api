﻿using MediatR;
using TaskGarden.Infrastructure.Models;

namespace TaskGarden.Api.Application.Features.Auth.Commands.ChangePassword;

public static class ChangePasswordEndpoint
{
    public static void MapChangePasswordEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/auth/change-password", async (ChangePasswordCommand command, IMediator mediator) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(ApiResponse<ChangePasswordResponse>.SuccessWithData(response));
            })
            .WithName("ChangePassword")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status200OK);
    }
}