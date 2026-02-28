# 🛠️ Guía de Configuración para Colaboradores

## Requisitos del Proyecto

Este proyecto usa **.NET 10 (Preview)** con las siguientes capas:

| Proyecto | Framework |
|---|---|
| `Ecommerce.Presentacion` | net10.0 |
| `Ecommerce.Negocio` | net10.0 |
| `Ecommerce.Datos` | net10.0 |

---

## ⚡ Opción 1: Docker Compose (Recomendada — Sin instalar nada)

Solo necesitas tener **Docker Desktop** instalado. No necesitas .NET SDK.

```bash
# 1. Clona el repositorio
git clone https://github.com/TU-USUARIO/Ecommerce-NA.git
cd Ecommerce-NA

# 2. Levanta la aplicación con un solo comando
docker compose up --build

# 3. Abre en tu navegador
# http://localhost:5270/Admin/Dashboard
```

Para detener:
```bash
docker compose down
```

---

## 🧊 Opción 2: Dev Container (Recomendada para desarrollo)

Si usas **VS Code**, esta es la mejor opción para desarrollar. Obtiene el SDK `.NET 10` exacto automáticamente dentro de un contenedor.

### Requisitos
- [VS Code](https://code.visualstudio.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- Extensión: [Dev Containers](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-containers)

### Pasos
1. Clona el repositorio y ábrelo en VS Code
2. VS Code detectará el archivo `.devcontainer/devcontainer.json` y mostrará un mensaje:
   > *"Reopen in Container"*
3. Haz clic en **"Reopen in Container"**
4. Espera a que se construya el contenedor (~2-3 minutos la primera vez)
5. Una vez dentro, ejecuta:
   ```bash
   cd Presentation
   dotnet run
   ```
6. Abre http://localhost:5270/Admin/Dashboard

---

## 💻 Opción 3: Instalar .NET 10 Preview localmente

Si prefieres trabajar sin Docker, instala el SDK de .NET 10 Preview:

### Windows
```powershell
# Opción A: Desde la página oficial
# https://dotnet.microsoft.com/en-us/download/dotnet/10.0

# Opción B: Con winget
winget install Microsoft.DotNet.SDK.Preview
```

### macOS
```bash
brew install --cask dotnet-sdk-preview
```

### Linux (Ubuntu/Debian)
```bash
# Agregar el feed de .NET
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 10.0 --quality preview
```

### Verificar instalación
```bash
dotnet --version
# Debe mostrar 10.0.x
```

### Ejecutar
```bash
cd Presentation
dotnet restore
dotnet run
# Abre: http://localhost:5270/Admin/Dashboard
```

---

## ❓ Solución de Problemas

### Error: "The current .NET SDK does not support targeting .NET 10.0"
**Causa:** Tienes .NET 8 o 9 instalado pero no .NET 10.  
**Solución:** Usa la Opción 1 (Docker) o instala .NET 10 Preview (Opción 3).

### Error: "NETSDK1045: The current .NET SDK does not support..."
**Causa:** El archivo `global.json` está pidiendo una versión específica del SDK.  
**Solución:** Asegúrate de instalar .NET 10 Preview. El `global.json` usa `rollForward: latestMajor` para ser flexible.

### Error: "Unable to find package Microsoft.EntityFrameworkCore..."
**Causa:** Algunos paquetes NuGet requieren fuentes preview.  
**Solución:** Ejecuta:
```bash
dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget.org
dotnet restore
```

### Error: "relation 'Categorias' does not exist"
**Causa:** Las tablas de la base de datos no se han creado.  
**Solución:**
```bash
cd Presentation
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project ../Data/Ecommerce.Datos.csproj
dotnet ef database update --project ../Data/Ecommerce.Datos.csproj
```

---

## 📁 Estructura del Proyecto

```
Ecommerce-NA/
├── .devcontainer/          ← Config de Dev Container
│   └── devcontainer.json
├── Data/                   ← Capa de Datos (Entidades, DbContext)
├── Dal/                    ← Capa de Negocio (Servicios, DTOs)
├── Presentation/           ← Capa de Presentación (MVC + API)
│   ├── Areas/Admin/        ← Panel de Administración
│   └── wwwroot/            ← Archivos estáticos (CSS, JS)
├── docker-compose.yml      ← Levantar con Docker
├── Dockerfile              ← Imagen de producción
├── global.json             ← Versión del SDK pinneada
└── CONTRIBUTING.md         ← Esta guía
```
