﻿using Microsoft.EntityFrameworkCore;
using TaskGarden.Application.Common.Contracts;
using TaskGarden.Domain.Entities;

namespace TaskGarden.Infrastructure.Repositories.Implementations;

public class TaskListAssignmentRepository : BaseRepository<TaskListAssignments>, ITaskListAssignmentRepository
{
    public TaskListAssignmentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<int> GetCountAsync(string userId, string categoryName)
    {
        return await _context.TaskListAssignments
            .Where(utl => utl.UserId == userId)
            .Include(utl => utl.TaskList)
            .Where(utl => utl.TaskList.Category.Name == categoryName)
            .CountAsync();
    }

    public async Task<string> GetAssignedRoleAsync(string userId, int taskListId)
    {
        var taskListUserRole = await _context.TaskListAssignments
            .Where(tl => tl.TaskListId == taskListId && tl.UserId == userId)
            .FirstOrDefaultAsync();

        return taskListUserRole.Role;
    }

    public async Task<TaskListAssignments?> GetByCategoryIdAsync(string userId, int categoryId)
    {
        return await _context.TaskListAssignments
            .Where(ut => ut.UserId == userId)
            .Include(ut => ut.TaskList)
            .FirstOrDefaultAsync(ut => ut.TaskList.CategoryId == categoryId);
    }

    // public async Task<List<TaskListAssignments>> GetByTaskListIdsAsync(List<int> taskListIds)
    // {
    //     return await _context.TaskListAssignments
    //         .Where(t => taskListIds.Contains(t.TaskListId))
    //         .ToListAsync();
    // }
}