FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar los archivos de proyecto (.csproj) usando la estructura de carpetas real
COPY ["Presentation/Ecommerce.Presentacion.csproj", "Presentation/"]
COPY ["Dal/Ecommerce.Negocio.csproj", "Dal/"]
COPY ["Data/Ecommerce.Datos.csproj", "Data/"]

# Restaurar dependencias del proyecto de presentación
RUN dotnet restore "Presentation/Ecommerce.Presentacion.csproj"

# Copiar el resto del código fuente
COPY . .
WORKDIR "/src/Presentation"
RUN dotnet build "Ecommerce.Presentacion.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicar la aplicación
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Ecommerce.Presentacion.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Crear la imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.Presentacion.dll"]
