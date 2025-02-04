﻿using TaskGarden.Data.Models;

namespace TaskGarden.Data.Repositories.Contracts;

public interface ITaskListAssignmentRepository : IBaseRepository<TaskListAssignments>
{
    Task<int> GetCountAsync(string userId, string categoryName);
    Task<string> GetAssignedRoleAsync(string userId, int taskListId);
    Task<TaskListAssignments?> GetByCategoryIdAsync(string userId, int categoryId);
    // Task<List<TaskListAssignments>> GetByTaskListIdsAsync(List<int> taskListIds);
}