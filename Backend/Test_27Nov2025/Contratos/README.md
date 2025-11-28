# Contratos – DTOs y Tipos de Intercambio

## Descripción general

Este proyecto contiene los **contratos de datos (DTOs)** utilizados entre:

- La API Web (`Test_27Nov2025`).
- La capa de aplicación (`Aplicacion`).
- Potenciales clientes (Front-end, integraciones).

Está diseñado para ser una librería simple, sin lógica de negocio.

## Tipos de archivos que contiene

- `/Login/*.cs`
    - `LoginRequest`: Datos que envía el cliente para autenticarse.
    - `LoginResponse`: Estructura del resultado de login (usuario + token + tipo de token).

- `/Usuarios/*.cs`
    - `UsuarioDto`: DTO interno para la capa de aplicación (representa un usuario completo).
    - `UsuarioResponseDto`: DTO pensado para exponer en respuestas públicas (sin password).
    - Otros DTOs de creación/actualización de usuario (`CreateUsuarioRequestDto`, `UpdateUsuarioRequestDto`, si están definidos en esta carpeta).

- `/General/*.cs`
    - `ResultadoDto<T>`: Tipo genérico para representar resultados de operaciones:
        - `Exitoso` (Bool).
        - `Datos` (Valor cuando hay éxito).
        - `Errores` (Lista de mensajes cuando falla).

- `Contratos.csproj`
    - Proyecto de librería sin dependencias de infraestructura ni Web.

## Dependencias entre proyectos

- **Depende de:**
    - Solo de .NET base; no debe depender ni de Web ni de Infraestructura.

- **Proyectos que dependen de este:**
    - `Aplicacion` (Para definir firmas de servicios y respuestas).
    - `Test_27Nov2025` (Para tipos utilizados en controladores y Swagger).
    - Opcionalmente otros proyectos (por ejemplo un futuro cliente de consola o pruebas de integración).

## Estructura de carpetas

- `/Login`
    - DTOs relacionados con autenticación/seguridad.

- `/Usuarios`
    - DTOs relacionados con la entidad usuario (Peticiones y respuestas).

- `/General`
    - Tipos reutilizables por varios módulos (`ResultadoDto<T>`, etc.).

- Raíz:
    - `Contratos.csproj`
    - `README.md`.

## Ventajas de esta separación

- Evita acoplar los controladores Web a las entidades de dominio.
- Permite cambiar la forma de exponer datos sin afectar la lógica de negocio interna.
- Facilita serialización/deserialización en JSON y documentación en Swagger.