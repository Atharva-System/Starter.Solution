using Blazored.LocalStorage;
using Microsoft.JSInterop;
using Starter.Blazor.Core.Constants;
using Starter.Blazor.Core.Response;
using Starter.Blazor.Core.Routes;
using Starter.Blazor.Core.Services.IServices;
using Starter.Blazor.Modules.Common;
using Starter.Blazor.Modules.User.Models;
using Starter.Blazor.Shared.Response;


namespace Starter.Blazor.Modules.User.Services;

public class UserService(IApiHandler api, ILocalStorageService localStorageService, IJSRuntime jsRuntime, INotificationService notificationService) : IUserService
{
    private readonly IApiHandler _api = api;
    private readonly ILocalStorageService _localStorageService = localStorageService;
    private readonly INotificationService _notificationService = notificationService;

    public async Task<ApiResponse<UpdateProfileDto>> GetProfileDetailAsync()
    {
        try
        {
            return await _api.Get<ApiResponse<UpdateProfileDto>>(UserEndpoints.GetProfile);
        }
        catch (Exception ex)
        {
            var errorResponse = await _api.ConvertStringToResponse<ApiResponse<object>>(ex.Message);
            await _notificationService.Failure(errorResponse.Messages);
            return null;
        }
    }

    public async Task<ApiResponse<UserlistDto>> GetUserDetailsByIdAsync(string userId)
    {
        try
        {
            return await _api.Get<ApiResponse<UserlistDto>>(UserEndpoints.GetById(userId));
        }
        catch (Exception ex)
        {
            var errorResponse = await _api.ConvertStringToResponse<ApiResponse<object>>(ex.Message);
            await _notificationService.Failure(errorResponse.Messages);
            return null;// Return null or handle error case appropriately
        }
    }

    public async Task<ApiResponse<string>> UpdateUserAsync(UserlistDto userDto)
    {
        try
        {
            return await _api.Put<ApiResponse<string>,UserlistDto>(UserEndpoints.Update(userDto.Id), userDto);
        }
        catch (Exception ex)
        {
            var errorResponse = await _api.ConvertStringToResponse<ApiResponse<string>>(ex.Message);
            await _notificationService.Failure(errorResponse.Messages);
            return errorResponse;// Return null or handle error case appropriately
        }
    }
    public async Task<ApiResponse<AcceptInviteDto>> GetAcceptInviteDetails(string userId)
    {
        try
        {
            return await _api.Get<ApiResponse<AcceptInviteDto>>(UserEndpoints.GetAcceptInviteDetails(userId));
        }
        catch (Exception ex)
        {
            var errorResponse = await _api.ConvertStringToResponse<ApiResponse<AcceptInviteDto>>(ex.Message);
            await _notificationService.Failure(errorResponse.Messages);
            errorResponse.Data = null;
            return errorResponse;// Return null or handle error case appropriately
        }
    }

    public async Task<PagedDataResponse<List<UserlistDto>>> GetUserlistsAsync(PaginationRequest param)
    {
        try
        {
            return await _api.Post<PagedDataResponse<List<UserlistDto>>, PaginationRequest>(UserEndpoints.Search, param);
        }
        catch (Exception ex)
        {
            var errorResponse = await _api.ConvertStringToResponse<PagedDataResponse<List<UserlistDto>>>(ex.Message);
            await _notificationService.Failure(errorResponse.Messages);
            errorResponse.Data = null;
            return errorResponse;// Return null or handle error case appropriately
        }
    }

    public async Task<ApiResponse<string>> AcceptInvite(UserRegisterDto userRegister)
    {
        try
        {
            return await _api.Post<ApiResponse<string>,UserRegisterDto>(UserEndpoints.AcceptInvite, userRegister);
        }
        catch (Exception ex)
        {
            var errorResponse = await _api.ConvertStringToResponse<ApiResponse<string>>(ex.Message);
            await _notificationService.Failure(errorResponse.Messages);
            errorResponse.Data = null;
            return errorResponse;// Return null or handle error case appropriately
        }
    }

    public async Task<ApiResponse<string>> InviteUserAsync(InviteUserDto userDto)
    {
        try
        {
            return await _api.Post<ApiResponse<string>,InviteUserDto>(UserEndpoints.InviteUser, userDto);
        }
        catch (Exception ex)
        {
            var errorResponse = await _api.ConvertStringToResponse<ApiResponse<string>>(ex.Message);
            await _notificationService.Failure(errorResponse.Messages);
            errorResponse.Data = null;
            return errorResponse;// Return null or handle error case appropriately
        }
    }

    public async Task<ApiResponse<string>> UpdateUserProfileAsync(UpdateProfileDto userDto)
    {
        try
        {
            userDto.Id = await _localStorageService.GetItemAsync<string>(StorageConstants.Local.Id);
            await _notificationService.Success("Update Profile update successful");
            return await _api.Put<ApiResponse<string>, UpdateProfileDto>(UserEndpoints.UpdateUserProfile(userDto.Id), userDto);
        }
        catch (Exception ex)
        {
            var errorResponse = await _api.ConvertStringToResponse<ApiResponse<string>>(ex.Message);
            await _notificationService.Failure(errorResponse.Messages);
            errorResponse.Data = null;
            return errorResponse;// Return null or handle error case appropriately
        }
    }

    public async Task<ApiResponse<string>> DeleteUser(string id)
    {
        try
        {
            return await _api.Delete<ApiResponse<string>>(UserEndpoints.DeleteUser(id));
        }
        catch (Exception ex)
        {
            var errorResponse = await _api.ConvertStringToResponse<ApiResponse<string>>(ex.Message);
            await _notificationService.Failure(errorResponse.Messages);
            errorResponse.Data = null;
            return errorResponse;// Return null or handle error case appropriately
        }
    }
}
