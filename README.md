# Lab10 - Arquitectura Hexagonal con .NET

Implementación de una API REST usando **Arquitectura Hexagonal (Clean Architecture)** con .NET 9.

## Estructura del Proyecto

```
lab10/
├── lab10.Domain/              # Capa de Dominio
│   ├── Entities/              # Entidades del dominio
│   │   ├── User.cs
│   │   └── Product.cs
│   └── Interfaces/            # Interfaces de repositorios
│       ├── IRepository.cs
│       ├── IUserRepository.cs
│       └── IProductRepository.cs
│
├── lab10.Application/         # Capa de Aplicación
│   ├── DTOs/                  # Data Transfer Objects
│   ├── Interfaces/            # Interfaces de servicios
│   └── Services/              # Lógica de negocio
│       ├── AuthService.cs
│       └── ProductService.cs
│
├── lab10.Infrastructure/      # Capa de Infraestructura
│   ├── Data/                  # DbContext
│   ├── Repositories/          # Implementación de repositorios
│   └── Configuration/         # Configuración de servicios
│
└── lab10/                     # Capa de Presentación (API)
    ├── Controllers/           # Controladores REST
    ├── Configuration/         # Configuración centralizada
    └── Program.cs             # Punto de entrada
```

## Características Implementadas

- ✅ Arquitectura Hexagonal (Clean Architecture)
- ✅ Patrón Repository
- ✅ JWT Authentication
- ✅ Swagger/OpenAPI Documentation
- ✅ Entity Framework Core con SQL Server
- ✅ Separación de responsabilidades en capas
- ✅ Inyección de dependencias
- ✅ DTOs para transferencia de datos

## Requisitos

- .NET SDK 8.0 o superior
- SQL Server o SQL Server LocalDB
- Visual Studio 2022, Rider, o VS Code

## Configuración Inicial

### 1. Restaurar paquetes

```bash
dotnet restore
```

### 2. Configurar la base de datos

Edita `lab10/appsettings.json` para configurar tu cadena de conexión:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=Lab10DB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 3. Crear la base de datos

```bash
# Instalar herramientas EF Core (si no las tienes)
dotnet tool install --global dotnet-ef

# Crear migración inicial
dotnet ef migrations add InitialCreate --project lab10.Infrastructure --startup-project lab10

# Aplicar migración
dotnet ef database update --project lab10.Infrastructure --startup-project lab10
```

### 4. Ejecutar la aplicación

```bash
dotnet run --project lab10
```

La API estará disponible en: `https://localhost:5001` o `http://localhost:5000`

Swagger UI: `https://localhost:5001/` (página principal)

## Endpoints de la API

### Autenticación

#### Registrar Usuario
```http
POST /api/Auth/register
Content-Type: application/json

{
  "username": "testuser",
  "email": "test@example.com",
  "password": "password123"
}
```

#### Login
```http
POST /api/Auth/login
Content-Type: application/json

{
  "username": "testuser",
  "password": "password123"
}
```

Respuesta:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "message": "Login successful"
}
```

### Productos (Requiere autenticación)

#### Obtener todos los productos
```http
GET /api/Products
Authorization: Bearer {token}
```

#### Obtener producto por ID
```http
GET /api/Products/{id}
Authorization: Bearer {token}
```

#### Crear producto
```http
POST /api/Products
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Laptop",
  "description": "High performance laptop",
  "price": 1200.00,
  "stock": 10
}
```

#### Actualizar producto
```http
PUT /api/Products/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Updated Laptop",
  "description": "Updated description",
  "price": 1300.00,
  "stock": 15
}
```

#### Eliminar producto
```http
DELETE /api/Products/{id}
Authorization: Bearer {token}
```

## Prueba con Swagger

1. Ejecuta la aplicación
2. Ve a `https://localhost:5001/`
3. Usa el endpoint `/api/Auth/register` para crear un usuario
4. Usa `/api/Auth/login` para obtener un token
5. Haz clic en el botón "Authorize" en Swagger
6. Ingresa: `Bearer {tu-token-aqui}`
7. Ahora puedes probar los endpoints de Products

## Arquitectura

### Capas

1. **Domain (Dominio)**
   - Entidades de negocio
   - Interfaces de repositorios
   - Lógica de dominio pura (sin dependencias externas)

2. **Application (Aplicación)**
   - DTOs
   - Interfaces de servicios
   - Lógica de aplicación
   - Casos de uso

3. **Infrastructure (Infraestructura)**
   - DbContext (Entity Framework)
   - Implementación de repositorios
   - Acceso a datos
   - Servicios externos

4. **Presentation (API)**
   - Controllers
   - Configuración de servicios
   - Middleware
   - Punto de entrada

### Patrones Implementados

- **Repository Pattern**: Abstracción del acceso a datos
- **Dependency Injection**: Inversión de control
- **DTO Pattern**: Separación entre entidades y datos de transferencia
- **Service Layer**: Lógica de negocio centralizada

## Configuración JWT

La configuración JWT se encuentra en `appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "SuperSecretKeyForLab10Application2025!@#$%",
    "Issuer": "lab10API",
    "Audience": "lab10Client",
    "ExpirationHours": 2
  }
}
```

## Notas Importantes

- El proyecto usa SQL Server LocalDB por defecto
- La autenticación es obligatoria para todos los endpoints de Products
- Los usuarios registrados tienen rol "User" por defecto
- El hash de contraseñas es simple (para producción usar BCrypt o similar)

## Próximos Pasos (Opcional)

- [ ] Implementar Unit of Work pattern
- [ ] Agregar encriptación BCrypt para contraseñas
- [ ] Implementar paginación en listados
- [ ] Agregar validaciones con FluentValidation
- [ ] Implementar logging con Serilog
- [ ] Agregar tests unitarios
- [ ] Implementar CQRS con MediatR

## Autor

Laboratorio 10 - Desarrollo de Aplicaciones Empresariales Avanzadas
TECSUP 2025-I
