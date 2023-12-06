FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY Starter.Blazor.csproj .
RUN dotnet restore Starter.Blazor.csproj
COPY . .
RUN dotnet build Starter.Blazor.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Starter.Blazor.csproj -c Release -o /app/publish

FROM public.ecr.aws/nginx/nginx:latest AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf