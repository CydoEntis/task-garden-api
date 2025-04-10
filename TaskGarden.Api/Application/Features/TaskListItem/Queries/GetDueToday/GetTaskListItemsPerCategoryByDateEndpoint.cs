﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskGarden.Api.Application.Shared.Models;

namespace TaskGarden.Api.Application.Features.TaskListItem.Queries.GetDueToday;

public static class GetTaskListItemsPerCategoryByDateEndpoint
{
    public static void MapGetTaskListItemsPerCategoryByDateEndpoint(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/task-list/due-by-date",
                async (IMediator mediator, [FromQuery] DateTime date, [FromQuery] int page = 1,
                    [FromQuery] int pageSize = 20) =>
                {
                    var query = new GetTaskListItemsPerCategoryByDateQuery(date, page, pageSize);
                    var response = await mediator.Send(query);
                    return Results.Ok(ApiResponse<PagedResponse<TaskListItemCategoryGroup>>.SuccessWithData(response));
                })
            .WithName("GetTaskListItemsPerCategoryByDate")
            .WithTags("Task List Item")
            .RequireAuthorization()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK);
    }
}