﻿using TaskGarden.Application.Common;
using TaskGarden.Domain.Entities;

namespace TaskGarden.Application.Services.Contracts;

public interface ISessionService
{
    Task<Session> GetSessionByRefreshTokenAsync(string refreshToken);
    Task<Session> GetSessionByUserIdAsync(string userId);
    Task<Session> CreateSessionAsync(string userId, RefreshToken refreshToken);
    Task InvalidateSessionAsync(Session session);
    Task<bool> ValidateRefreshToken(string refreshToken);
    Task InvalidateAllSessionsByUserIdAsync(string userId);
    Task<Session?> GetActiveSessionAsync(string userId);
}