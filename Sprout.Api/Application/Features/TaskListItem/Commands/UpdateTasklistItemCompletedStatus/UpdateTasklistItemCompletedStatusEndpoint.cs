﻿using MediatR;
using Sprout.Api.Application.Shared.Models;

namespace Sprout.Api.Application.Features.TaskListItem.Commands.UpdateTaskListItemCompletedStatus;

public static class UpdateTasklistItemCompletedStatusEndpoint
{
    public static void MapUpdateTaskListItemCompletedStatusEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapPut("/api/task-list/{taskListId}/items/status",
                async (UpdateTaskListItemCompletedStatusCommand command, IMediator mediator) =>
                {
                    var response = await mediator.Send(command);
                    return Results.Ok(ApiResponse<UpdateTaskListItemCompletedStatusResponse>.SuccessWithData(response));
                })
            .WithName("UpdateCompletedStatus")
            .WithTags("Task List Item")
            .RequireAuthorization()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);
    }
}