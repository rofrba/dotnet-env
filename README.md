docker build -t dotnet-env .

docker run -it -e MYAPP_MY_VARIABLE=test-env dotnet-env

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

# OPENSHIFT

# Crear el deployment desde los fuentes

oc login

oc project XXX

oc new-app https://github.com/rofrba/dotnet-env.git

oc logs deploy/dotnet-env

# Luego se setear, para asegurar los resultados usar o eliminar pod a mano

oc scale deploy/dotnet-env --replicas=0

oc scale deploy/dotnet-env --replicas=1

# Setear variables de entorno en forma directa

oc set env deploy/dotnet-env MYAPP_MY_VARIABLE=Homero

# tambien se puede setear desde un configmap o secret como variables de entorno

oc create configmap myenvvar --from-literal=MYAPP_MY_VARIABLE=Bart

oc set env deploy/dotnet-env --from=configmap/myenvvar


# Verificar con:

oc set env deploy/dotnet-env --list

# Otra opción es trabajar con archivos de configuracion montados (externalizados)

# Crear el secreto o configmap

oc create secret generic myappsettings --from-file=app.json=appsettings.json

# esto dispara un redespliegue

oc set volume deploy/dotnet-env --add --name=mysecreto --type=secret --secret-name=myappsettings --mount-path=/opt/app-root/src/bin/Release/net6.0/appsettings.json --sub-path=appsettings.json
# falta los items, que no se pueden setear desde linea de comando


```
oc patch deployment dotnet-env --type strategic -p  '
{
  "spec": {
    "template": {
      "spec": {
        "volumes": [
          {
            "name": "mysecreto",
            "secret": {
              "secretName": "myappsettings",
              "items": [
                {
                  "key": "app.json",
                  "path": "appsettings.json",
                  "mode": 420
                }
              ]
            }
          }
        ]
      }
    }
  }
}'
```


# o Ajustar a mano

```
volumes:
- name: mysecreto
    secret:
    secretName: myappsettings
    items:
        - key: app.json
        path: appsettings.json
    defaultMode: 420
```
