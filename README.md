# .NET 6 Web API Exemplar

work in progress....

## TO DO
+ Fix token auth and authz
+ Redirect to swagger
+ Fixed default port 5000
+ Dockerfile and docker compose
+ Code coverages
+ Sample Unit Test
+ Mongo access


## Local Setup

### Consul 
```shell
docker run -d -p 8500:8500 -p 8600:8600/udp --name=my-consul consul agent -server -ui -node=server-1 -bootstrap-expect=1 -client=0.0.0.0
```
http://localhost:8500/


### Seq
```shell
docker run --name seq -d --restart unless-stopped -e ACCEPT_EULA=Y -p 5341:80 datalust/seq:latest
```
http://localhost:5341/

### References
https://dev.to/engincanv/usage-of-consul-in-net-core-configuration-management-39h5
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-6.0
https://github.com/llatinov/aws.examples.csharp/tree/c3dda376d74c80da52e6026f6dce644c6564b67b/SqsReader/src/SqsReader/HealthChecks
### Guidelines
https://dev.to/i5han3/git-commit-message-convention-that-you-can-follow-1709
https://www.conventionalcommits.org/en/v1.0.0/

---

“Exemplar.” Merriam-Webster.com Dictionary, Merriam-Webster, https://www.merriam-webster.com/dictionary/exemplar. Accessed 12 Nov. 2021.