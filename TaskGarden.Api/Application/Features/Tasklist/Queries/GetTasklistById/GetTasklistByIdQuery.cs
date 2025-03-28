﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskGarden.Api.Application.Shared.Projections;
using TaskGarden.Application.Common.Exceptions;
using TaskGarden.Infrastructure;
using TaskGarden.Infrastructure.Projections;

namespace TaskGarden.Api.Application.Features.TaskList.Queries.GetTaskListById
{
    public record GetTasklistByIdQuery(int TaskListId) : IRequest<GetTaskListByIdQueryResponse>;

    public class GetTaskListByIdQueryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CompletedTasksCount { get; set; }
        public int TotalTasksCount { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Member> Members { get; set; } = new List<Member>();
        public List<TasklistItemDetail> TasklistItems { get; set; } = new List<TasklistItemDetail>();
        public string CategoryColor { get; set; }
    }

    public class GetTaskListByIdQueryHandler : IRequestHandler<GetTasklistByIdQuery, GetTaskListByIdQueryResponse>
    {
        private readonly AppDbContext _context;

        public GetTaskListByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GetTaskListByIdQueryResponse> Handle(GetTasklistByIdQuery request,
            CancellationToken cancellationToken)
        {
            var taskListResponse = await GetTaskListByIdAsync(request.TaskListId, cancellationToken);
            if (taskListResponse == null)
                throw new NotFoundException("Task list could not be found.");

            return taskListResponse;
        }

        private async Task<GetTaskListByIdQueryResponse?> GetTaskListByIdAsync(int taskListId,
            CancellationToken cancellationToken)
        {
            var taskListData = await _context.UserTasklistCategories
                .AsNoTracking()
                .Where(utc => utc.TaskListId == taskListId)
                .Include(utc => utc.Tasklist) // Ensure we get the Tasklist data
                .Include(utc => utc.Category) // Ensure we get the Category data
                .Select(utc => new GetTaskListByIdQueryResponse
                {
                    Id = utc.Tasklist.Id,
                    Name = utc.Tasklist.Name,
                    Description = utc.Tasklist.Description,
                    CreatedAt = utc.Tasklist.CreatedAt,
                    UpdatedAt = utc.Tasklist.UpdatedAt,
                    Members = utc.Tasklist.TaskListMembers
                        .Select(tlm => new Member
                        {
                            Id = tlm.User.Id,
                            Name = $"{tlm.User.LastName} {tlm.User.FirstName}"
                        })
                        .ToList(),
                    TotalTasksCount = utc.Tasklist.TaskListItems.Count(),
                    CompletedTasksCount = utc.Tasklist.TaskListItems.Count(ti => ti.IsCompleted),
                    TasklistItems = utc.Tasklist.TaskListItems
                        .OrderBy(q => q.Position)
                        .Select(q => new TasklistItemDetail
                        {
                            Id = q.Id,
                            Description = q.Description,
                            IsCompleted = q.IsCompleted,
                            Position = q.Position,
                        })
                        .ToList(),
                    CategoryColor = utc.Category.Color // Optional: Can keep it if needed separately
                })
                .FirstOrDefaultAsync(cancellationToken);

            return taskListData;
        }
    }
}