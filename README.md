# independentreservesample
Sample application using IndependentReserve API to get Bitcoin trades data

### How to run
From repository directory
```shell
docker build --no-cache -f .\Web\Dockerfile -t independentreservesample .
```

```shell
docker run -p 5016:8080 independentreservesample
```

Then open http://localhost:5016.

## How to run unit tests
```shell
docker build --no-cache -f .\Tests\Dockerfile -t independentreservesample_test .
```

```shell
docker run independentreservesample_test
```

Note: separate Dockerfile for unit tests project allows to parallelize building and testing application in CI/CD.

## What should be done next
- more unit tests: 
	- on Index.cshtml.cs
	- on IndependentReserveClient (mock external APIs somehow)
- extract business login from Index.cshtml.cs into separate service and cover it with unit tests
- UI: new API returning JSON with average trades info + polling this API from UI for faster refresh
- or send updated trades info to UI using SignalR
- integration tests using testcontainers