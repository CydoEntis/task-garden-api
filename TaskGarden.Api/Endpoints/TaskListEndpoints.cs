﻿using TaskGarden.Api.Dtos.Category;
using TaskGarden.Api.Dtos.TaskList;
using TaskGarden.Api.Services.Contracts;
using TaskGarden.Data.Models;

namespace TaskGarden.Api.Endpoints;

public static class TaskListEndpoints
{
    public static void MapTaskListEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/task-list").WithTags("Task List");

        group.MapGet("/{taskListId:int}",
                async (int taskListId, ITaskListService taskListService) =>
                {
                    var response = await taskListService.GetTaskListByIdAsync(taskListId);
                    return Results.Ok(ApiResponse<TaskListDetailsResponseDto>.SuccessResponse(response));
                })
            .WithName("GetTaskListById")
            .RequireAuthorization()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK);

        group.MapPost("/",
                async (NewTaskListRequestDto dto, ITaskListService taskListService) =>
                {
                    var response = await taskListService.CreateNewTaskListAsync(dto);
                    return Results.Ok(ApiResponse<NewTaskListResponseDto>.SuccessResponse(response));
                })
            .WithName("CreateTaskList")
            .RequireAuthorization()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK);
    }
}