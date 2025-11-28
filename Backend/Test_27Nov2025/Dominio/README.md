# Dominio – Modelo de Dominio

## Descripción general

Este proyecto contiene el **modelo de dominio** y los contratos que definen cómo se accede a los datos.  
No conoce detalles de infraestructura (BD, web, etc.).

Responsabilidades:

- Modelar la entidad `Usuario` y su comportamiento.
- Declarar interfaces de repositorios (`IUsuarioRepository`).
- Definir tipos y utilidades de dominio (si aplica).

## Tipos de archivos que contiene

- `/Entidades/*.cs`
    - `Usuario.cs`: entidad que representa la fila de la tabla `usuarios` con reglas de negocio:
        - Métodos para actualizar datos.
        - Método `CambiarPassword` para cambiar el hash de contraseña.
        - Campos de auditoría (`FechaCreacion`, `FechaModificacion`).

- `/IRepositorios/*.cs`
    - `IUsuarioRepository.cs`: define las operaciones de acceso a datos:
        - `GetUser`, `GetUsers`, `CreateUser`, `UpdateUser`, `DeleteUser`, `GetByTelefono`.

- Otros directorios (por ejemplo `/General`) destinados a tipos compartidos de dominio.

- `Dominio.csproj`  
  Define que este proyecto es una librería de clases. Puede referenciar a `Contratos` si se requieren tipos compartidos, pero no referencia a la infraestructura.

## Dependencias entre proyectos

- **Depende de:**
    - Opcionalmente `Contratos` (si se reutilizan tipos, por ejemplo para resultados).

- **Proyectos que dependen de este:**
    - `Aplicacion` (para las entidades y contratos de repositorios).
    - `Infraestructura` (para implementar `IUsuarioRepository`).
    - `Test_27Nov2025` (indirectamente, a través de las capas anteriores).

## Estructura de carpetas

- `/Entidades`
    - Entidades del dominio. Actualmente:
        - `Usuario.cs`

- `/IRepositorios`
    - Interfaces de repositorio (contratos de persistencia agnósticos a la tecnología).
        - `IUsuarioRepository.cs`

- `/General` (si existe)
    - Elementos de dominio compartidos, helpers, etc.

- Raíz:
    - `Dominio.csproj`
    - `README.md`

## Notas de diseño

- El dominio implementa comportamiento (Métodos en `Usuario`), no solo propiedades.
- Mantiene las reglas de negocio cerca de los datos, facilitando pruebas unitarias.
- No depende de ninguna implementación concreta de BD (esto se resuelve en `Infraestructura`).