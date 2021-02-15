FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app

ENV ASPNETCORE_URLS http://*:9402
EXPOSE 9402

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /source
COPY ["src/Interface/Interface.csproj", "source/Interface/"]
RUN dotnet restore "source/Interface/Interface.csproj"
COPY . .
WORKDIR "/source/src/Interface"
RUN dotnet build "Interface.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Interface.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Interface.dll"]
