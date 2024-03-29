﻿using Starter.Blazor.Core.Response;
using Starter.Blazor.Modules.ForgotPassword.Models;

namespace Starter.Blazor.Modules.ForgotPassword.Services;

public interface IForgotPasswordService
{
    Task<ApiResponse<string>> ForgotPasswordAsync(ForgotPasswordDto forgotPassword);
}
