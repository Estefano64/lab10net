FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 10000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["lab10/lab10.csproj", "lab10/"]
COPY ["lab10.Application/lab10.Application.csproj", "lab10.Application/"]
COPY ["lab10.Domain/lab10.Domain.csproj", "lab10.Domain/"]
COPY ["lab10.Infrastructure/lab10.Infrastructure.csproj", "lab10.Infrastructure/"]
RUN dotnet restore "lab10/lab10.csproj"
COPY . .
WORKDIR "/src/lab10"
RUN dotnet build "lab10.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "lab10.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:10000
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "lab10.dll"]
