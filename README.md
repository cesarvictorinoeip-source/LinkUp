# LinkUp
# 💬 LinkUp

Red social desarrollada con **ASP.NET Core MVC**, siguiendo la **arquitectura Onion** y usando **ASP.NET Identity** para la gestión de usuarios. Permite publicar contenido, interactuar con amigos y jugar partidas de **Battleship** por turnos.

> Proyecto académico desarrollado en el Instituto Tecnológico de Las Américas (ITLA).

---

## ✨ Funcionalidades

### 🔐 Cuenta y seguridad
- Registro de usuarios con foto de perfil y validación de teléfono con formato dominicano.
- **Activación de cuenta por correo** antes del primer inicio de sesión.
- Restablecimiento de contraseña mediante un enlace con token enviado por correo.
- Edición del perfil: datos personales, foto y contraseña.
- Rutas protegidas con `[Authorize]`: solo usuarios autenticados acceden a la red social.

### 📝 Publicaciones
- Crear, editar y eliminar publicaciones con **texto + imagen** o **texto + video de YouTube**.
- Los videos se reproducen **dentro de la aplicación**, sin salir a YouTube.
- Comentarios y **respuestas anidadas** (hilos de conversación).
- Reacciones de **"Me gusta"** y **"No me gusta"**, con opción de cambiar la reacción.

### 👥 Amigos
- Feed con las publicaciones de los amigos, de la más reciente a la más antigua.
- Perfil de cada amigo con todas sus publicaciones.
- Eliminación de amigos con confirmación.

### 🤝 Solicitudes de amistad
- Envío de solicitudes con buscador de usuarios y cantidad de **amigos en común**.
- Aceptar, rechazar o eliminar solicitudes.
- Control para evitar solicitudes duplicadas entre los mismos usuarios.

### 🚢 Battleship
- Partidas por turnos entre amigos en un **tablero de 12×12**.
- **Fase de posicionamiento:** cada jugador ubica 5 barcos (de 2, 3, 3, 4 y 5 posiciones), con validación para que no se salgan del tablero ni se superpongan.
- **Fase de ataque:** turnos alternos con control estricto del turno; los aciertos se marcan en rojo y los fallos en verde.
- Un barco se hunde cuando se aciertan todas sus posiciones; gana quien hunda toda la flota rival.
- Opción de **rendirse**, y victoria automática si el rival no ataca en **48 horas**.
- **Historial** de partidas con estadísticas (jugadas, ganadas y perdidas) y visualización de los tableros finales de ambos jugadores.

---

## 🛠️ Tecnologías

| Área | Tecnologías |
|---|---|
| Backend | C#, ASP.NET Core MVC |
| Base de datos | SQL Server, Entity Framework Core (Code First) |
| Seguridad | ASP.NET Identity |
| Patrones | Repository Pattern, servicios genéricos |
| Mapeo | AutoMapper |
| Validaciones | ViewModels con Data Annotations |
| Interfaz | Bootstrap |

---

## 🏗️ Arquitectura

El proyecto sigue la **arquitectura Onion**, separando responsabilidades en capas:

```
Source
├── Core
│   ├── LinkUp.Core.Domain          → Entidades y reglas del dominio
│   └── LinkUp.Core.Application     → Lógica de negocio, servicios, DTOs e interfaces
├── Infrastructure
│   ├── LinkUp.Infrastructure.Identity     → Usuarios, autenticación y tokens
│   ├── LinkUp.Infrastructure.Persistence  → DbContext, repositorios y migraciones
│   └── LinkUp.Infrastructure.Shared       → Servicio de correo
└── Presentation
    └── LinkUp                             → Aplicación web MVC
```

---

## 🚀 Cómo ejecutarlo

### Requisitos
- [.NET SDK](https://dotnet.microsoft.com/download) (versión del proyecto)
- SQL Server
- Visual Studio 2022 (recomendado)

### Pasos

1. **Clona el repositorio**
   ```bash
   git clone https://github.com/cesarvictorinoeip-source/LinkUp.git
   ```

2. **Configura el `appsettings.json`** del proyecto `LinkUp`:
   - `ConnectionStrings` → tu servidor de SQL Server.
   - Configuración de correo → tu correo y contraseña de aplicación (necesario para la activación de cuentas y el restablecimiento de contraseña).

3. **Aplica las migraciones** desde la Consola del Administrador de paquetes, seleccionando como proyecto predeterminado las capas de **Persistence** e **Identity**:
   ```powershell
   Update-Database
   ```

4. **Ejecuta el proyecto** `LinkUp`.

---

## 👨‍💻 Autor

**César Tomás Victorino**
Estudiante de Desarrollo de Software — ITLA

- GitHub: [@cesarvictorinoeip-source](https://github.com/cesarvictorinoeip-source)
