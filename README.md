# Login-back

Este proyecto es una API backend para la gestión de usuarios, autenticación y roles en un sistema BootCamp. Utiliza ASP.NET Core, Entity Framework Core y MySQL como base de datos. Incluye autenticación JWT, gestión de perfiles de usuario, roles y soporte para refresh tokens. La autenticación está configurada para usar cookies http-only, por lo que no necesitas enviar el token en el header Authorization.

## Características principales
- Registro y autenticación de usuarios
- Asignación y gestión de roles (Admin, User, Profesor, RRHH)
- Asociación de usuarios con ubicaciones y perfiles
- Emisión y renovación de tokens JWT y refresh tokens
- Integración con Redis para caché
- Envío de correos electrónicos (configurable)

## Configuración
La configuración principal se encuentra en `Presentacion/appsettings.json`, donde puedes ajustar la cadena de conexión, parámetros de JWT y correo electrónico.

## Ejemplo de uso de endpoints

### Registro de usuario
```http
POST /api/auth/register
Content-Type: application/json
{
  "userName": "usuario1",
  "email": "usuario1@email.com",
  "password": "TuPassword123",
  "idLocation": 1,
  "roles": ["User"]
}
```

### Login


```http
POST /api/auth/login
Content-Type: application/json
{
  "userName": "usuario1",
  "password": "TuPassword123"
}
```

### Login
```
POST /api/auth/login
Content-Type: application/json
{
  "userName": "usuario1",
  "password": "TuPassword123"
}
```
### Renovar token
```
POST /api/auth/refresh
// No necesitas enviar 
Authorization, el token se 
gestiona por cookie http-only
```
### Obtener perfil de usuario
```
GET /api/user/profile/{id}
// El acceso se valida 
automáticamente por la cookie 
http-only
```
## Notas
- Todos los endpoints protegidos requieren que el usuario esté autenticado; la sesión se gestiona por cookies http-only.
- Los roles disponibles son: Admin, User, Profesor, RRHH.
- El backend corre por defecto en http://localhost:5125 y https://localhost:7175 .
