#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["src/services/Volvo.Trucks.API/Volvo.Trucks.API.csproj", "src/services/Volvo.Trucks.API/"]
COPY ["src/buildingblocks/Volvo.Core/Volvo.Core.csproj", "src/buildingblocks/Volvo.Core/"]
COPY ["src/buildingblocks/Volvo.WebApi.Core/Volvo.WebApi.Core.csproj", "src/buildingblocks/Volvo.WebApi.Core/"]
RUN dotnet restore "src/services/Volvo.Trucks.API/Volvo.Trucks.API.csproj"
COPY . .
WORKDIR "src/services/Volvo.Trucks.API"
RUN dotnet tool install --global dotnet-ef
RUN dotnet build "Volvo.Trucks.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Volvo.Trucks.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Volvo.Trucks.API.dll"]
