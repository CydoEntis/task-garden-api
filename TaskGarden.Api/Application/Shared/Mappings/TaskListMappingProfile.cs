﻿using AutoMapper;
using TaskGarden.Api.Application.Features.Categories.Queries.GetAllTaskListsForCategory;
using TaskGarden.Api.Application.Features.TaskList.Commands.CreateTaskList;
using TaskGarden.Api.Application.Features.TaskList.Commands.UpdateTaskList;
using TaskGarden.Api.Application.Features.TaskList.Queries.GetTaskListById;
// using TaskGarden.Api.Application.Features.TaskList.Queries.GetTaskListById;
using TaskGarden.Api.Application.Features.TaskListItem.Commands.CreateTaskListItem;
using TaskGarden.Api.Application.Features.TaskListItem.Commands.ReorderTaskListItem;
using TaskGarden.Api.Application.Shared.Models;
using TaskGarden.Api.Application.Shared.Projections;
using TaskGarden.Api.Domain.Entities;

namespace TaskGarden.Api.Application.Shared.Mappings;

public class TaskListMappingProfile : Profile
{
    public TaskListMappingProfile()
    {
        CreateMap<TaskList, TaskListOverview>()
            .ReverseMap();

        CreateMap<CreateTaskListCommand, TaskList>().ReverseMap();
        CreateMap<CreateTaskListResponse, TaskList>().ReverseMap();

        // CreateMap<GetTaskListByIdQueryResponse, TaskList>().ReverseMap();

        CreateMap<CreateTaskListCommand, TaskList>().ReverseMap();
        CreateMap<UpdateTaskListCommand, TaskList>()
            .ForMember(dest => dest.CreatedById, opt => opt.Ignore());

        CreateMap<TaskListOverview, GetAllTaskListsForCategoryResponse>().ReverseMap();
        // CreateMap<TaskListPreview, GetTaskListByIdQueryResponse>();
        CreateMap<TaskListMember, MemberResponse>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                $"{src.User.FirstName} {src.User.LastName}"));


        // Task List Items
        CreateMap<CreateTasklistItemCommand, TaskListItem>();
        CreateMap<TasklistItemDetail, TasklistItemResponse>();
        CreateMap<ReorderTasklistItemCommand, TaskListItem>();
        CreateMap<TaskListItem, TasklistItemDetail>();
    }
}