# Test_27Nov2025 – API Web (.NET 8)

## Descripción general

Este proyecto expone una API REST para la gestión de usuarios y autenticación con JWT.

Responsabilidades principales:

- Exponer endpoints HTTP (login y CRUD de usuarios).
- Aplicar autenticación y autorización con JWT.
- Orquestar el flujo entre capa de aplicación y cliente HTTP.
- Configurar middlewares de ASP.NET Core: routing, swagger, logging, etc.

## Tipos de archivos que contiene

- `Program.cs`  
  Punto de entrada de la aplicación. Configura:
    - Inyección de dependencias.
    - Autenticación / autorización (JWT).
    - Swagger / OpenAPI.
    - Pipelines de middlewares.

- `Controllers/*.cs`  
  Controladores Web API:
    - `AuthController`: endpoint de login que genera tokens JWT.
    - `UsersController`: endpoints CRUD de usuarios, protegidos con `[Authorize]` (salvo los que se decida abrir).

- `appsettings*.json`  
  Configuración de:
    - Cadenas de conexión a base de datos.
    - Parámetros de JWT (`Issuer`, `Audience`, `Secret`).
    - Niveles de logging.

- `*.csproj`  
  Definición del proyecto, dependencias de NuGet y referencias a otros proyectos de la solución.

## Dependencias entre proyectos

- **Depende de:**
    - `Aplicacion`  
      Para usar `IUsuarioService` y la lógica de negocio.
    - `Dominio`  
      (Indirectamente) para tipos compartidos usados por la capa de aplicación.
    - `Infraestructura`  
      Para registrar repositorios y la fábrica de conexiones a BD.
    - `Contratos`  
      Para los DTOs expuestos en las respuestas/requests de los controladores.

- **Proyectos que dependen de este:**  
  Ninguno. Es el proyecto de capa más externa (entry point).

## Estructura de carpetas

- `/Controllers`  
  Controladores REST. Cada controlador agrupa endpoints de un área funcional.

- `/Properties`  
  Archivos de configuración generados por ASP.NET Core (por ejemplo `launchSettings.json`).

- Raíz del proyecto:
    - `Program.cs`
    - `appsettings.json`
    - `appsettings.Development.json`
    - `Test_27Nov2025.csproj`
    - `README.md` (este archivo)

## Consideraciones de seguridad

- JWT firmado con algoritmo `HS256` y clave simétrica (`Jwt:Secret`).
- Los endpoints con `[Authorize]` requieren header:
  ```http
  Authorization: Bearer {token}
  ```
- El endpoint de login está marcado con `[AllowAnonymous]` para permitir la autenticación inicial.

## Cómo ejecutar

Desde la carpeta de la solución:

bash dotnet build dotnet run --project Test_27Nov2025/Test_27Nov2025.csproj

## La API se expone (en desarrollo) en:
- `https://localhost:7198/swagger` (Para documentación y pruebas interactivas).
