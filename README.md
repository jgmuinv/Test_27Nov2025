# Prueba Técnica – Backend .NET 8 (Test_27Nov2025)

## Descripción general

Esta solución implementa una API REST en **.NET 8** para la gestión de usuarios y el acceso a la aplicación mediante **autenticación JWT**.

La arquitectura está organizada en capas:

- **Contratos** – DTOs y tipos de intercambio.
- **Dominio** – Entidades y contratos de repositorio.
- **Infraestructura** – Acceso a datos (Dapper + PostgreSQL).
- **Aplicacion** – Servicios / casos de uso.
- **Test_27Nov2025** – API Web ASP.NET Core.

El objetivo es mantener una buena separación de responsabilidades, facilitando pruebas, mantenimiento y extensibilidad.

---

## Estructura de la solución
text / ├─Contratos/ # DTOs y tipos de intercambio ├─ Dominio/ # Entidades del dominio + interfaces de repositorios ├─ Infraestructura/ # Implementaciones de repositorios con Dapper / PostgreSQL ├─ Aplicacion/ # Servicios de aplicación (casos de uso) └─ Test_27Nov2025/ # API Web ASP.NET Core 8 + Swagger


Cada proyecto tiene su propio `README.md` con detalles específicos.  
A continuación se ofrece una visión de conjunto.

---

## 1. Contratos

**Ruta:** `Contratos/`

Responsabilidad:

- Definir los **Data Transfer Objects (DTOs)** utilizados entre el cliente, la API y la capa de aplicación.
- Mantener un esquema de tipos independiente de las entidades de dominio.

Ejemplos:

- `LoginRequest`, `LoginResponse`
- `UsuarioDto`, `UsuarioResponseDto`
- `ResultadoDto<T>` para encapsular resultados y errores

Depende únicamente de .NET.  
Es consumido por `Aplicacion` y `Test_27Nov2025`.

---

## 2. Dominio

**Ruta:** `Dominio/`

Responsabilidad:

- Modelar el **núcleo de negocio**.
- Definir entidades y reglas de negocio (por ejemplo, `Usuario`).
- Declarar interfaces de repositorios (`IUsuarioRepository`) sin conocer detalles de BD.

Conceptos clave:

- Entidad `Usuario` con:
  - Datos personales.
  - Estado (`A`/`I`).
  - Métodos de comportamiento: `Actualizar`, `CambiarPassword`, etc.

Consumido por `Aplicacion` e `Infraestructura`.

---

## 3. Infraestructura

**Ruta:** `Infraestructura/`

Responsabilidad:

- Implementar el acceso a datos usando **Dapper** y **Npgsql** sobre PostgreSQL.
- Crear conexiones a la BD mediante `IDbConnectionFactory` / `NpgsqlConnectionFactory`.
- Implementar `UsuarioRepository` (consulta, inserción, actualización, eliminación lógica).

Tecnologías:

- PostgreSQL
- Dapper
- Npgsql

Depende de:

- `Dominio` (para `Usuario` y `IUsuarioRepository`).

---

## 4. Aplicacion

**Ruta:** `Aplicacion/`

Responsabilidad:

- Implementar la lógica de **casos de uso** y orquestar el flujo entre dominio e infraestructura.
- Exponer servicios como `IUsuarioService` usados por los controladores de la API.
- Manejar la autenticación:
  - Verificación de password con `IPasswordHasher<Usuario>`.
  - Generación de **JWT** con `JwtSecurityTokenHandler`.

Principales tareas de `UsuarioService`:

- `LoginAsync`  
  Consulta usuario por teléfono, valida password y genera un token JWT.
- `CreateUserAsync` / `UpdateUserAsync` / `DeleteUserAsync` / `GetUserAsync` / `GetUsersAsync`  
  Implementan el CRUD de usuarios mapeando entre `Usuario` y los DTOs.

Depende de:

- `Dominio`
- `Contratos`

Es consumido por `Test_27Nov2025`.

---

## 5. Test_27Nov2025 (API Web)

**Ruta:** `Test_27Nov2025/`

Responsabilidad:

- Exponer la API REST al exterior.
- Configurar:
  - Inyección de dependencias.
  - Autenticación y autorización con JWT.
  - Swagger / OpenAPI.

### Endpoints principales

- `POST /Auth/Login/login`  
  Autenticación. Devuelve:
  - Datos del usuario autenticado.
  - `access_token` (JWT).
  - `token_type` (normalmente `"Bearer"`).

- `GET /Users/GetUsers`  
  Lista todos los usuarios (puede o no estar protegido según configuración).

- `GET /Users/GetUser/{id}`  
  Obtiene el detalle de un usuario. Protegido con `[Authorize]`.

- `POST /Users/CreateUser`  
  Crea un nuevo usuario. Protegido con `[Authorize]`.

- `PUT /Users/UpdateUser/{id}`  
  Actualiza un usuario existente. Protegido.

- `DELETE /Users/DeleteUser/{id}`  
  Marca el usuario como inactivo (`estado = 'I'`). Protegido.

---

## 6. Seguridad JWT

Configuración en `appsettings.json`:

json "Jwt": { "Issuer": "JoseMejia", "Audience": "TiendaWeb", "Secret": "dk5tn[8i[LJbcU`rC9$jJ0/6f@u9O$J-BzZDR4-D~!+mg*]J5;" }


Puntos clave:

- **Firma HS256** con una clave simétrica (`Secret`).
- El token incluye claims como:
  - `sub` (Id de usuario)
  - `unique_name` (correo)
  - `telefono`
- Validación en `Program.cs`:
  - Firma (`IssuerSigningKey`)
  - Emisor (`Issuer`)
  - Audiencia (`Audience`)
  - Tiempo de expiración

### Uso desde Swagger

1. Llamar a `POST /Auth/Login/login` con credenciales válidas.
2. Copiar el valor de `access_token`.
3. Pulsar botón **Authorize** en Swagger.
4. Pegar el token **(sin la palabra `Bearer`)**.
5. Ejecutar endpoints protegidos (como `CreateUser`).

Swagger enviará automáticamente:

http Authorization: Bearer {access_token}


---

## 7. Base de datos

### Tabla `usuarios` (resumen)

Campos típicos (según enunciado de la prueba):

- `id` (clave primaria, autoincremental)
- `nombres`, `apellidos`
- `fechanacimiento`
- `direccion`
- `password` (hash)
- `telefono`, `email` (con posibles índices únicos)
- `estado` (`A`/`I`)
- `fechacreacion`, `fechamodificacion` (manejadas por triggers)

### Scripts

Se recomienda mantener en una carpeta aparte (por ejemplo `BaseDeDatos/`):

- Script de creación de tabla `usuarios`.
- Script de secuencia/identity para `id`.
- Script de trigger/función para fechas de auditoría.

---

## 8. Cómo ejecutar la solución

1. **Base de datos**
   - Crear la base de datos `Test_27Nov2025`.
   - Ejecutar los scripts de creación de esquema y tabla `usuarios`.
   - Ajustar la cadena de conexión en `appsettings.json` si es necesario.

2. **Compilación y ejecución**

   Desde la raíz de la solución:

   ```bash
   dotnet restore
   dotnet build
   dotnet run --project Test_27Nov2025/Test_27Nov2025.csproj
   ```

3. **Probar desde Swagger**

   - Abrir `https://localhost:{puerto}/swagger`.
   - Autenticarse vía `Auth/Login/login`.
   - Autorizar con el token y consumir los endpoints de `Users`.

---

## 9. Buenas prácticas y extensiones futuras

- **Separación por capas**: permite sustituir fácilmente la infraestructura (por ejemplo, usar EF Core) sin cambiar dominio ni API.
- **DTOs explícitos**: evitan exponer internamente la entidad de dominio y permiten versionar la API de forma independiente.
- **Resultado tipado (`ResultadoDto<T>`)**: unifica la forma de devolver éxito/errores desde la capa de aplicación.

Posibles mejoras:

- Implementar pruebas unitarias para `UsuarioService` y `UsuarioRepository`.
- Añadir paginación y filtros a `GetUsers`.
- Auditar acciones de cambios (creación, edición, baja de usuarios).
- Documentar ejemplos de respuestas esperadas en el README o en la especificación OpenAPI.

---

## 10. Contacto / Notas finales

Este proyecto está preparado para ser evaluado como parte de una **prueba técnica de backend**.  
Los archivos `README.md` de cada proyecto contienen detalles específicos de su función y relaciones de dependencia.

Para cualquier duda o ajuste adicional (Por ejemplo, adaptación a otro proveedor de BD, despliegue en contenedores, etc.), se pueden extender las capas respetando la arquitectura ya establecida.