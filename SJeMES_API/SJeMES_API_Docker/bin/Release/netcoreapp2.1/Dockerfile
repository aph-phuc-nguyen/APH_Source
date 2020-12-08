FROM microsoft/dotnet:2.1-aspnetcore-runtime
FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-buster-slim
FROM mcr.microsoft.com/dotnet/core/sdk:2.1-buster


WORKDIR /app
COPY . .
EXPOSE 5010
ENTRYPOINT ["dotnet", "SJWebAPI_NETCore_Docker.dll"]