# Fase base para producción
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Fase build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar solo el csproj desde la carpeta del proyecto
COPY ["Uttt.Micro.Libro/Uttt.Micro.Libro.csproj", "./"]

# Restaurar dependencias
RUN dotnet restore "./Uttt.Micro.Libro.csproj"

# Copiar todo el código desde la raíz
COPY . .

# Cambiar al directorio del proyecto dentro del contexto
WORKDIR "/src/Uttt.Micro.Libro"

# Construir el proyecto
RUN dotnet build "./Uttt.Micro.Libro.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicar el proyecto
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Uttt.Micro.Libro.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Imagen final de producción
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Uttt.Micro.Libro.dll"]
