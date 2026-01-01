docker rm databaseContainer
docker build -t database .
docker run -d --name databaseContainer -it database