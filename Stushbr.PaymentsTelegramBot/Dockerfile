FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Stushbr.PaymentsTelegramBot/Stushbr.PaymentsTelegramBot.csproj", "Stushbr.PaymentsTelegramBot/"]
RUN dotnet restore "Stushbr.PaymentsTelegramBot/Stushbr.PaymentsTelegramBot.csproj"
COPY . .
WORKDIR "/src/Stushbr.PaymentsTelegramBot"
RUN dotnet build "Stushbr.PaymentsTelegramBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Stushbr.PaymentsTelegramBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Stushbr.PaymentsTelegramBot.dll"]
