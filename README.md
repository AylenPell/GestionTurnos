# Gestión de Turnos — API Backend

Este proyecto es una API REST en **.NET 8** para gestionar pacientes, profesionales, solicitudes de turnos y listado de especialidades/estudios, con autenticación JWT, comunicaciones dentro de la platadorma y notificaciones por WhatsApp (Twilio).

---

## 1- Stack técnico

- **.NET 8 / ASP.NET Core Web API**
- **Entity Framework Core** (SQL Server como base principal)
- **JWT Bearer Authentication**
- **Polly** para resiliencia de llamadas externas
- **Twilio API** para envío de WhatsApp
- **Swagger/OpenAPI** para explorar endpoints

Referencia de configuración central: `API/Program.cs`.

---

## 2- Arquitectura general (estilo Clean Architecture)

La solución está separada en proyectos para mantener responsabilidades claras:

### `API/`
Capa de entrada HTTP.
- Define controllers y rutas.
- Configura auth, autorización por policies, CORS, Swagger, DI (Inyección de Dependencias).
- Ejecuta seeders al iniciar en entorno de desarrollo para la carga de datos necesarios para testing.

### `Application/`
Capa de casos de uso.
- Contiene servicios de negocio (`UserService`, `AppointmentService`, etc.).
- Define interfaces de repositorios y servicios externos (abstracciones).
- Implementa las reglas de negocio y validaciones necesarias para ejecutar correctamente cada caso de uso.

### `Domain/`
Capa de núcleo de negocio.
- Entidades (`User`, `Professional`, `Appointment`, etc.).
- Enums de negocio (`Roles`, `AppointmentStatus`).
- Sin dependencias de infraestructura.

### `Infrastracture/`
Implementación técnica.
- `GestorTurnosContext` (EF Core).
- Repositorios concretos.
- Integración con Twilio y políticas Polly.
- Migrations y seeders.

### `Contracts/`
DTOs de entrada/salida.
- Requests y responses que viajan entre controllers y servicios.
- Evita exponer entidades del dominio directamente al exterior.

---

## 3- Resumen flujo request

1. Llega al **Controller** (`API`).
2. El controller delega en un **Service** (`Application`).
3. El service aplica reglas y validaciones.
4. Si necesita persistencia, usa una interfaz de repositorio (`Application/Abstraction`) implementada en `Infrastracture`.
5. Devuelve DTOs (`Contracts`) al controller.
6. Controller responde HTTP.

Este patrón evita lógica de negocio en controllers y mantiene una separación limpia entre reglas y detalles técnicos.

---

## 4- Seguridad: autenticación y autorización

### Autenticación
- `POST /api/Authentication/Login` (usuarios)
- `POST /api/Authentication/ProfessionalLogin` (profesionales)

Ambos endpoints emiten JWT con claims relevantes (`sub`, `role`, etc.).

### Policies configuradas
- `ProfessionalPolicy`: Professional, SuperAdmin
- `UserPolicy`: User, SuperAdmin
- `UserAndAdminPolicy`: User, Admin, SuperAdmin
- `AdminPolicy`: Admin, SuperAdmin
- `SuperAdminPolicy`: solo SuperAdmin

Las policies se aplican en los controllers según el caso de uso.

---

## 5- Modelo funcional del sistema

### Entidades principales
- **User**: paciente/usuario final.
- **Professional**: profesionales de la salud (médicos).
- **Appointment**: solicitudes de turno.
- **Specialty**: especialidades médicas.
- **Study**: estudios médicos.
- **Role**: rol de acceso.

### Reglas relevantes
- Manejo de estados de turno (`Pendiente`, `EnCurso`, `Confirmado`, etc.) que hacen de puente para la comunicación interna dentro de la plataforma entre operadora y paciente.
- Validación de fechas y horarios de turnos.
- Validaciones de DNI, email, teléfono, fecha de nacimiento y contraseña.
- Soft delete (`IsActive`) para desactivación lógica en lugar de borrado físico, para no perder información y mantener la consistencia en la base de datos, ya que hay tablas relacionadas mediante foreing keys.

---

## 6- Listado de endpoints

---

### 6.1 Auth
Base: `/api/Authentication`

- `POST /api/Authentication/Login`
- `POST /api/Authentication/ProfessionalLogin`

---

### 6.2 User (self-service del usuario)
Base: `/User`

- `GET /User/me/appointments` > solicitudes de turnos del usuario autenticado
- `GET /User/me` > perfil propio
- `GET /User/me/dni` > perfil propio por DNI de claim
- `POST /User` (**anónimo**) > creación de usuario
- `PUT /User/me` > actualización de perfil propio
- `DELETE /User/me` > borrado lógico de cuenta propia

Policy del controller: `UserPolicy` (excepto `POST` que es `AllowAnonymous`).

---

### 6.3 Admin de usuarios
Base: `/AdminUser`

- `GET /AdminUser` > trae el listado de usuarios
- `GET /AdminUser/{id}` > busca un usuario por id
- `GET /AdminUser/dni/{dni}` > busca un usuario por dni
- `POST /AdminUser` > creación de usuario
- `PUT /AdminUser/{id}` > edición de un usuarior buscado por id
- `DELETE /AdminUser/{id}` > borrado lógico

Policy del controller: `AdminPolicy`.

---

### 6.4 SuperAdmin de usuarios
Base: `/SuperAdminUser`
Tiene las mismas funcionalidades que Admin
- `GET /SuperAdminUser`
- `GET /SuperAdminUser/{id}`
- `GET /SuperAdminUser/dni/{dni}`
- `POST /SuperAdminUser`
- `PUT /SuperAdminUser/{id}`
- `DELETE /SuperAdminUser/{id}`
+ la opción de revertir el borrado lógico
- `PATCH /SuperAdminUser/reactivate/{id}`

Policy del controller: `SuperAdminPolicy`.

---

### 6.5 Professionals
Base: `/Professional`

- `GET /Professional` > trae todos los profesionales
- `GET /Professional/{id}` > busca profesional por id
- `GET /Professional/license/{license}` > busca profesional por licencia médica
- `GET /Professional/specialty/{specialtyId}` (AdminPolicy) > busca por especialidad
- `POST /Professional` (AdminPolicy) > creación de profesional
- `PUT /Professional/{id}` (AdminPolicy) > edición de profesional
- `DELETE /Professional/{id}` (AdminPolicy) > borrado lógico
- `PATCH /Professional/reactivate/{id}` (AdminPolicy) > reversión de borrado lógico

---

### 6.6 Professional Schedule
Base: `/api/ProfessionalSchedule`

- `GET /api/ProfessionalSchedule/{id}` (ProfessionalPolicy) > trae la agenda del profesional por id
- `GET /api/ProfessionalSchedule/MySchedule` (ProfessionalPolicy) > trae la agenda del profesional autenticado
- `GET /api/ProfessionalSchedule/professional/{professionalId}` (AdminPolicy)

---

### 6.7 Appointments
Base: `/api/Appointment`

- `GET /api/Appointment` (UserAndAdminPolicy)
- `GET /api/Appointment/user/{userId}` (UserAndAdminPolicy)
- `POST /api/Appointment` (UserAndAdminPolicy)
- `PUT /api/Appointment/{id}` (AdminPolicy)
- `PATCH /api/Appointment/{id}/status` (Authorize)
- `DELETE /api/Appointment/{id}` (AdminPolicy)

Cuando cambia el estado de un turno, el servicio intenta enviar notificación por WhatsApp (Twilio).

---

### 6.8 Studies
Base: `/api/Study`

- `GET /api/Study`
- `GET /api/Study/{id}`
- `POST /api/Study` (AdminPolicy)
- `PUT /api/Study/{id}` (AdminPolicy)
- `DELETE /api/Study/{id}` (AdminPolicy)
- `PATCH /api/Study/reactivate/{id}` (AdminPolicy)

---

### 6.9 Specialties
Base: `/Specialty`

- `GET /Specialty`
- `GET /Specialty/{id}`
- `GET /Specialty/name/{name}`
- `POST /Specialty`
- `PUT /Specialty/{id}`
- `DELETE /Specialty/{id}`

Policy del controller: `AdminPolicy`.

---

### 6.10 Twilio (mensajería manual)
Base: `/api/TwilioWhatsApp`

- `POST /api/TwilioWhatsApp/send`

Permite disparar un mensaje a un usuario por `UserId`.

---

## 7) Configuración y ejecución local

### Requisitos
- SDK .NET 8
- SQL Server disponible (local o remoto)

### Variables de configuración (`API/appsettings.json`)
- `ConnectionStrings:DefaultConnection`
- `Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience`
- `Twilio:AccountSid`, `Twilio:AuthToken`, `Twilio:From`

### Comandos útiles
```bash
# restaurar paquetes
dotnet restore

# compilar solución
dotnet build GestionTurnos.sln

# ejecutar API
dotnet run --project API/API.csproj
```

Swagger queda disponible en entorno de desarrollo (`/swagger`).

---

## 8) Seed de datos y migraciones

Al iniciar en `Development`, la API ejecuta seeders para:
- Roles
- Pacientes
- Especialidades
- Estudios
- Profesionales
- Relación Professional-Specialty
- Turnos

Esto acelera pruebas funcionales y deja una base inicial consistente.

---

## 9) Observaciones técnicas (honestas)

- Hay mezcla de rutas con prefijo `/api` y rutas directas por nombre de controller (`/User`, `/Professional`, etc.). Está bien, pero conviene unificar criterio más adelante.
- Actualmente la contraseña se valida por reglas, pero no está hasheada en esta capa (tema recomendado para siguiente iteración).
- La estrategia de soft delete está bien aplicada para evitar pérdida de datos históricos.

---

## 10) Próximos pasos recomendados

1. Unificar convenciones de rutas y versionado (`/api/v1/...`).
2. Incorporar hash de contraseñas (ej. BCrypt/Argon2).
3. Sumar pruebas unitarias en servicios críticos (`AppointmentService`, `UserService`).
4. Agregar trazabilidad estructurada para integraciones externas (Twilio).
5. Documentar ejemplos de request/response por endpoint en Swagger o en una colección Postman.

---

Si querés, en una segunda iteración armamos una **guía de onboarding en 30 minutos** (qué leer primero, qué correr, y primer cambio sugerido para romper el hielo en el repo).