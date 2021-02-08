FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["APIGEO/APIGEO.csproj", "APIGEO/"]
RUN dotnet restore "APIGEO/APIGEO.csproj"
COPY . .
WORKDIR "/src/APIGEO"
RUN dotnet build "APIGEO.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "APIGEO.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "APIGEO.dll"]