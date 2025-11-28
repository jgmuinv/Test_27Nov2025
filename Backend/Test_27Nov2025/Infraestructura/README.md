# Infraestructura – Acceso a Datos (Dapper + PostgreSQL)

## Descripción general

Este proyecto implementa las **interfaces de repositorio del dominio** utilizando Dapper y PostgreSQL.

Responsabilidades:

- Configurar la conexión a base de datos (`NpgsqlConnectionFactory`).
- Implementar `IUsuarioRepository` con consultas SQL específicas.
- Mapear filas de la tabla `usuarios` a entidades `Usuario`.

## Tipos de archivos que contiene

- `/Data/DbConnectionFactory.cs`
    - `IDbConnectionFactory`: Interfaz para crear conexiones.
    - `NpgsqlConnectionFactory`: Implementación que usa `IConfiguration` y `NpgsqlConnection`.

- `/Repositorios/*.cs`
    - `UsuarioRepository.cs`: Implementación de `IUsuarioRepository` usando Dapper.
        - `GetUser`, `GetUsers`, `GetByTelefono`.
        - `CreateUser` (con `INSERT ... RETURNING id`).
        - `UpdateUser`, `DeleteUser` (Actualización lógica del campo `estado`).

- `Infraestructura.csproj`  
  Referencias importantes:
    - `Dapper`
    - `Npgsql`
    - `Dominio` (para usar `Usuario` e `IUsuarioRepository`).

## Dependencias entre proyectos

- **Depende de:**
    - `Dominio`  
      Para las entidades y contratos de repositorio.
    - Paquetes externos:
        - `Dapper`
        - `Npgsql`
        - `Microsoft.Extensions.Configuration.Abstractions` (vía DI).

- **Proyectos que dependen de este:**
    - `Test_27Nov2025` (API), que registra `UsuarioRepository` e `IDbConnectionFactory` en el contenedor de DI.

## Estructura de carpetas

- `/Data`
    - `DbConnectionFactory.cs`: Centraliza la creación de conexiones a PostgreSQL con la cadena `DefaultConnection`.

- `/Repositorios`
    - `UsuarioRepository.cs`: Implementación concreta de persistencia usando SQL parametrizado.

- Raíz:
    - `Infraestructura.csproj`
    - `Class1.cs` (Si existe; puede eliminarse o reutilizarse).
    - `README.md`.

## Notas de implementación

- Usar `CommandDefinition` con `CancellationToken` para mejorar cancelación en las operaciones.
- SQL preparado para mapear columnas de la tabla `usuarios` a las propiedades de `Usuario`.
- La lógica de auditoría (fechas de creación/modificación) puede apoyarse en triggers en la BD.