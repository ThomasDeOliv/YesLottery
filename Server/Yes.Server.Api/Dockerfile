#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Yes.Server.Api/Yes.Server.Api.csproj", "Yes.Server.Api/"]
COPY ["Yes.Server.Scheduler/Yes.Server.Scheduler.csproj", "Yes.Server.Scheduler/"]
COPY ["Yes.Server.Datas.Business/Yes.Server.Datas.Business.csproj", "Yes.Server.Datas.Business/"]
COPY ["Yes.Server.Datas.DbAccess/Yes.Server.Datas.DbAccess.csproj", "Yes.Server.Datas.DbAccess/"]
COPY ["Yes.Server.Services.GameProvider/Yes.Server.Services.GameProvider.csproj", "Yes.Server.Services.GameProvider/"]
RUN dotnet restore "Yes.Server.Api/Yes.Server.Api.csproj"
COPY . .
WORKDIR "/src/Yes.Server.Api"
RUN dotnet build "Yes.Server.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Yes.Server.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Yes.Server.Api.dll"]