#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["TF47_Api/TF47_Api.csproj", "TF47_Api/"]
RUN dotnet restore "TF47_Api/TF47_Api.csproj"
COPY . .
WORKDIR "/src/TF47_Api"
RUN dotnet build "TF47_Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TF47_Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TF47_Api.dll"]