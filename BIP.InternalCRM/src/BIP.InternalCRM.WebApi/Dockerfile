#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/BIP.InternalCRM.WebApi/BIP.InternalCRM.WebApi.csproj", "src/BIP.InternalCRM.WebApi/"]
COPY ["src/BIP.InternalCRM.Application/BIP.InternalCRM.Application.csproj", "src/BIP.InternalCRM.Application/"]
COPY ["src/BIP.InternalCRM.Domain/BIP.InternalCRM.Domain.csproj", "src/BIP.InternalCRM.Domain/"]
COPY ["src/BIP.InternalCRM.Primitives/BIP.InternalCRM.Primitives.csproj", "src/BIP.InternalCRM.Primitives/"]
COPY ["src/BIP.InternalCRM.Infrastructure/BIP.InternalCRM.Infrastructure.csproj", "src/BIP.InternalCRM.Infrastructure/"]
COPY ["src/BIP.InternalCRM.Persistence/BIP.InternalCRM.Persistence.csproj", "src/BIP.InternalCRM.Persistence/"]
COPY ["src/BIP.InternalCRM.Shopify/BIP.InternalCRM.Shopify.csproj", "src/BIP.InternalCRM.Shopify/"]
RUN dotnet restore "src/BIP.InternalCRM.WebApi/BIP.InternalCRM.WebApi.csproj"
COPY . .
WORKDIR "/src/src/BIP.InternalCRM.WebApi"
RUN dotnet build "BIP.InternalCRM.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BIP.InternalCRM.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BIP.InternalCRM.WebApi.dll"]