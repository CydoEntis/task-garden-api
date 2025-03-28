﻿// using MediatR;
// using TaskGarden.Application.Common.Exceptions;
// using TaskGarden.Domain.Enums;
// using TaskGarden.Infrastructure.Projections;
// using Microsoft.EntityFrameworkCore;
// using TaskGarden.Api.Application.Shared.Extensions;
// using TaskGarden.Api.Application.Shared.Handlers;
// using TaskGarden.Api.Application.Shared.Projections;
// using TaskGarden.Api.Domain.Entities;
// using TaskGarden.Api.Infrastructure.Services.Interfaces;
// using TaskGarden.Application.Common.Constants;
// using TaskGarden.Infrastructure;
//
// namespace TaskGarden.Api.Application.Features.Invitation.Commands.InviteUser;
//
// public record InviteUserCommand : IRequest<bool>
// {
//     public string InvitedUserEmail { get; init; } = default!;
//     public int TaskListId { get; init; }
// }
//
// public class InviteUserCommandHandler : AuthRequiredHandler, IRequestHandler<InviteUserCommand, bool>
// {
//     private readonly AppDbContext _context;
//     private readonly IEmailService _emailService;
//     private readonly ITokenService _tokenService;
//
//
//     public InviteUserCommandHandler(
//         IHttpContextAccessor httpContextAccessor,
//         AppDbContext context,
//         IEmailService emailService,
//         ITokenService tokenService,
//         IConfiguration configuration) : base(httpContextAccessor)
//     {
//         _context = context;
//         _emailService = emailService;
//         _tokenService = tokenService;
//     }
//
//     public async Task<bool> Handle(InviteUserCommand request, CancellationToken cancellationToken)
//     {
//         if (await InviteExistsAsync(request.InvitedUserEmail, request.TaskListId))
//             throw new ConflictException("An invitation already exists for this user.");
//
//         var userId = GetAuthenticatedUserId();
//         var user = await GetUserByIdAsync(userId);
//
//         if (user == null)
//             throw new NotFoundException("User not found.");
//
//
//         var taskList = await GetTaskListDetailsById(request.TaskListId);
//         if (taskList == null)
//             throw new NotFoundException("Task list not found");
//
//         if (await _context.TaskListMembers.IsMemberAsync(request.InvitedUserEmail, request.TaskListId))
//             throw new ConflictException("The user is already a member of this task list.");
//
//         var invitation = await CreateInviteAsync(taskList, request.InvitedUserEmail, user);
//         if (invitation is null)
//             throw new ResourceCreationException("Something went wrong while creating invite.");
//
//         await SendInviteEmail(invitation, request.InvitedUserEmail);
//
//         return true;
//     }
//
//     private async Task<bool> InviteExistsAsync(string email, int taskListId)
//     {
//         return await _context.Invitations
//             .AnyAsync(i => i.InvitedUserEmail == email &&
//                            i.TaskListId == taskListId &&
//                            i.Status == InvitationStatus.Pending);
//     }
//
//     private async Task<AppUser?> GetUserByIdAsync(string userId)
//     {
//         return await _context.Users.FindAsync(userId);
//     }
//
//     private async Task<TaskListPreview?> GetTaskListDetailsById(int id)
//     {
//         // Come back to this it can prob be improved by using the UserTaskListCategory table instead.
//         return await _context.TaskLists
//             .Where(q => q.Id == id)
//             .Select(tl => new TaskListPreview()
//             {
//                 Id = tl.Id,
//                 Name = tl.Name,
//                 Description = tl.Description,
//                 CategoryName = _context.UserTaskListCategories
//                     .Where(utc => utc.TaskListId == tl.Id)
//                     .Select(utc => utc.Category.Name)
//                     .FirstOrDefault(),
//                 CompletedTasksCount = tl.TaskListItems.Count(q => q.IsCompleted),
//                 TotalTasksCount = tl.TaskListItems.Count(),
//                 IsCompleted = tl.IsCompleted,
//                 CreatedAt = tl.CreatedAt,
//                 Members = tl.TaskListMembers
//                     .Select(tla => new Member
//                     {
//                         Id = tla.User.Id,
//                         Name = tla.User.FirstName + " " + tla.User.LastName,
//                     })
//                     .ToList(),
//                 TaskListItems = tl.TaskListItems.OrderBy(q => q.Position).Select(q => new TaskListItemDetail
//                 {
//                     Id = q.Id,
//                     Description = q.Description,
//                     IsCompleted = q.IsCompleted,
//                     Position = q.Position,
//                 }).ToList(),
//             })
//             .FirstOrDefaultAsync();
//     }
//
//     private async Task<Domain.Entities.Invitation> CreateInviteAsync(TaskListPreview taskList, string recipientsEmail,
//         AppUser inviter)
//     {
//         var invitation = new Domain.Entities.Invitation
//         {
//             TaskListId = taskList.Id,
//             InvitedUserEmail = recipientsEmail,
//             InviterUserId = inviter.Id,
//             Token = _tokenService.GenerateInviteToken(inviter, taskList.Id, taskList.Name, taskList.CategoryName,
//                 taskList.Members),
//             Status = InvitationStatus.Pending,
//             ExpiresAt = DateTime.UtcNow.AddDays(7)
//         };
//
//         await _context.Invitations.AddAsync(invitation);
//
//         await _context.SaveChangesAsync();
//         return invitation;
//     }
//
//
//     private async Task SendInviteEmail(Domain.Entities.Invitation invite, string recipientsEmail)
//     {
//         var inviteUrl = $"https://localhost:5173/invite/{invite.Token}";
//         var placeholders = new Dictionary<string, string>
//         {
//             { "Recipient's Email", recipientsEmail },
//             { "Invite Link", inviteUrl }
//         };
//
//
//         await _emailService.SendEmailAsync("Task Garden", recipientsEmail, "You’ve been invited!",
//             "InviteUserTemplate", placeholders);
//     }
// }