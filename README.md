docker build -t dotnet-env .

docker run -it -e MyApp_MY_VARIABLE=test-env dotnet-env

You can see the next log output:

```
Configuring logging...
Value from appsettings.json: ValueFromConfig
Secret from environment variable: test-env
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: /opt/app-root/src/bin/Release/net6.0
```
