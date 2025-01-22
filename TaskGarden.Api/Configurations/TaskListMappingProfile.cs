﻿using AutoMapper;
using TaskGarden.Api.Dtos.Auth;
using TaskGarden.Api.Dtos.Category;
using TaskGarden.Api.Dtos.TaskList;
using TaskGarden.Data.Models;

namespace TaskGarden.Api.Configurations;

public class TaskListMappingProfile : Profile
{
    public TaskListMappingProfile()
    {
        CreateMap<NewTaskListRequestDto, TaskList>().ReverseMap();
        CreateMap<NewTaskListResponseDto, TaskList>().ReverseMap();
        CreateMap<TaskListResponseDto, TaskList>().ReverseMap();
        CreateMap<NewTaskListRequestDto, TaskList>().ReverseMap();
    }
}