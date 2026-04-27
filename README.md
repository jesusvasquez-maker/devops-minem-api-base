# DevOpsMinem API

Web API de gestión de usuarios construida en .NET 8 con arquitectura hexagonal básica.
Desarrollada como vehículo pedagógico para el curso **DevOps Fundamentals** — MINEM / New Horizons.

## Arquitectura

```text
Domain       →  Entidades y contratos (sin dependencias externas)
Application  →  Casos de uso y DTOs
Infrastructure → EF Core + SQLite
API          →  Controllers + Swagger
```

## Requisitos

- .NET 8 SDK
- dotnet-ef (`dotnet tool install --global dotnet-ef`)

## Ejecución local

```bash
git clone git@github.com:<usuario>/devops-minem.git
cd devops-minem
dotnet restore
dotnet ef database update --project src/DevOpsMinem.Infrastructure --startup-project src/DevOpsMinem.API
dotnet run --project src/DevOpsMinem.API
```

Swagger disponible en: http://localhost:5000/swagger

## Tests

```bash
dotnet test
```

## Endpoints

| Método | Ruta            | Descripción              |
|--------|-----------------|--------------------------|
| GET    | /api/users      | Listar todos los usuarios |
| GET    | /api/users/{id} | Obtener usuario por ID   |
| POST   | /api/users      | Registrar nuevo usuario  |
| PUT    | /api/users/{id} | Actualizar usuario       |
| DELETE | /api/users/{id} | Eliminar usuario         |

##Historia
> Modificado por Martín Valenzuela en el Laboratorio 2.
