FROM mcr.microsoft.com/dotnet/core/sdk:3.1

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

ARG LBHPACKAGESTOKEN
ENV LBHPACKAGESTOKEN=$LBHPACKAGESTOKEN

WORKDIR /app

# Copy csproj and nuget config and restore as distinct layers
COPY ./DeveloperHubAPI.sln ./
COPY ./DeveloperHubAPI/DeveloperHubAPI.csproj ./DeveloperHubAPI/
COPY ./DeveloperHubAPI.Tests/DeveloperHubAPI.Tests.csproj ./DeveloperHubAPI.Tests/
COPY /NuGet.Config /root/.nuget/NuGet/NuGet.Config

RUN dotnet restore ./DeveloperHubAPI/DeveloperHubAPI.csproj
RUN dotnet restore ./DeveloperHubAPI.Tests/DeveloperHubAPI.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c debug -o out DeveloperHubAPI.Tests/DeveloperHubAPI.Tests.csproj

CMD dotnet test
