using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Starter.Blazor.Core.Constants;

namespace Starter.Blazor.Core.Authentication;

public class AuthenticationHeaderHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorageService;
    public AuthenticationHeaderHandler(ILocalStorageService localStorageService)
        => _localStorageService = localStorageService;

    protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken
        )
    {
        if(request.Headers.Authorization?.Scheme == "Bearer")
        {
            var savedToken = await this._localStorageService.GetItemAsync<String>(StorageConstants.Local.AuthToken);
            if(!string.IsNullOrWhiteSpace(savedToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
            }
        }
        return await base.SendAsync(request, cancellationToken );
    }
}
