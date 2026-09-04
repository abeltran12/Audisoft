# Esta fase se usa cuando se ejecuta desde VS en modo rápido (valor predeterminado para la configuración de depuración)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# Esta fase se usa para compilar el proyecto de servicio
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Audisoft.Web.Host/Audisoft.Web.Host.csproj", "src/Audisoft.Web.Host/"]
COPY ["src/Audisoft.Web/Audisoft.Web.csproj", "src/Audisoft.Web/"]
RUN dotnet restore "src/Audisoft.Web.Host/Audisoft.Web.Host.csproj"
COPY . .
WORKDIR "/src/src/Audisoft.Web.Host"
RUN dotnet build "Audisoft.Web.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase se usa para publicar el proyecto (incluye los archivos estáticos publicados del cliente WASM)
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Audisoft.Web.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# El publish del Host (por ProjectReference) no resuelve el fingerprint del index.html del cliente.
# Publicamos el cliente aparte (eso sí lo resuelve bien) y reemplazamos el wwwroot con ese resultado.
RUN dotnet publish "/src/src/Audisoft.Web/Audisoft.Web.csproj" -c $BUILD_CONFIGURATION -o /app/client-publish /p:UseAppHost=false
RUN rm -rf /app/publish/wwwroot && cp -r /app/client-publish/wwwroot /app/publish/wwwroot

# Esta fase se usa en producción
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Audisoft.Web.Host.dll"]
