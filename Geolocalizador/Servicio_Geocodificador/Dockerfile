FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Servicio_Geocodificador/Geocodificador.csproj", "Servicio_Geocodificador/"]
RUN dotnet restore "Servicio_Geocodificador/Geocodificador.csproj"
COPY . .
WORKDIR "/src/Servicio_Geocodificador"
RUN dotnet build "Geocodificador.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Geocodificador.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Geocodificador.dll"]