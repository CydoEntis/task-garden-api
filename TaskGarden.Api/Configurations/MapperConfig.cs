﻿using AutoMapper;

namespace TaskGarden.Api.Configurations;

public class MapperConfig
{
    public static MapperConfiguration RegisterMappings()
    {
        return new MapperConfiguration(config => { config.AddProfile(new AuthMappingProfile()); });
    }
}