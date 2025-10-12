FROM mcr.microsoft.com/dotnet/sdk:9.0 AS compilation
COPY . /src
WORKDIR /src/Api.Service
RUN dotnet publish -c Release -o /app /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS execution
ENV ASPNETCORE_URLS=http://+:80
WORKDIR /app
EXPOSE 80
EXPOSE 443
WORKDIR /app
COPY --from=compilation /app .
ENTRYPOINT dotnet Api.Service.dll
