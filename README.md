# .NET 6 Web API Exemplar 

work in progress....

## TO DO
+ Fix/Implement JWT  auth and authz
+ Dockerfile and docker compose
+ Code coverages
+ Sample Unit Test
+ Mongo access


## Local Setup

### Consul 
```shell
docker run -d --name=dev-consul -e CONSUL_BIND_INTERFACE=eth0 -p 8500:8500 -v $PWD/devconsul:/consul/data consul agent -server -ui -bind 0.0.0.0 -client 0.0.0.0 -bootstrap -bootstrap-expect 1
```


Browse to http://localhost:8500/ on the **Key/Value** section create a folder named *Exemplar* , inside the folder create a key named *Development* with the following JSON value:

```json
{
  "message": "Hola mundo!",
  "autor":
  {
    "lastname":"Longhi"
  },
  "languages":["Spanish","English"]
}
```


### Seq
```shell
docker run --name seq -d --restart unless-stopped -e ACCEPT_EULA=Y -p 5341:80 datalust/seq:latest
```
http://localhost:5341/

## References
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-6.0
https://dev.to/engincanv/usage-of-consul-in-net-core-configuration-management-39h5
https://github.com/llatinov/aws.examples.csharp/tree/c3dda376d74c80da52e6026f6dce644c6564b67b/SqsReader/src/SqsReader/HealthChecks

## Guidelines
https://dev.to/i5han3/git-commit-message-convention-that-you-can-follow-1709
https://www.conventionalcommits.org/en/v1.0.0/

---
> “Exemplar.” Merriam-Webster.com Dictionary, Merriam-Webster, https://www.merriam-webster.com/dictionary/exemplar. Accessed 12 Nov. 2021.