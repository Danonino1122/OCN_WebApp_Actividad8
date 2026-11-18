# Guía de despliegue - WebApp OCN (módulo RegistrosOCN)

**Proyecto:** `OCN_WebApp` — ASP.NET Core MVC + Entity Framework Core + SQL Server
**Módulo trabajado:** RegistrosOCN (CRUD con llaves foráneas hacia Zona y Problematica)
**Materia:** Bases de Datos Avanzadas · Actividad 8 · Universidad de Montemorelos

---

## 1. Requisitos

| Requisito | Versión usada en el desarrollo | Nota |
|---|---|---|
| .NET SDK | 11.0.100-rc.3 | Para ejecutar solo la publicación basta el **ASP.NET Core Runtime 11**. |
| SQL Server | SQL Server 2025 Developer (instancia `MSSQLSERVER`, `localhost`) | Sirve cualquier edición, incluida Express. |
| SSMS | Cualquier versión reciente | Para ejecutar los scripts y revisar los datos. |
| Herramienta EF (opcional) | `dotnet-ef` 11.0.0 | Solo si se crea la base con migración en lugar de script: `dotnet tool install --global dotnet-ef` |
| Editor (opcional) | VS Code con C# Dev Kit, o Visual Studio | Solo para abrir el código. |

**Base de datos:** `OCN_WebApp_DB` (base de prueba propia, con 3 tablas: `Zonas`, `Problematicas` y `RegistrosOCN`).

---

## 2. Configuración

La cadena de conexión se llama **`OcnConnection`** y está en **`OCN_WebApp/appsettings.json`**:

```json
"ConnectionStrings": {
  "OcnConnection": "Server=localhost;Database=OCN_WebApp_DB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

- Usa **autenticación de Windows** (`Trusted_Connection=True`), así que **no contiene contraseñas**.
- Si su SQL Server tiene otro nombre de instancia, cambie solo `Server=`. Por ejemplo: `Server=localhost\SQLEXPRESS`.
- Si usa autenticación de SQL Server, reemplace `Trusted_Connection=True` por `User Id=<usuario>;Password=<contraseña>` (**nunca** suba la contraseña real al repositorio).
- **¿Por qué en `appsettings.json` y no en `appsettings.Development.json`?** La versión publicada corre en modo *Production* y **solo lee `appsettings.json`**. Así, una sola cadena sirve para desarrollo y para la publicación.
- Si ya se publicó la app, el mismo cambio debe hacerse en `Publicacion/appsettings.json`.

---

## 3. Base de datos

En SSMS, conectado a la instancia, ejecute **en este orden** los scripts de la carpeta `Scripts/`:

| Orden | Script | Qué hace |
|---|---|---|
| 1 | `01_CreacionBD_Migracion.sql` | Crea la base `OCN_WebApp_DB` y las 3 tablas con sus llaves primarias y foráneas. Es el equivalente en SQL de la migración `InitialCreate` (generado con `dotnet ef migrations script --idempotent`). Se puede ejecutar más de una vez sin duplicar nada. |
| 2 | `02_DatosSemilla.sql` | Borra los datos existentes y carga **3 zonas, 3 problemáticas y 10 registros** anónimos. Reinicia los Id para que siempre queden del 1 al 10. Se puede re-ejecutar para dejar la base limpia antes de probar. |

**Alternativa con Entity Framework** (en lugar del script 1), desde la carpeta `OCN_WebApp`:

```powershell
dotnet ef database update
```

**Verificación en SSMS:**

```sql
USE OCN_WebApp_DB;
SELECT (SELECT COUNT(*) FROM Zonas)         AS Zonas,
       (SELECT COUNT(*) FROM Problematicas) AS Problematicas,
       (SELECT COUNT(*) FROM RegistrosOCN)  AS Registros;
-- Resultado esperado:  3 | 3 | 10
```

---

## 4. Ejecución

**Opción A: desde el código fuente (modo desarrollo)**

```powershell
cd OCN_WebApp
dotnet run --launch-profile http
```

La app queda en **http://localhost:5025**.

**Opción B: desde la publicación (modo producción)**

```powershell
# 1) Generar la publicación (una sola vez, desde la carpeta OCN_WebApp)
cd OCN_WebApp
dotnet publish -c Release -o ..\Publicacion

# 2) Ejecutar lo publicado (sin necesidad del código fuente)
cd ..\Publicacion
dotnet OCN_WebApp.dll --urls "http://localhost:5050"
```

Debe aparecer:

```
Now listening on: http://localhost:5050
Hosting environment: Production
Application started. Press Ctrl+C to shut down.
```

Para detener la app, presione **Ctrl+C** en la terminal.

---

## 5. Verificación

**Ruta de prueba:** `/RegistrosOCN` (por ejemplo, http://localhost:5050/RegistrosOCN). También se llega desde el enlace **"Registros OCN"** del menú superior.

| Prueba | Cómo se hace | Resultado esperado |
|---|---|---|
| Index | Abrir `/RegistrosOCN` | Tabla con los 10 registros, con los **nombres** de zona y problemática. |
| Details | Clic en "Ver" | Datos completos de un registro. |
| Create | Clic en "+ Nuevo registro", elegir zona y problemática en los **dropdowns** y guardar | Regresa a Index con el registro nuevo al final. |
| Edit | Clic en "Editar", cambiar datos y guardar | Los dropdowns abren con la opción actual marcada y el cambio se ve en Index. |
| Delete | Clic en "Borrar" y luego "Sí, eliminar" | Pantalla de confirmación; el registro desaparece de Index. |
| Validación de rango | En Create/Edit, poner Intensidad = 9 o Frecuencia = −2 | Mensaje en rojo "entre 1 y 5"; no se guarda. |
| Validación de requeridos | En Create, dejar un dropdown en "-- Elige --" | Mensaje "Este campo es obligatorio"; no se guarda. |
| AntiForgeryToken | Inspeccionar el formulario de Create, Edit o Delete | Campo oculto `__RequestVerificationToken`. |
| Registro inexistente | Abrir `/RegistrosOCN/Details/999` | Error 404 (no encontrado). |

---

## 6. Checklist de verificación (completado)

| # | Punto | Estado | Evidencia |
|---|---|---|---|
| 1 | Proyecto ASP.NET Core MVC compila sin errores | ✅ | `dotnet build`: 0 errores |
| 2 | Base `OCN_WebApp_DB` creada con migración `InitialCreate` (3 tablas, 2 FK) | ✅ | Captura SSMS |
| 3 | Datos semilla cargados (3 zonas, 3 problemáticas, 10 registros) | ✅ | Captura SSMS |
| 4 | Index muestra registros con nombre de Zona y Problematica (navegación) | ✅ | Captura Index |
| 5 | Details muestra un registro | ✅ | Captura Details |
| 6 | Create con dropdowns de Zona y Problematica guarda correctamente | ✅ | Captura Create |
| 7 | Edit con dropdowns (opción actual seleccionada) guarda correctamente | ✅ | Captura Edit |
| 8 | Delete muestra confirmación (GET) y borra por POST (`DeleteConfirmed`) | ✅ | Captura Delete |
| 9 | `[Range(1,5)]` con mensaje propio en Intensidad y Frecuencia | ✅ | Captura datos inválidos |
| 10 | `[Required]` en ZonaId y ProblematicaId (`int?`) | ✅ | Captura datos inválidos |
| 11 | `ModelState.IsValid` antes de guardar; listas de FK recargadas si es inválido | ✅ | Código del controlador |
| 12 | AntiForgeryToken en Create, Edit y Delete + `[ValidateAntiForgeryToken]` | ✅ | Capturas de inspección (×3) |
| 13 | POST de Delete **sin token** es rechazado | ✅ | Respuesta 400; el registro no se borró |
| 14 | Registro inexistente devuelve 404 (Details, Edit, Delete) | ✅ | Prueba con Id 999 |
| 15 | Publicación local (`dotnet publish`) generada y ejecutada | ✅ | Captura app en puerto 5050 (Production) |

---

## 7. Problemas conocidos y limitaciones

- **Puerto ocupado** (*"address already in use"*): ya hay otra copia de la app corriendo en ese puerto. Hay que cerrarla (Ctrl+C) o usar otro puerto con `--urls "http://localhost:5051"`.
- **Versión preliminar de .NET:** el proyecto usa .NET 11 RC; al compilar aparece el aviso `NETSDK1057`. Es solo informativo.
- **Error de certificado en `sqlcmd`** (*"La cadena de certificación fue emitida por una entidad en la que no se confía"*): agregar `-C` al comando `sqlcmd`. La app no tiene este problema porque su cadena incluye `TrustServerCertificate=True`.
- **Los Id no se reinician** al borrar registros desde la app (comportamiento normal de `IDENTITY` en SQL Server). Para volver a los Id 1–10, re-ejecutar `02_DatosSemilla.sql`.
- **Borrado en cascada:** si se borra una Zona o Problematica directamente en SQL Server, se borran también sus registros (valor por defecto de EF). La app no tiene pantallas para borrar zonas ni problemáticas.
- **Mensaje en inglés:** si se deja Intensidad o Frecuencia **completamente vacía**, aparece el mensaje automático de ASP.NET (*"The Intensidad field is required."*), porque esos campos solo tienen `[Range]`.
- **Alcance:** solo el módulo RegistrosOCN tiene CRUD. Zonas y Problematicas se cargan con el script semilla.
