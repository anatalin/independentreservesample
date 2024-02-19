FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Tests/Tests.csproj", "Tests/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Web/Web.csproj", "Web/"]
RUN dotnet restore "./Tests/Tests.csproj"

COPY . .

RUN dotnet build "./Tests/Tests.csproj" --verbosity normal

WORKDIR /src/Tests
ENTRYPOINT ["dotnet", "test", "--verbosity", "normal"]