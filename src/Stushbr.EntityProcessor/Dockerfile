FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Stushbr.Scheduler/Stushbr.Scheduler.csproj", "Stushbr.Scheduler/"]
RUN dotnet restore "Stushbr.Scheduler/Stushbr.Scheduler.csproj"
COPY . .
WORKDIR "/src/Stushbr.Scheduler"
RUN dotnet build "Stushbr.Scheduler.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Stushbr.Scheduler.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Stushbr.Scheduler.dll"]
