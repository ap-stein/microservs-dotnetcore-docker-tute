FROM mcr.microsoft.com/dotnet/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["BandwithTester/BandwithTester.csproj", "BandwithTester/"]
RUN dotnet restore "BandwithTester/BandwithTester.csproj"
COPY . .
WORKDIR "/src/BandwithTester"
RUN dotnet build "BandwithTester.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BandwithTester.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BandwithTester.dll"]
