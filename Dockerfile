# Imagen base para runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Imagen para build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar solo el archivo del proyecto para restaurar dependencias
COPY Uttt.Micro.Libro.csproj ./
RUN dotnet restore ./Uttt.Micro.Libro.csproj

# Copiar todo el código fuente
COPY . ./

# Construir el proyecto
RUN dotnet build ./Uttt.Micro.Libro.csproj -c Release -o /app/build

# Publicar el proyecto
FROM build AS publish
RUN dotnet publish ./Uttt.Micro.Libro.csproj -c Release -o /app/publish /p:UseAppHost=false

# Imagen final para producción
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Uttt.Micro.Libro.dll"]
