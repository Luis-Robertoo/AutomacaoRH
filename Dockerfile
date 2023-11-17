FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /App

COPY ./ .

RUN dotnet restore

RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /App
COPY --from=build /App/out .

ENV ASPNETCORE_URLS=http://+:5000

EXPOSE 5000

ENTRYPOINT ["dotnet", "AutomacaoRecursosHumanos.dll"]