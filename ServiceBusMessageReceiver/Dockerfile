#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ServiceBusMessageReceiver/ServiceBusMessageReceiver.csproj", "ServiceBusMessageReceiver/"]
RUN dotnet restore "ServiceBusMessageReceiver/ServiceBusMessageReceiver.csproj"
COPY . .
WORKDIR "/src/ServiceBusMessageReceiver"
RUN dotnet build "ServiceBusMessageReceiver.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ServiceBusMessageReceiver.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServiceBusMessageReceiver.dll"]