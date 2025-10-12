@echo off

if "%1"=="" ( echo "passe o nome da migracao" ) else (
   dotnet ef migrations add %1 -p Api.Infrastructure -s Api.Service
)