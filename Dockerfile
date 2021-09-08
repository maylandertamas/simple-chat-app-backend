FROM mcr.microsoft.com/dotnet/sdk:5.0 as build

# Use native linux file polling for better performance
ENV DOTNET_USE_POLLING_FILE_WATCHER 1

# Fix time zone
RUN cp /usr/share/zoneinfo/Europe/Budapest /etc/localtime
RUN echo Europe/Budapest >  /etc/timezone

RUN dotnet tool install --global dotnet-ef

WORKDIR /app
COPY . .
RUN dotnet restore

RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh