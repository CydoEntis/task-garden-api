﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TaskGarden.Application.Features.Auth.Commands.ChangePassword;
using TaskGarden.Application.Features.Auth.Commands.ForgotPassword;
using TaskGarden.Application.Features.Auth.Commands.Google;
using TaskGarden.Application.Features.Auth.Commands.Login;
using TaskGarden.Application.Features.Auth.Commands.Register;
using TaskGarden.Application.Features.Categories.Commands.CreateCategory;
using TaskGarden.Application.Features.Categories.Commands.DeleteCategory;
using TaskGarden.Application.Features.Categories.Commands.UpdateCategory;
using TaskGarden.Application.Features.Categories.Queries.GetAllTaskListsForCategory;
using TaskGarden.Application.Features.TaskList.Commands.CreateTaskList;
using TaskGarden.Application.Features.TaskList.Commands.DeleteTaskList;
using TaskGarden.Application.Features.TaskList.Commands.UpdateTaskList;
using TaskGarden.Application.Features.TaskListItem.Commands.CreateTaskListItem;
using TaskGarden.Application.Features.TaskListItem.Commands.UpdateTaskListItem;
using TaskGarden.Application.Features.TaskListItem.UpdateTaskListItemCompletedStatus;
using TaskGarden.Application.Services;
using TaskGarden.Application.Services.Contracts;

namespace TaskGarden.Application;

public static class ValidatorServiceRegistration
{
    public static IServiceCollection AddValidatorService(this IServiceCollection services)
    {
        services.AddScoped<IValidationService, ValidationService>();

        // Auth Validators
        services.AddValidatorsFromAssemblyContaining<RegisterCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<LoginCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<ChangePasswordCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<ForgotPasswordCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<GoogleLoginCommandValidator>();


        // Category Validators
        services.AddValidatorsFromAssemblyContaining<CreateCategoryCommand>();
        services.AddValidatorsFromAssemblyContaining<UpdateCategoryCommand>();
        services.AddValidatorsFromAssemblyContaining<DeleteCategoryCommand>();

        // Task List Validators
        services.AddValidatorsFromAssemblyContaining<GetAllTaskListsForCategoryQueryValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateTaskListCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateTaskListCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<DeleteTaskListCommandHandler>();


        // Task List Item Validators
        services.AddValidatorsFromAssemblyContaining<CreateTaskListItemCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateTaskListItemCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<DeleteTaskListCommandHandler>();
        services.AddValidatorsFromAssemblyContaining<UpdateTaskListItemCompletedStatusCommand>();


        return services;
    }
}