﻿namespace TaskGarden.Api.Dtos.Auth;

public class ResetPasswordRequestDto
{
    public string Email { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; }
}