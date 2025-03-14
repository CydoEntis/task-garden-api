﻿using AutoMapper;

namespace TaskGarden.Application.MappingProfiles;


public class MapperConfig
{
    public static MapperConfiguration RegisterMappings()
    {
        return new MapperConfiguration(config =>
        {
            config.AddProfile(new AuthMappingProfile());
            config.AddProfile(new CategoryMappingProfile());
            config.AddProfile(new TaskListMappingProfile());
            config.AddProfile(new UserTaskListCategoryMappingProfile());
            config.AddProfile(new MemberMappingProfile());
            config.AddProfile(new UserTaskListCategoryMappingProfile());

        });
    }
}