## Simple example using RabbitMq

A console app that sends messages to RabbitMQ and a service that reads those messages.

## Docker comand to run RabbitMQ

docker run -d --hostname rabbit-local --name rabbitmq -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_USER=admin -e RABBITMQ_DEFAULT_PASS=admin rabbitmq:3-management