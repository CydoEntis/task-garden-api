﻿using TaskGarden.Api.Domain.Entities;
using TaskGarden.Api.Infrastructure.Persistence;
using TaskGarden.Infrastructure;

namespace TaskGarden.Api.Application.Shared.Extensions
{
    public static class UserTaskListCategoryExtensions
    {
        public static async Task<bool> AssignCategoryAndTaskListAsync(
            this AppDbContext context,
            string userId,
            int taskListId,
            int categoryId)
        {
            await context.UserTaskListCategories.AddAsync(new UserTaskListCategory
            {
                UserId = userId,
                TaskListId = taskListId,
                CategoryId = categoryId
            });

            return await context.SaveChangesAsync() > 0;
        }

        public static async Task<bool> AssignCategoryAsync(
            this AppDbContext context,
            string userId,
            int categoryId)
        {
            await context.UserTaskListCategories.AddAsync(new UserTaskListCategory
            {
                UserId = userId,
                CategoryId = categoryId
            });

            return await context.SaveChangesAsync() > 0;
        }
    }
}