﻿namespace Starter.Application.Contracts.Persistence.Services;
public interface ITaskService : ITransientService
{
    Task<bool> IsTaskAssignedToUser(string userId);
}
