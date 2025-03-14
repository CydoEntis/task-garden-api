﻿using FluentValidation;

namespace TaskGarden.Application.Features.TaskListItem.Commands.CreateTaskListItem;

public class CreateTaskListItemCommandValidator : AbstractValidator<CreateTaskListItemCommand>
{
    public CreateTaskListItemCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(250).WithMessage("Description must not exceed 250 characters.");

        RuleFor(x => x.TaskListId)
            .GreaterThan(0).WithMessage("TaskListId must be greater than zero.");
    }
}