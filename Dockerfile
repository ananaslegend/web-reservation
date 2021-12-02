FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

#COPY "../WebReservation.sln" "WebReservation.sln"

COPY "WebReservation.API/*.csproj" "WebReservation.API/"
COPY "WebReservation.Data/*.csproj" "WebReservation.Data/"
RUN dotnet restore "WebReservation.API/WebReservation.API.csproj"

COPY "WebReservation.API/" "WebReservation.API/"
COPY "WebReservation.Data/" "WebReservation.Data/"
WORKDIR "/src/WebReservation.API/"
RUN dotnet build "WebReservation.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebReservation.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebReservation.API.dll"]
