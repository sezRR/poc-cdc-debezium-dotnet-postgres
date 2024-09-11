FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src
COPY ["src/DebeziumDotnet.API/DebeziumDotnet.API.csproj", "src/DebeziumDotnet.API/"]
RUN dotnet restore "src/DebeziumDotnet.API/DebeziumDotnet.API.csproj"
COPY . .
WORKDIR "/src/src/DebeziumDotnet.API"
RUN dotnet build "DebeziumDotnet.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "DebeziumDotnet.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DebeziumDotnet.API.dll"]
