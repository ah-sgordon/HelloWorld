FROM microsoft/aspnet:1.0.0-rc1-update1-coreclr
 
ADD . /service
 
WORKDIR /service
 
RUN ["dnu", "restore"]
 
ENTRYPOINT ["dnx", "web", "--server.urls", "http://0.0.0.0:5000"]