FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env
WORKDIR /app

COPY Paula_Cardona_SWD6.3A/PaulaCardonaSWD63A.csproj PaulaCardonaSWD63A/
COPY DataAccess/DataAccess.csproj DataAccess/
COPY Common/Common.csproj Common/
RUN dotnet restore PaulaCardonaSWD63A/PaulaCardonaSWD63A.csproj

COPY . ./
RUN dotnet publish PaulaCardonaSWD63A -c Release -o out
 
FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "PaulaCardonaSWD63A.dll"]