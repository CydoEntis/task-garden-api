﻿namespace TaskGarden.Api.Dtos.TaskList;

public class UpdateTaskListRequestDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int CategoryId { get; set; }
}