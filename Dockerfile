FROM postgres:17-alpine
ENV POSTGRES_USER=adminUser
ENV POSTGRES_PASSWORD=password
ENV POSTGRES_DB=testDatabase
EXPOSE 5432
COPY DatabaseSchema.sql /docker-entrypoint-initdb.d/
VOLUME /var/lib/postgresql/data