﻿using AutoMapper;
using TaskGarden.Api.Dtos.TaskList;
using TaskGarden.Api.Errors;
using TaskGarden.Api.Helpers;
using TaskGarden.Api.Services.Contracts;
using TaskGarden.Data.Enums;
using TaskGarden.Data.Models;
using TaskGarden.Data.Repositories.Contracts;

namespace TaskGarden.Api.Services.Implementations;

public class TaskListService : ITaskListService
{
    private readonly ITaskListRepository _taskListRepository;
    private readonly IUserContextService _userContextService;
    private readonly IMapper _mapper;
    private readonly IUserTaskListService _userTaskListService;

    public TaskListService(ITaskListRepository taskListRepository, IUserContextService userContextService,
        IMapper mapper, IUserTaskListService userTaskListService)
    {
        _taskListRepository = taskListRepository;
        _userContextService = userContextService;
        _mapper = mapper;
        _userTaskListService = userTaskListService;
    }

    public async Task<NewTaskListResponseDto> CreateNewTaskListAsync(NewTaskListRequestDto dto)
    {
        var userId = _userContextService.GetUserId();
        if (userId == null)
            throw new UnauthorizedAccessException("User not authenticated");

        var taskList = _mapper.Map<TaskList>(dto);
        taskList.UserId = userId;


        await _taskListRepository.AddAsync(taskList);
        await _userTaskListService.AssignUserToTaskListAsync(userId, taskList.Id, TaskListRole.Owner);

        return new NewTaskListResponseDto() { Message = $"Task list created: {taskList.Id}", Id = taskList.Id };
    }

    public async Task<TaskListResponseDto> GetTaskListByIdAsync(int taskListId)
    {
        var userId = _userContextService.GetUserId();
        if (userId == null)
            throw new UnauthorizedAccessException("User not authenticated");

        var taskLists = await _taskListRepository.GetTaskListByIdForUser(userId, taskListId);
        return _mapper.Map<TaskListResponseDto>(taskLists);
    }

    public async Task UpdateTaskListAsync(int taskListId, UpdateTaskListRequestDto dto)
    {
        var userId = _userContextService.GetUserId();
        if (userId == null)
            throw new UnauthorizedAccessException("User not authenticated");

        var userRoleString = await _userTaskListService.GetUserRoleAsync(userId, taskListId);

        if (!Enum.TryParse<TaskListRole>(userRoleString, out var userRole))
        {
            throw new PermissionException("Invalid role");
        }

        if (userRole != TaskListRole.Owner && userRole != TaskListRole.Editor)
            throw new PermissionException("You do not have permission to update this task list");

        // TODO: Add updating of individual tasks.

        var taskList = await _taskListRepository.GetAsync(taskListId);
        if (taskList == null)
            throw new NotFoundException("Task list not found");

        _mapper.Map(dto, taskList);
        await _taskListRepository.UpdateAsync(taskList);

        // TODO: Decided if i want to add a return dto, Update messagge and possible id of the updated entity.
    }
}