# Audisoft

Sistema de gestión de estudiantes, profesores y notas.

Andrés Beltrán — Ingeniero en Informática

## Tecnologías

- .NET 10 / ASP.NET Core Web API
- Entity Framework Core + SQL Server
- Blazor WebAssembly
- Docker / Docker Compose

## Arquitectura

Solución en capas:

- `Audisoft.Domain` — entidades del dominio.
- `Audisoft.Application` — lógica de negocio, DTOs, validaciones (FluentValidation).
- `Audisoft.Infrastructure` — acceso a datos (EF Core, repositorios, migraciones, seed).
- `Audisoft.Api` — API REST.
- `Audisoft.Web` — cliente Blazor WebAssembly.
- `Audisoft.Web.Host` — servidor mínimo que sirve el cliente Blazor (necesario para Docker).
- `tests` — pruebas unitarias.

## Ejecución con Docker

Requiere tener Docker Desktop instalado. No hace falta instalar SQL Server ni el SDK de .NET.

    docker compose up --build

Al finalizar, quedan disponibles:

| Servicio | URL |
|---|---|
| Aplicación web | http://localhost:5080 |
| API | http://localhost:7250 |
| Documentación de la API (Scalar) | http://localhost:7250/scalar/v1 |

> Las URL son http://, no https://. Si el navegador las autocompleta a https:// va a dar error de conexión — escríbelas a mano.

La base de datos se crea y se llena automáticamente con datos de ejemplo (12 estudiantes, 12 profesores, 30 notas) cada vez que se levanta el contenedor de la Api. No es necesario ejecutar ningún script a mano.

Para detener:

    docker compose down

## Script SQL

En sql/schema.sql está el script de creación de la base de datos (generado desde las migraciones de EF Core), incluido como documentación de la estructura.

## Funcionalidades

- CRUD de Estudiantes, Profesores y Notas.
- Paginación y filtros de búsqueda en cada listado.
- Validación de formularios con mensajes de error en el propio modal.
- Alertas (toast) de éxito/error al crear, editar o eliminar, con auto-cierre.
