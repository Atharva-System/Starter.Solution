FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src/blazor
COPY ["src/UI/Blazor/Starter.Blazor.csproj", "."]
RUN dotnet restore "./Starter.Blazor.csproj"
COPY ["src/UI/Blazor", "."]
WORKDIR /src/blazor
RUN dotnet build "./Starter.Blazor.csproj" -c Release -o /app/blazor/build

FROM build AS publish
RUN dotnet publish "./Starter.Blazor.csproj" -c Release -o /app/blazor/publish

FROM public.ecr.aws/nginx/nginx:latest AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/blazor/publish/wwwroot .
COPY ["src/UI/Blazor/nginx.conf", "/etc/nginx/nginx.conf"]
