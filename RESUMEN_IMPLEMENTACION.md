# Lab 10 - Arquitectura Hexagonal con MySQL

## Implementación Completada

### 1. Arquitectura Implementada

El proyecto sigue la **Arquitectura Hexagonal (Clean Architecture)** con las siguientes capas:

```
lab10/
├── lab10.Domain/           # Capa de Dominio (Entidades e Interfaces)
├── lab10.Application/      # Capa de Aplicación (DTOs y Servicios)
├── lab10.Infrastructure/   # Capa de Infraestructura (Repositorios y DbContext)
└── lab10/                  # Capa de Presentación (API y Controllers)
```

### 2. Base de Datos MySQL - ticketerabd

Se realizó el **scaffold** de la base de datos MySQL `ticketerabd` generando las siguientes entidades:

#### Entidades (Domain/Entities):
- **User** - Usuarios del sistema
  - UserId (Guid)
  - Username, Email, PasswordHash
  - CreatedAt

- **Ticket** - Tickets de soporte
  - TicketId (Guid)
  - UserId, Title, Description, Status
  - CreatedAt, ClosedAt

- **Response** - Respuestas a tickets
  - ResponseId (Guid)
  - TicketId, ResponderId, Message
  - CreatedAt

- **Role** - Roles del sistema
  - RoleId (Guid)
  - RoleName

- **UserRole** - Relación User-Role
  - UserId, RoleId (composite key)
  - AssignedAt

### 3. Patrón Repository Implementado

#### Interfaces de Repositorio (Domain/Interfaces):
- `IRepository<T>` - Repositorio genérico base
- `IUserRepository` - Operaciones específicas de usuarios
- `ITicketRepository` - Operaciones específicas de tickets
- `IResponseRepository` - Operaciones específicas de respuestas
- `IRoleRepository` - Operaciones específicas de roles

#### Implementaciones (Infrastructure/Repositories):
- `Repository<T>` - Implementación base con operaciones CRUD
- `UserRepository` - GetByUsername, GetByEmail
- `TicketRepository` - GetByUserId, GetByStatus, GetWithResponses
- `ResponseRepository` - GetByTicketId, GetByResponderId
- `RoleRepository` - GetByName

### 4. Servicios de Aplicación

#### Interfaces (Application/Interfaces):
- `IAuthService` - Autenticación y registro
- `ITicketService` - Gestión de tickets
- `IResponseService` - Gestión de respuestas

#### Implementaciones (Application/Services):
- **AuthService** - Login y registro con generación de JWT
- **TicketService** - CRUD completo de tickets + cerrar ticket
- **ResponseService** - CRUD de respuestas vinculadas a tickets

### 5. DTOs Implementados (Application/DTOs)

- **Authentication**:
  - `LoginDto` - Login de usuarios
  - `RegisterDto` - Registro de nuevos usuarios
  - `UserDto` - Respuesta con datos de usuario

- **Tickets**:
  - `TicketDto` - Datos completos del ticket
  - `CreateTicketDto` - Crear nuevo ticket
  - `UpdateTicketDto` - Actualizar ticket existente

- **Responses**:
  - `ResponseDto` - Datos completos de respuesta
  - `CreateResponseDto` - Crear nueva respuesta

### 6. Controllers REST API

#### AuthController
- `POST /api/auth/login` - Iniciar sesión
- `POST /api/auth/register` - Registrar nuevo usuario

#### TicketsController (Requiere JWT)
- `GET /api/tickets` - Listar todos los tickets
- `GET /api/tickets/{id}` - Obtener ticket por ID
- `GET /api/tickets/status/{status}` - Filtrar por estado
- `GET /api/tickets/my-tickets` - Tickets del usuario autenticado
- `POST /api/tickets` - Crear nuevo ticket
- `PUT /api/tickets/{id}` - Actualizar ticket
- `PATCH /api/tickets/{id}/close` - Cerrar ticket
- `DELETE /api/tickets/{id}` - Eliminar ticket

#### ResponsesController (Requiere JWT)
- `GET /api/responses/ticket/{ticketId}` - Respuestas de un ticket
- `GET /api/responses/{id}` - Obtener respuesta por ID
- `POST /api/responses/ticket/{ticketId}` - Crear respuesta
- `DELETE /api/responses/{id}` - Eliminar respuesta

### 7. Configuraciones

#### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ticketerabd;Uid=root;Pwd=root;Port=3306;"
  },
  "JwtSettings": {
    "SecretKey": "SuperSecretKeyForLab10Application2025!@#$%",
    "Issuer": "lab10API",
    "Audience": "lab10Client",
    "ExpirationHours": 2
  }
}
```

#### Infrastructure Configuration
- `InfrastructureServicesExtensions.cs` - Registro de DbContext y Repositorios
  - Conexión MySQL con Pomelo.EntityFrameworkCore.MySql
  - Registro de todos los repositorios con Dependency Injection

#### API Configuration
- `ServiceRegistrationExtensions.cs` - Registro centralizado de:
  - Servicios de aplicación
  - JWT Authentication
  - Swagger con soporte JWT

#### Program.cs
- Configuración limpia usando extensiones
- Middleware de autenticación y autorización
- Swagger UI en ruta raíz ("/")

### 8. Seguridad JWT

- **Autenticación**: Bearer Token JWT
- **Claims incluidos**:
  - Sub: Username
  - NameIdentifier: UserId (Guid)
  - Name: Username
  - Email: Email del usuario
  - Role: Rol del usuario

- **Configuración Swagger**: Botón "Authorize" para ingresar token JWT

### 9. Validaciones

Todos los DTOs incluyen validaciones con Data Annotations:
- `[Required]` - Campos obligatorios
- `[StringLength]` - Longitud máxima de strings
- `[RegularExpression]` - Validación de formatos

## Cómo Ejecutar

1. **Asegúrate de que MySQL está corriendo** en localhost:3306
2. **Verifica que existe la base de datos** `ticketerabd`
3. **Cierra cualquier proceso** que esté usando los archivos del proyecto
4. **Ejecuta el proyecto**:
   ```bash
   cd lab10
   dotnet run
   ```
5. **Accede a Swagger**: `https://localhost:XXXX/` (el puerto se muestra en consola)

## Flujo de Prueba en Swagger

1. **Registrar un usuario**: `POST /api/auth/register`
2. **Login**: `POST /api/auth/login` → Copiar el token JWT
3. **Autorizar en Swagger**: Click en "Authorize" → Pegar `Bearer {token}`
4. **Crear tickets**: `POST /api/tickets`
5. **Listar tickets**: `GET /api/tickets/my-tickets`
6. **Agregar respuestas**: `POST /api/responses/ticket/{ticketId}`
7. **Cerrar ticket**: `PATCH /api/tickets/{id}/close`

## Arquitectura Hexagonal - Flujo

```
Controller → Service (Application) → Repository → DbContext → MySQL
    ↓
   DTO    →   Entity (Domain)    →  Interface  →   Data
```

## Tecnologías Utilizadas

- **.NET 9.0**
- **Entity Framework Core 9.0.10**
- **Pomelo.EntityFrameworkCore.MySql** (Provider MySQL)
- **JWT Bearer Authentication**
- **Swagger/OpenAPI** con soporte JWT
- **MySQL 9.0.1**

## Patrones Implementados

✅ **Repository Pattern** - Abstracción de acceso a datos
✅ **Dependency Injection** - Inversión de dependencias
✅ **DTO Pattern** - Transferencia de datos
✅ **Service Layer** - Lógica de negocio
✅ **JWT Authentication** - Seguridad con tokens

## Observaciones

- El hash de contraseñas es simple (Base64). En producción usar **BCrypt** o **PBKDF2**
- Los roles están en la tabla `UserRole` pero se usa un rol por defecto "User" en JWT
- Se puede extender con **UnitOfWork pattern** (opcional en el laboratorio)
- Swagger está configurado para documentación automática

---

**Implementación completada para Lab 10 - JMAYHUA 2025-I**
