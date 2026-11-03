# personapi-dotnet

PersonAPI es una solución desarrollada en ASP.NET Core 10 que expone tanto una aplicación web MVC clásica como una Web API RESTful. El sistema permite gestionar un registro estructurado de `Personas`, relacionando a cada individuo con sus `Estudios`, `Profesiones` y `Teléfonos`. 

El proyecto sigue una arquitectura limpia basada en el patrón Repositorio y utiliza Entity Framework Core para el acceso a datos.

## Requisitos Previos

Asegúrate de tener instaladas las siguientes herramientas en tu entorno de desarrollo antes de comenzar:

* [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
* Un motor de base de datos SQL Server (LocalDB, SQL Server Express o Dockerized).
* Herramientas de línea de comandos de Entity Framework Core (opcional, pero recomendado): `dotnet tool install --global dotnet-ef`
* [Docker Desktop](https://www.docker.com/products/docker-desktop/) (opcional, para despliegue en contenedores).

## Configuración del Ambiente

1. **Clonar el repositorio:**
   ```bash
   git clone <URL_DEL_REPOSITORIO>
   cd personapi-dotnet
   ```

2. **Restaurar las dependencias:**
   Esto descargará todos los paquetes de NuGet requeridos (EF Core, Swagger, etc.).
   ```bash
   dotnet restore
   ```

3. **Configuración de la Base de Datos:**
   Abre el archivo `appsettings.Development.json` (o `appsettings.json`) y configura tu cadena de conexión (Connection String) apuntando a tu instancia de SQL Server.
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=TU_SERVIDOR;Database=PersonaDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
   }
   ```

4. **Aplicar Migraciones (Entity Framework Core):**
   Si el proyecto utiliza Code-First, crea la estructura de la base de datos ejecutando:
   ```bash
   dotnet ef database update
   ```

## Compilación y Ejecución (Desarrollo)

Para compilar el código fuente y verificar que no existan errores de sintaxis:

```bash
dotnet build
```

Para levantar la aplicación en tu entorno local, ejecuta:

```bash
dotnet run
```

Por defecto, la aplicación se ejecutará en los puertos configurados en `Properties/launchSettings.json` (usualmente `http://localhost:5000` o similar). 
* **Web MVC:** Accede a la URL base desde tu navegador.
* **API Documentation:** Accede a `/swagger` para ver la documentación interactiva de la Web API generada por Swashbuckle.

## Despliegue (Deployment)

Tienes dos opciones principales para desplegar esta aplicación en un entorno de producción:

### Opción A: Despliegue mediante Contenedores (Docker)

El proyecto incluye un `Dockerfile` listo para contenerizar la aplicación.

1. **Construir la imagen de Docker:**
   ```bash
   docker build -t personapi-dotnet-app .
   ```

2. **Ejecutar el contenedor:**
   ```bash
   docker run -d -p 8080:80 --name personapi personapi-dotnet-app
   ```
   *(Nota: Asegúrate de inyectar las variables de entorno necesarias, como la cadena de conexión de producción, al ejecutar el contenedor).*

### Opción B: Publicación Directa (Binarios)

Para desplegar en servicios como IIS (Windows Server), Nginx/Apache (Linux) o Azure App Service:

1. **Generar los archivos optimizados (Release):**
   ```bash
   dotnet publish -c Release -o ./publish_output
   ```

2. **Despliegue:**
   Copia el contenido de la carpeta `./publish_output` a tu servidor de destino. La aplicación se puede iniciar directamente ejecutando:
   ```bash
   dotnet personapi-dotnet.dll
   ```
