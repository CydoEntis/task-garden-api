﻿using MediatR;
using TaskGarden.Api.Application.Shared.Models;
using TaskGarden.Api.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using TaskGarden.Api.Application.Shared.Handlers;
using TaskGarden.Application.Common.Exceptions;
using TaskGarden.Infrastructure;

namespace TaskGarden.Api.Application.Features.TasklistMembers.Commands.UpdateUserRole;

public record UpdateUserRoleCommand(int TaskListId, string UserId, TaskListRole NewRole)
    : IRequest<UpdateUserRoleCommandResponse>;

public class UpdateUserRoleCommandResponse : BaseResponse
{
    public string Message { get; set; } = "User role updated successfully.";
}

public class UpdateUserRoleCommandHandler
    : AuthRequiredHandler, IRequestHandler<UpdateUserRoleCommand, UpdateUserRoleCommandResponse>
{
    private readonly AppDbContext _context;

    public UpdateUserRoleCommandHandler(IHttpContextAccessor httpContextAccessor, AppDbContext context)
        : base(httpContextAccessor)
    {
        _context = context;
    }

    public async Task<UpdateUserRoleCommandResponse> Handle(UpdateUserRoleCommand request,
        CancellationToken cancellationToken)
    {
        var userId = GetAuthenticatedUserId();

        var requestingUser = await _context.TasklistMembers
            .FirstOrDefaultAsync(m => m.UserId == userId && m.TasklistId == request.TaskListId, cancellationToken);

        if (requestingUser == null || requestingUser.Role == TaskListRole.Viewer)
            throw new PermissionException("You do not have permission to update roles.");

        var targetUser = await _context.TasklistMembers
            .FirstOrDefaultAsync(m => m.UserId == request.UserId && m.TasklistId == request.TaskListId,
                cancellationToken);

        if (targetUser == null)
            throw new NotFoundException("User is not a member of this task list.");

        if (targetUser.Role == TaskListRole.Owner)
            throw new PermissionException("Owners cannot have their role changed.");

        if (requestingUser.Role == TaskListRole.Editor && targetUser.Role == TaskListRole.Editor)
            throw new PermissionException("Editors cannot update the role of another Editor.");

        if (request.NewRole == TaskListRole.Owner)
            throw new PermissionException("Cannot assign the Owner role through this endpoint.");

        targetUser.Role = request.NewRole;
        _context.TasklistMembers.Update(targetUser);
        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateUserRoleCommandResponse();
    }
}