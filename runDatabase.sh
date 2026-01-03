docker rm databaseContainer
docker build -t database .
docker run -d --name databaseContainer -p 5432:5432 -it database