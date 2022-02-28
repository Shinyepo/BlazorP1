#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BlazorP1/Server/BlazorP1.Server.csproj", "BlazorP1/Server/"]
COPY ["BlazorP1/Shared/BlazorP1.Shared.csproj", "BlazorP1/Shared/"]
COPY ["BlazorP1/Client/BlazorP1.Client.csproj", "BlazorP1/Client/"]
RUN dotnet restore "BlazorP1/Server/BlazorP1.Server.csproj"
COPY . .
WORKDIR "/src/BlazorP1/Server"
RUN dotnet build "BlazorP1.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorP1.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorP1.Server.dll"]
