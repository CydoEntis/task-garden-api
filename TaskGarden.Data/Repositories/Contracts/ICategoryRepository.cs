﻿using TaskGarden.Data.Models;

namespace TaskGarden.Data.Repositories.Contracts;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<Category?> GetCategoryByCategoryNameAsync(string userId, string categoryName);
    Task<IEnumerable<Category>> GetAllCategoriesForUserAsync(string userId);
    Task<List<Category>> GetAllCategoriesWithTaskListsAsync(string userId, string categoryName);
    Task<List<CategoryWithCount>> GetCategoriesWithTaskListCountsForUserAsync(string userId);
}