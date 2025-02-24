﻿using FluentValidation;
using TaskGarden.Application.Features.Shared.Constants;

namespace TaskGarden.Application.Features.Categories.Queries.GetAllTaskListsForCategory;

public class GetAllTaskListsForCategoryQueryValidator : AbstractValidator<GetAllTaskListsForCategoryQuery>
{
    public GetAllTaskListsForCategoryQueryValidator()
    {
        RuleFor(x => x.CategoryName)
            .NotEmpty().WithMessage("Category name is required");
    }
}