﻿using AutoMapper;
using TaskGarden.Api.Dtos.Category;
using TaskGarden.Api.Dtos.TaskList;
using TaskGarden.Api.Errors;
using TaskGarden.Api.Helpers;
using TaskGarden.Api.Services.Contracts;
using TaskGarden.Infrastructure.Enums;
using TaskGarden.Infrastructure.Models;
using TaskGarden.Infrastructure.Repositories.Contracts;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITaskListRepository _taskListRepository;
    private readonly ITaskListAssignmentRepository _taskListAssignmentRepository;
    private readonly ITaskListItemRepository _taskListItemRepository;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public CategoryService(ICategoryRepository categoryRepository,
        IMapper mapper, IUserContextService userContextService,
        ITaskListAssignmentRepository taskListAssignmentRepository, ITaskListRepository taskListRepository,
        ITaskListItemRepository taskListItemRepository)
    {
        _categoryRepository = categoryRepository;
        _taskListRepository = taskListRepository;
        _taskListItemRepository = taskListItemRepository;
        _taskListAssignmentRepository = taskListAssignmentRepository;
        _userContextService = userContextService;
        _mapper = mapper;
    }

    public async Task<NewCategoryResponseDto> CreateNewCategoryAsync(NewCategoryRequestDto dto)
    {
        var userId = _userContextService.GetUserId();
        if (userId == null)
            throw new UnauthorizedAccessException("User not authenticated");

        var existingCategory = await _categoryRepository.GetByNameAsync(userId, dto.Name);
        if (existingCategory is not null)
            throw new ConflictException("Category already exists");

        var category = _mapper.Map<Category>(dto);
        category.UserId = userId;

        await _categoryRepository.AddAsync(category);
        return new NewCategoryResponseDto() { Message = $"{category.Name} category has been created" };
    }

    public async Task<List<CategoryOverviewResponseDto>> GetAllCategoriesAsync()
    {
        var userId = _userContextService.GetUserId();
        if (userId == null)
            throw new UnauthorizedAccessException("User not authenticated");

        var categories = await _categoryRepository.GetAllCategoriesTaskListsAsync(userId);
        return _mapper.Map<List<CategoryOverviewResponseDto>>(categories);
    }


    public async Task<List<TaskListResponseDto>> GetAllTaskListsInCategory(string categoryName)
    {
        var userId = _userContextService.GetUserId();
        if (userId == null)
            throw new UnauthorizedAccessException("User not authenticated");

        var existingCategory = await _categoryRepository.GetByNameAsync(userId, categoryName);
        if (existingCategory is null)
            throw new NotFoundException("Category does not exist");

        var taskLists = await _taskListRepository.GetAllTaskListsInCategoryAsync(existingCategory.Id);

        return _mapper.Map<List<TaskListResponseDto>>(taskLists);
    }

    public async Task<UpdateCategoryResponseDto> UpdateCategoryAsync(UpdateCategoryRequestDto dto)
    {
        var userId = _userContextService.GetUserId();
        if (userId == null)
            throw new UnauthorizedAccessException("User not authenticated");

        var category = await _categoryRepository.GetAsync(dto.Id);
        if (category == null)
            throw new NotFoundException("Category not found.");

        if (category.UserId != userId)
            throw new PermissionException("You are not the owner of this category.");

        _mapper.Map(dto, category);
        await _categoryRepository.UpdateAsync(category);
        return new UpdateCategoryResponseDto { Message = $"{category.Name} category has been updated successfully", CategoryId = category.Id };
    }

    public async Task<DeleteCategoryResponseDto> DeleteCategoryAsync(int categoryId)
    {
        var userId = _userContextService.GetUserId();
        if (userId == null)
            throw new UnauthorizedAccessException("User not authenticated");

        var category = await _categoryRepository.GetAsync(categoryId);
        if (category == null)
            throw new NotFoundException("Category not found.");

        if (category.UserId != userId)
            throw new PermissionException("You are not the owner of this category.");

        var result = await _categoryRepository.DeleteCategoryAndDependenciesAsync(category);
        if (!result)
            throw new ResourceModificationException("Category could not be deleted.");
        
        return new DeleteCategoryResponseDto { Message = $"{category.Name} category has been deleted successfully" };
    }
}