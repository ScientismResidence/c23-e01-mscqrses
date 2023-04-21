# Prerequisites guide

1. Install Docker and Docker Compose
2. Create the new network in Docker with name `C23-E01-MSCQRSES`

``
docker network create --attachable -d bridge C23-E01-MSCQRSES
``
3. Startup the services.

``
docker-compose up -d
``

4. Have a pleasure