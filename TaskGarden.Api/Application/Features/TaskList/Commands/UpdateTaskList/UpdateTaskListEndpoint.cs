﻿using MediatR;
using TaskGarden.Api.Application.Shared.Models;

namespace TaskGarden.Api.Application.Features.TaskList.Commands.UpdateTaskList;

public static class UpdateTaskListEndpoint
{
    public static void MapUpdateTaskListEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPut("/api/task-list", async (UpdateTaskListCommand command, IMediator mediator) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(ApiResponse<UpdateTaskListResponse>.SuccessWithData(response));
            })
            .WithName("UpdateTaskList")
            .RequireAuthorization()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK);
    }
}