﻿using AutoMapper;

namespace TaskGarden.Api.Application.Shared.Mappings;

public class MapperConfig
{
    public static MapperConfiguration RegisterMappings()
    {
        return new MapperConfiguration(config =>
        {
            config.AddProfile(new AuthMappingProfile());
            config.AddProfile(new CategoryMappingProfile());
            config.AddProfile(new TasklistMappingProfile());
            config.AddProfile(new UserTasklistCategoryMappingProfile());
            config.AddProfile(new MemberMappingProfile());
            config.AddProfile(new UserTasklistCategoryMappingProfile());
        });
    }
}