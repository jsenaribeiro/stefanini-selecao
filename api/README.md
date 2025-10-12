# API

## Execução

```bash
cd api/Api.Infrastructure

# roda todos os serviços
docker compose --profile all up 

# faz o build e roda todos os serviços
docker compose --profile all up --build

# toda apenas o serviço de sqlserver
docker compose pup
```

## Test

```bash
cd ./api/tester.bat
```

## Migrations

```bash
cd ./api/migrate.bat nome_migration
```