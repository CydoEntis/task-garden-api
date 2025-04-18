﻿using Microsoft.EntityFrameworkCore;
using Sprout.Api.Domain.Entities;
using Sprout.Api.Infrastructure.Persistence;
using Sprout.Infrastructure;

namespace Sprout.Api.Application.Shared.Extensions;

public static class CategoryExtensions
{
    public static async Task<bool> CategoryExistsAsync(this DbSet<Category> categories, string categoryName,
        string userId)
    {
        return await categories.AnyAsync(c => c.UserId == userId && c.Name.ToLower() == categoryName.ToLower());
    }

    public static async Task<Category?> GetByIdAsync(this DbSet<Category> categories, int categoryId)
    {
        return await categories.FirstOrDefaultAsync(c => c.Id == categoryId);
    }

    public static async Task<Category> CreateCategoryAsync(this AppDbContext context, Category category)
    {
        context.Categories.Add(category);
        await context.SaveChangesAsync();
        return category;
    }
}