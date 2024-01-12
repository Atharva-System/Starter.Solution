﻿namespace Starter.Blazor.Core.Constants;

public static class StorageConstants
{
    public static class Local
    {
        public static string Preference = "clientPreference";

        public static string AuthToken = "authToken";
        public static string RefreshToken = "refreshToken";
        public static string Id = "id";
        public static string Username = "username";
        public static string Email = "email";
        public static string UserImageURL = "userImageURL";
    }

    public static class Server
    {
        public static string Preference = "serverPreference";

        //TODO - add
    }
}
