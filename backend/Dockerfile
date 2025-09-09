FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["backend.csproj", "./"]
RUN dotnet restore "./backend.csproj"

COPY . .
RUN dotnet publish "backend.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

ENV DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_USE_POLLING_FILE_WATCHER=false \
    NUGET_XMLDOC_MODE=skip

COPY --from=build /app/publish/ ./

RUN if ! id -u app >/dev/null 2>&1; then \
    useradd --system --user-group app; \
fi \
    && chown -R app:app /app
USER app

EXPOSE 80
ENTRYPOINT ["dotnet", "backend.dll"]