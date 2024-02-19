#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Web/Web.csproj", "Web/"]
COPY ["Common/Common.csproj", "Common/"]
RUN dotnet restore "./Web/Web.csproj"

COPY . .

# run DB migrations
RUN dotnet tool install --version 8.0.2 --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet ef database update --project ./Common --startup-project ./Web --verbose

WORKDIR "/src/Web"
RUN dotnet build "./Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

RUN cp -r /src/Web/sqlite /app/publish
RUN chmod -R 777 /app/publish/sqlite

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV DOTNET_URLS=http://+:8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Web.dll"]