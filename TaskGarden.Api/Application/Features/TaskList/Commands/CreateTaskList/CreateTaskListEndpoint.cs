﻿using MediatR;
using TaskGarden.Infrastructure.Models;

namespace TaskGarden.Api.Application.Features.TaskList.Commands.CreateTaskList;

public static class CreateTaskListEndpoint
{
    public static void MapCreateTaskListEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/task-list", async (CreateTaskListCommand command, IMediator mediator) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(ApiResponse<CreateTaskListResponse>.SuccessWithData(response));
            })
            .WithName("CreateTaskList")
            .RequireAuthorization()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK);
    }
}