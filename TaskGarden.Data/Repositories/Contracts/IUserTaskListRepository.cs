﻿using TaskGarden.Data.Models;

namespace TaskGarden.Data.Repositories.Contracts;

public interface IUserTaskListRepository : IBaseRepository<UserTaskList>
{
    Task<int> GetTaskListCountByCategoryForUserAsync(string userId, string categoryName);
    Task<UserTaskList> GetUserTaskListByUserAndCategoryIdAsync(string userId, int categoryId);
    Task<string> GetUserRoleForTaskListAsync( string userId, int taskListId);
}