#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["TF47-API.csproj", ""]
RUN dotnet restore "./TF47-API.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TF47-API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TF47-API.csproj" -c Release -o /app/publish

RUN apt-get update && apt-get upgrade
RUN apt-get install wine wine32

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TF47-API.dll"]
