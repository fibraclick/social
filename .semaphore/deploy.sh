#!/bin/bash
set -e

cd /home/deploy/fibraclick-social
echo DOCKER_TAG=$DOCKER_TAG > .env
echo HOSTNAME=$HOSTNAME >> .env
docker-compose -f docker-compose.prod.yml pull
docker-compose -f docker-compose.prod.yml -p $CONTAINER_NAME up -d --force-recreate --remove-orphans
exit
