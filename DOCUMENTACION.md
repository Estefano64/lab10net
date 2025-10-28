# Documentación del Laboratorio 10

## Alumno
**Laboratorio 10 - Arquitectura Hexagonal**
TECSUP - Desarrollo de Aplicaciones Empresariales Avanzadas 2025-I

---

## Estructura del Proyecto Implementada

### 1. Capa Domain (Dominio)
**Ubicación:** `lab10.Domain/`

**Responsabilidad:** Contiene las entidades de negocio y las interfaces de los repositorios. Es el núcleo del sistema y no depende de ninguna otra capa.

**Archivos creados:**
```
lab10.Domain/
├── Entities/
│   ├── User.cs              # Entidad de usuario
│   └── Product.cs           # Entidad de producto
└── Interfaces/
    ├── IRepository.cs       # Interfaz genérica de repositorio
    ├── IUserRepository.cs   # Interfaz específica para usuarios
    └── IProductRepository.cs # Interfaz específica para productos
```

**Características:**
- Entidades con propiedades bien definidas
- Interfaces de repositorio que definen contratos
- Sin dependencias externas (puro dominio)

---

### 2. Capa Application (Aplicación)
**Ubicación:** `lab10.Application/`

**Responsabilidad:** Contiene la lógica de negocio, servicios y DTOs. Orquesta el flujo de datos entre las capas.

**Archivos creados:**
```
lab10.Application/
├── DTOs/
│   ├── LoginDto.cs          # DTO para login
│   ├── RegisterDto.cs       # DTO para registro
│   ├── UserDto.cs           # DTO de usuario
│   ├── ProductDto.cs        # DTO de producto
│   └── CreateProductDto.cs  # DTO para crear producto
├── Interfaces/
│   ├── IAuthService.cs      # Interfaz del servicio de autenticación
│   └── IProductService.cs   # Interfaz del servicio de productos
└── Services/
    ├── AuthService.cs       # Implementación de autenticación y JWT
    └── ProductService.cs    # Implementación de lógica de productos
```

**Características:**
- Separación de DTOs y entidades de dominio
- Servicios que implementan lógica de negocio
- Generación de tokens JWT
- Validaciones de negocio

---

### 3. Capa Infrastructure (Infraestructura)
**Ubicación:** `lab10.Infrastructure/`

**Responsabilidad:** Implementa el acceso a datos y servicios externos. Contiene Entity Framework y repositorios concretos.

**Archivos creados:**
```
lab10.Infrastructure/
├── Data/
│   └── ApplicationDbContext.cs  # DbContext de EF Core
├── Repositories/
│   ├── Repository.cs            # Implementación genérica
│   ├── UserRepository.cs        # Implementación para usuarios
│   └── ProductRepository.cs     # Implementación para productos
└── Configuration/
    └── InfrastructureServicesExtensions.cs  # Configuración de servicios
```

**Características:**
- DbContext configurado con Entity Framework Core
- Implementación del patrón Repository
- Configuración de relaciones de base de datos
- Inyección de dependencias para repositorios

---

### 4. Capa Presentation (API)
**Ubicación:** `lab10/`

**Responsabilidad:** Punto de entrada de la aplicación. Contiene controllers y configuración de servicios.

**Archivos creados:**
```
lab10/
├── Controllers/
│   ├── AuthController.cs        # Endpoints de autenticación
│   └── ProductsController.cs    # Endpoints de productos
├── Configuration/
│   └── ServiceRegistrationExtensions.cs  # Configuración centralizada
├── Program.cs                   # Punto de entrada
└── appsettings.json            # Configuración de la aplicación
```

**Características:**
- Controllers con documentación Swagger
- Configuración de JWT Bearer Authentication
- Middleware de autorización
- Swagger UI integrado

---

## Patrones Implementados

### 1. Patrón Repository
Abstrae el acceso a datos y proporciona una interfaz limpia para operaciones CRUD.

**Ejemplo:**
```csharp
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
```

### 2. Dependency Injection
Todas las dependencias se inyectan a través del constructor, facilitando el testing y el desacoplamiento.

### 3. DTO Pattern
Separación entre entidades de dominio y objetos de transferencia de datos.

### 4. Service Layer Pattern
Encapsula la lógica de negocio en servicios reutilizables.

---

## Configuración de JWT

La autenticación JWT está configurada con los siguientes parámetros:

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

**Características de seguridad:**
- Tokens con expiración de 2 horas
- Validación de issuer y audience
- Claims incluidos: Sub, Jti, NameIdentifier, Name, Email, Role

---

## Endpoints Implementados

### Autenticación (No requiere token)

#### POST /api/Auth/register
Registra un nuevo usuario en el sistema.

**Request:**
```json
{
  "username": "testuser",
  "email": "test@example.com",
  "password": "password123"
}
```

**Response (200 OK):**
```json
{
  "message": "User registered successfully",
  "user": {
    "id": 1,
    "username": "testuser",
    "email": "test@example.com",
    "role": "User",
    "createdAt": "2025-10-27T10:00:00Z"
  }
}
```

#### POST /api/Auth/login
Autentica al usuario y devuelve un token JWT.

**Request:**
```json
{
  "username": "testuser",
  "password": "password123"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "message": "Login successful"
}
```

---

### Productos (Requiere autenticación)

#### GET /api/Products
Obtiene todos los productos.

**Headers:**
```
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "name": "Laptop HP",
    "description": "Laptop HP Core i7",
    "price": 1200.00,
    "stock": 15
  }
]
```

#### GET /api/Products/{id}
Obtiene un producto por su ID.

#### POST /api/Products
Crea un nuevo producto.

**Request:**
```json
{
  "name": "Mouse Logitech",
  "description": "Mouse inalámbrico",
  "price": 99.99,
  "stock": 50
}
```

#### PUT /api/Products/{id}
Actualiza un producto existente.

#### DELETE /api/Products/{id}
Elimina un producto.

---

## Pruebas con Swagger

### Paso 1: Ejecutar la aplicación
```bash
dotnet run --project lab10
```

### Paso 2: Abrir Swagger UI
Navega a: `https://localhost:5001/`

### Paso 3: Registrar un usuario
1. Abre el endpoint POST `/api/Auth/register`
2. Click en "Try it out"
3. Ingresa los datos del usuario
4. Click en "Execute"

### Paso 4: Hacer login
1. Abre el endpoint POST `/api/Auth/login`
2. Ingresa las credenciales
3. Copia el token de la respuesta

### Paso 5: Autorizar en Swagger
1. Click en el botón "Authorize" (candado verde)
2. Ingresa: `Bearer {tu-token-aquí}`
3. Click en "Authorize"
4. Click en "Close"

### Paso 6: Probar endpoints protegidos
Ahora puedes probar todos los endpoints de `/api/Products`

---

## Pruebas Realizadas

### Prueba 1: Registro de Usuario
✅ **Resultado:** Usuario registrado exitosamente
- Username único verificado
- Email único verificado
- Password hasheado correctamente
- Rol asignado por defecto: "User"

### Prueba 2: Login
✅ **Resultado:** Token JWT generado correctamente
- Validación de credenciales funcional
- Token con claims correctos
- Expiración configurada a 2 horas

### Prueba 3: Crear Producto (Autenticado)
✅ **Resultado:** Producto creado exitosamente
- Autorización JWT funcional
- Validación de datos correcta
- ID auto-generado

### Prueba 4: Listar Productos
✅ **Resultado:** Lista de productos obtenida
- Autorización requerida
- Datos completos retornados

### Prueba 5: Actualizar Producto
✅ **Resultado:** Producto actualizado
- Validación de existencia
- Cambios aplicados correctamente

### Prueba 6: Eliminar Producto
✅ **Resultado:** Producto eliminado
- Validación de existencia
- Eliminación física de la base de datos

---

## Observaciones y Conclusiones

### Observaciones

1. **Separación de Responsabilidades**
   - Cada capa tiene una responsabilidad clara y definida
   - El dominio no depende de detalles de implementación
   - Fácil de mantener y extender

2. **Testabilidad**
   - Las interfaces facilitan la creación de mocks
   - La inyección de dependencias permite pruebas unitarias
   - Cada capa puede probarse independientemente

3. **Escalabilidad**
   - Fácil agregar nuevas entidades siguiendo el patrón establecido
   - Los servicios son reutilizables
   - La configuración está centralizada

4. **Seguridad**
   - JWT implementado correctamente
   - Endpoints protegidos con [Authorize]
   - Roles configurables para autorización

5. **Documentación**
   - Swagger proporciona documentación interactiva
   - Comentarios XML en controllers
   - README completo con instrucciones

### Conclusiones

1. **Arquitectura Hexagonal es efectiva**
   - Proporciona una estructura clara y mantenible
   - Facilita la evolución del sistema sin afectar el dominio
   - La inversión de dependencias funciona correctamente

2. **Patrón Repository simplifica el acceso a datos**
   - Abstrae la complejidad de Entity Framework
   - Permite cambiar el ORM sin afectar la lógica de negocio
   - Facilita las pruebas unitarias

3. **JWT es adecuado para APIs REST**
   - Stateless y escalable
   - Fácil integración con diferentes clientes
   - Seguro con configuración adecuada

4. **Entity Framework Core funciona bien con el patrón**
   - DbContext encapsulado en Infrastructure
   - Migraciones fáciles de gestionar
   - LINQ proporciona consultas expresivas

5. **Swagger mejora la experiencia del desarrollador**
   - Documentación automática
   - Pruebas interactivas
   - Especificación OpenAPI generada

6. **Buenas prácticas implementadas**
   - DTOs para separar capas
   - Async/await para operaciones I/O
   - Try-catch para manejo de errores
   - Configuración centralizada

### Recomendaciones para Mejoras Futuras

1. **Implementar Unit of Work**
   - Para transacciones más complejas
   - Mayor control sobre el contexto

2. **Agregar validaciones con FluentValidation**
   - Validaciones más expresivas
   - Reutilización de reglas

3. **Implementar encriptación BCrypt**
   - Mayor seguridad en contraseñas
   - Hash con salt automático

4. **Agregar logging con Serilog**
   - Trazabilidad de operaciones
   - Debugging más fácil

5. **Implementar paginación**
   - Mejor rendimiento en listas grandes
   - Menor consumo de memoria

6. **Agregar tests unitarios**
   - xUnit o NUnit
   - Mocking con Moq
   - Alta cobertura de código

---

## Recursos Utilizados

- **.NET 9.0**
- **Entity Framework Core 9.0.10**
- **JWT Bearer Authentication**
- **Swashbuckle (Swagger) 9.0.6**
- **SQL Server**

## Autor

Laboratorio completado siguiendo los requerimientos de la guía del profesor Ing. Jhordan Leonardo Mayhua Ubillas.

**Fecha:** Octubre 2025
**Curso:** Desarrollo de Aplicaciones Empresariales Avanzadas
**Institución:** TECSUP
