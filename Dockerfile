FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/CollegeKingsBackend.Presentation/CollegeKingsBackend.Presentation.csproj", "src/CollegeKingsBackend.Presentation/"]
COPY ["src/CollegeKingsBackend.Application/CollegeKingsBackend.Application.csproj", "src/CollegeKingsBackend.Application/"]
COPY ["src/CollegeKingsBackend.Infrastructure/CollegeKingsBackend.Infrastructure.csproj", "src/CollegeKingsBackend.Infrastructure/"]
COPY ["src/CollegeKingsBackend.Domain/CollegeKingsBackend.Domain.csproj", "src/CollegeKingsBackend.Domain/"]
RUN dotnet restore "src/CollegeKingsBackend.Presentation/CollegeKingsBackend.Presentation.csproj"
COPY . .
WORKDIR "/src/src/CollegeKingsBackend.Presentation"
RUN dotnet build "CollegeKingsBackend.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CollegeKingsBackend.Presentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CollegeKingsBackend.Presentation.dll"]
