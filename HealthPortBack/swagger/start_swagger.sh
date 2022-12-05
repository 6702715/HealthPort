#!/bin/bash

docker run --rm -p 7001:8080 -v $PWD:/files -e SWAGGER_JSON=/files/healthport_back_api.yml  --name healthport-swagger-ui swaggerapi/swagger-ui