﻿using Starter.Blazor.Core.Response;
using Starter.Blazor.Modules.ChangePassword.Model;

namespace Starter.Blazor.Modules.ChangePassword.Services;

public interface IChangePasswordServices
{
    Task<ApiResponse<string>> ChangePasswordAsync(ChangePasswordRequest chnagePassword);
}
