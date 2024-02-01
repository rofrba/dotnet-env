FROM registry.access.redhat.com/ubi8/dotnet-60:6.0-32.20230511125009 as builder
WORKDIR /opt/app-root/src
COPY --chown=1001 . .
RUN dotnet publish -c Release

FROM registry.access.redhat.com/ubi8/dotnet-60:6.0-32.20230511125009
COPY --from=builder /opt/app-root/src/bin /opt/app-root/src/bin
# COPY bin /opt/app-root/src/bin
WORKDIR /opt/app-root/src/bin/Release/net6.0
COPY appsettings.json .
CMD ["dotnet", "MyApp.dll"]