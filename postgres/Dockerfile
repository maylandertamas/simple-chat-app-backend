FROM postgres:13-alpine
# Fix time zone
ENV TZ Europe/Budapest
RUN apk add --no-cache tzdata
RUN cp /usr/share/zoneinfo/${TZ} /etc/localtime
RUN echo ${TZ} >  /etc/timezone
