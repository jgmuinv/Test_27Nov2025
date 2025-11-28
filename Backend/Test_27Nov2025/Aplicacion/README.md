# Aplicacion – Capa de Aplicación

## Descripción general

Este proyecto implementa la **lógica de aplicación** (Casos de uso) sobre el dominio.  
Coordina:

- Reglas de negocio a nivel de caso de uso.
- Mapeo entre entidades de dominio y DTOs.
- Integración con repositorios de la capa de infraestructura.

No conoce detalles de transporte HTTP ni de persistencia concreta.

## Responsabilidades principales

- Definir **interfaces de servicios** (por ejemplo `IUsuarioService`).
- Implementar servicios (`UsuarioService`) que:
    - Usan repositorios (`IUsuarioRepository`).
    - Transforman entidades de dominio ↔ DTOs.
    - Generan tokens JWT para el login.
- Manejar la estructura de respuesta estándar (`ResultadoDto<T>`).

## Tipos de archivos que contiene

- `/Interfaces/*.cs`  
  Contratos de servicios de aplicación (por ejemplo `IUsuarioService`).

- `/Servicios/*.cs`  
  Implementaciones de servicios:
    - `UsuarioService`:
        - Login (verificación de credenciales).
        - CRUD de usuarios.
        - Generación de tokens JWT usando `IConfiguration` y `Microsoft.IdentityModel.Tokens`.

- `Aplicacion.csproj`  
  Definición de dependencias:
    - Referencia a `Contratos` (DTOs).
    - Referencia a `Dominio` (entidades y repositorios).

## Dependencias entre proyectos

- **Depende de:**
    - `Contratos`  
      Para los DTOs de entrada y salida (`LoginRequest`, `LoginResponse`, `UsuarioDto`, etc.).
    - `Dominio`
        - Entidades (`Usuario`).
        - Interfaces de repositorio (`IUsuarioRepository`).

- **Proyectos que dependen de este:**
    - `Test_27Nov2025` (API Web), que consume `IUsuarioService`.

## Estructura de carpetas

- `/Interfaces`
    - `IUsuarioService.cs`: define los métodos de casos de uso que el Web API utilizará.

- `/Servicios`
    - `UsuarioService.cs`: implementación de `IUsuarioService`.
        - Usa `IUsuarioRepository` (Dominio + Infraestructura).
        - Usa `IPasswordHasher<Usuario>` para manejo seguro de passwords.
        - Usa `JwtSecurityToken` para emisión de tokens.

- Raíz:
    - `Aplicacion.csproj`
    - `Class1.cs` (si existe, se puede eliminar o reutilizar).
    - `README.md` (este archivo).

## Buenas prácticas implementadas

- Separación clara entre:
    - Dominio (reglas de negocio puras).
    - Aplicación (orquestación / casos de uso).
- Uso de DTOs para evitar exponer entidades de dominio directamente.
- Manejo unificado de resultados con `ResultadoDto<T>` para propagar errores y datos.