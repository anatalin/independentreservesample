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
