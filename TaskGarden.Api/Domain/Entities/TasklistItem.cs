﻿using System.ComponentModel.DataAnnotations.Schema;

namespace TaskGarden.Api.Domain.Entities;

public class TasklistItem : BaseEntity
{
    public string Description { get; set; }
    public bool IsCompleted { get; set; } = false;
    public int Position { get; set; }
    public required int TasklistId { get; set; }
    [ForeignKey("TasklistId")]
    public Tasklist Tasklist { get; set; }
    public string? CompletedById { get; set; }
    [ForeignKey("CompletedById")]
    public AppUser? CompletedBy { get; set; }
}