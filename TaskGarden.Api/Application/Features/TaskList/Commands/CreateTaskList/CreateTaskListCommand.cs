﻿using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskGarden.Api.Application.Shared.Extensions;
using TaskGarden.Api.Application.Shared.Handlers;
using TaskGarden.Api.Application.Shared.Models;
using TaskGarden.Api.Application.Shared.Projections;
using TaskGarden.Api.Domain.Entities;
using TaskGarden.Api.Domain.Enums;
using TaskGarden.Api.Infrastructure.Persistence;
using TaskGarden.Application.Common.Exceptions;
using TaskGarden.Domain.Enums;
using TaskGarden.Infrastructure;

namespace TaskGarden.Api.Application.Features.TaskList.Commands.CreateTaskList;

public record CreateTaskListCommand(string Name, string Description, string CategoryName)
    : IRequest<CreateTaskListResponse>;

public class CreateTaskListResponse
{
    public int TasklistId { get; set; }
}

public class CreateTaskListCommandHandler : AuthRequiredHandler,
    IRequestHandler<CreateTaskListCommand, CreateTaskListResponse>
{
    private readonly AppDbContext _context;
    private readonly IValidator<CreateTaskListCommand> _validator;
    private readonly IMapper _mapper;

    public CreateTaskListCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        AppDbContext context,
        IValidator<CreateTaskListCommand> validator,
        IMapper mapper
    ) : base(httpContextAccessor)
    {
        _context = context;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<CreateTaskListResponse> Handle(CreateTaskListCommand request, CancellationToken cancellationToken)
    {
        var userId = GetAuthenticatedUserId();

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var category = await GetCategoryAsync(userId, request.CategoryName)
                       ?? throw new NotFoundException("Category does not exist.");

        var newTaskList = new Domain.Entities.TaskList
        {
            Name = request.Name,
            Description = request.Description,
            CreatedById = userId,
        };

        var createdTaskList = await CreateTaskListAsync(userId, newTaskList)
                              ?? throw new ResourceCreationException("The task list could not be created.");

        if (!await _context.AssignCategoryAndTaskListAsync(userId, createdTaskList.Id, category.Id))
            throw new ResourceCreationException("The category could not be assigned to the task list.");

        if (!await AssignUserToTaskListAsync(userId, createdTaskList))
            throw new ResourceCreationException("The user could not be assigned to the task list.");


        return new CreateTaskListResponse
        {
            TasklistId = createdTaskList.Id,
        };
    }

    private Task<Category?> GetCategoryAsync(string userId, string categoryName) =>
        _context.Categories
            .FirstOrDefaultAsync(c => c.UserId == userId && c.Name.ToLower() == categoryName.ToLower());

    private async Task<Domain.Entities.TaskList> CreateTaskListAsync(string userId, Domain.Entities.TaskList taskList)
    {
        _context.TaskLists.Add(taskList);
        await _context.SaveChangesAsync();
        return taskList;
    }

    private async Task<bool> AssignUserToTaskListAsync(string userId, Domain.Entities.TaskList taskList)
    {
        _context.TaskListMembers.Add(new TaskListMember
        {
            UserId = userId,
            TasklistId = taskList.Id,
            Role = TaskListRole.Owner
        });

        return await _context.SaveChangesAsync() > 0;
    }
}