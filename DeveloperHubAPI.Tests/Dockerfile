FROM mcr.microsoft.com/dotnet/core/sdk:3.1

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

ENV DynamoDb_LocalMode='true'
ARG LBHPACKAGESTOKEN
ENV LBHPACKAGESTOKEN=$LBHPACKAGESTOKEN

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY /nuget.config /root/.nuget/nuget/nuget.config
COPY ./DeveloperHubAPI.sln ./
COPY ./DeveloperHubAPI/DeveloperHubAPI.csproj ./DeveloperHubAPI/
COPY ./DeveloperHubAPI.Tests/DeveloperHubAPI.Tests.csproj ./DeveloperHubAPI.Tests/

RUN dotnet restore ./DeveloperHubAPI/DeveloperHubAPI.csproj
RUN dotnet restore ./DeveloperHubAPI.Tests/DeveloperHubAPI.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c debug -o out DeveloperHubAPI.Tests/DeveloperHubAPI.Tests.csproj

CMD dotnet test
