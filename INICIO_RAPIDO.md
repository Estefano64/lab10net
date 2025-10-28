# Inicio Rápido - Lab10

## Pasos para ejecutar el proyecto

### 1. Configurar la base de datos

Tienes dos opciones:

#### Opción A: Usar SQL Server LocalDB (Recomendado para desarrollo)
```bash
# La cadena de conexión ya está configurada en appsettings.json
# Solo ejecuta las migraciones:
dotnet ef database update --project lab10.Infrastructure --startup-project lab10
```

#### Opción B: Usar SQL Server (Si LocalDB no está disponible)

1. Abre SQL Server Management Studio (SSMS)
2. Ejecuta el script `DatabaseScript.sql` que está en la raíz del proyecto
3. Actualiza la cadena de conexión en `lab10/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=Lab10DB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

O si usas autenticación SQL Server:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=Lab10DB;User Id=TU_USUARIO;Password=TU_PASSWORD;MultipleActiveResultSets=true"
  }
}
```

### 2. Ejecutar la aplicación

```bash
cd lab10
dotnet run
```

La aplicación se ejecutará en:
- **HTTPS:** https://localhost:5001
- **HTTP:** http://localhost:5000
- **Swagger:** https://localhost:5001/ (página principal)

### 3. Probar con Swagger

1. Abre tu navegador en: `https://localhost:5001/`

2. **Registrar usuario:**
   - Abre POST `/api/Auth/register`
   - Click "Try it out"
   - Usa este JSON:
   ```json
   {
     "username": "testuser",
     "email": "test@example.com",
     "password": "test123"
   }
   ```
   - Click "Execute"

3. **Hacer Login:**
   - Abre POST `/api/Auth/login`
   - Click "Try it out"
   - Usa este JSON:
   ```json
   {
     "username": "testuser",
     "password": "test123"
   }
   ```
   - Click "Execute"
   - **COPIA EL TOKEN** de la respuesta

4. **Autorizar en Swagger:**
   - Click en el botón **"Authorize"** (candado verde en la esquina superior derecha)
   - Escribe: `Bearer TU_TOKEN_AQUI` (reemplaza TU_TOKEN_AQUI con el token que copiaste)
   - Click "Authorize"
   - Click "Close"

5. **Probar endpoints de Products:**
   - Ahora puedes probar todos los endpoints GET, POST, PUT, DELETE de `/api/Products`

### 4. Probar con Postman/Insomnia (Opcional)

#### Registrar Usuario
```http
POST https://localhost:5001/api/Auth/register
Content-Type: application/json

{
  "username": "testuser",
  "email": "test@example.com",
  "password": "test123"
}
```

#### Login
```http
POST https://localhost:5001/api/Auth/login
Content-Type: application/json

{
  "username": "testuser",
  "password": "test123"
}
```

#### Crear Producto (requiere token)
```http
POST https://localhost:5001/api/Products
Content-Type: application/json
Authorization: Bearer TU_TOKEN_AQUI

{
  "name": "Laptop HP",
  "description": "Laptop Core i7",
  "price": 1200.00,
  "stock": 10
}
```

#### Listar Productos
```http
GET https://localhost:5001/api/Products
Authorization: Bearer TU_TOKEN_AQUI
```

## Datos de Prueba

Si ejecutaste el script SQL (`DatabaseScript.sql`), ya tienes estos usuarios:

### Usuario Admin
- **Username:** admin
- **Password:** test123
- **Email:** admin@lab10.com
- **Role:** Admin

### Usuario Normal
- **Username:** testuser
- **Password:** test123
- **Email:** test@lab10.com
- **Role:** User

Y productos de ejemplo:
- Laptop HP - $1200.00
- Mouse Logitech - $99.99
- Teclado Mecánico - $149.99
- Monitor Dell - $450.00
- Webcam Logitech - $79.99

## Estructura del Proyecto

```
lab10/
├── lab10.Domain/           # Entidades y contratos
├── lab10.Application/      # Lógica de negocio y DTOs
├── lab10.Infrastructure/   # Acceso a datos y EF Core
└── lab10/                  # API y Controllers
```

## Comandos Útiles

### Crear nueva migración
```bash
dotnet ef migrations add NombreMigracion --project lab10.Infrastructure --startup-project lab10
```

### Aplicar migraciones
```bash
dotnet ef database update --project lab10.Infrastructure --startup-project lab10
```

### Revertir última migración
```bash
dotnet ef migrations remove --project lab10.Infrastructure --startup-project lab10
```

### Ver migraciones aplicadas
```bash
dotnet ef migrations list --project lab10.Infrastructure --startup-project lab10
```

### Compilar proyecto
```bash
dotnet build
```

### Ejecutar tests (cuando se implementen)
```bash
dotnet test
```

## Solución de Problemas

### Error de conexión a la base de datos
- Verifica que SQL Server esté ejecutándose
- Revisa la cadena de conexión en `appsettings.json`
- Asegúrate de haber ejecutado las migraciones o el script SQL

### Error 401 Unauthorized
- Verifica que hayas hecho login y obtenido un token
- Verifica que el token esté en el header: `Authorization: Bearer {token}`
- Verifica que el token no haya expirado (duración: 2 horas)

### Swagger no se abre
- Verifica que estés usando HTTPS: `https://localhost:5001/`
- Si hay problemas con el certificado SSL, acepta el certificado en tu navegador

### El proyecto no compila
```bash
# Restaurar paquetes NuGet
dotnet restore

# Limpiar y reconstruir
dotnet clean
dotnet build
```

## Documentación Adicional

- **README.md** - Documentación completa del proyecto
- **DOCUMENTACION.md** - Documentación técnica detallada con conclusiones
- **DatabaseScript.sql** - Script SQL para crear la base de datos

## Contacto

Si tienes problemas, revisa los archivos de documentación o contacta al instructor del laboratorio.

---

**¡Listo para empezar! 🚀**
