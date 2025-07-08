FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Central.API/Central.API.csproj", "Central.API/"]
COPY ["Central.Application/Central.Application.csproj", "Central.Application/"]
COPY ["Central.Domain/Central.Domain.csproj", "Central.Domain/"]
COPY ["Central.Persistence/Central.Persistence.csproj", "Central.Persistence/"]
RUN dotnet restore "Central.API/Central.API.csproj"
COPY . .
WORKDIR "/src/Central.API"
RUN dotnet build "Central.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Central.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Central.API.dll"]