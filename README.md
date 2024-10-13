# ContainerizeSQL-API


# basic dotnet cli commands to initial the project

    # dotnet new sln -n "myminapi"
    # dotnet new "webapi" -lang "C#" -n "MyDemoApi" -o "MyDemoApi"
    # dotnet sln "d:\Blog\RevisedAPI\myminapi.sln" add "d:\Blog\RevisedAPI\MyDemoApi\MyDemoApi.csproj"
    # dotnet add d:/Blog/RevisedAPI/MyDemoApi/MyDemoApi.csproj package Microsoft.EntityFrameworkCore.SqlServer -v 8.0.8 
    # dotnet add d:/Blog/RevisedAPI/MyDemoApi/MyDemoApi.csproj package Microsoft.EntityFrameworkCore.Tools -v 8.0.8 
    # dotnet add d:/Blog/RevisedAPI/MyDemoApi/MyDemoApi.csproj package Swashbuckle.AspNetCore -v 6.8.1 
    # dotnet add d:/Blog/RevisedAPI/MyDemoApi/MyDemoApi.csproj package Microsoft.Extensions.Caching.StackExchangeRedis -v 8.0.0

# docker commands
    # docker build -t dockeredapi:latest -f Dockerfile .
    # docker run -i --name myfirstpoc  52cabec57cd2
    # docker run --rm -d -e ASPNETCORE_ENVIRONMENT=Development --name sampleapi -p 8080:8080 -p 8081:8081 1695890ffec8     
 
    docker run -d -e ASPNETCORE_ENVIRONMENT=Development -p 7077:7077 ConStr=Data Source=localhost,1433;Initial Catalog=ecomDB;User Id=sa; Password=Pass@word1!;TrustServerCertificate=True; --name myfirstpoc a3b3224b8d36


docker image ls -f "reference=doc*api"
docker rmi $(docker image ls -f "reference=doc*api" -q)



 docker images --format "{{.ID}}: {{.Repository}}"


 docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Pass@word1!" -e "MSSQL_PID=Express" --name "sqlserver1" -v volmydemoapi:/var/opt/mssql -p 1466:1433 -d mcr.microsoft.com/mssql/server:2019-latest



                API         DB      Result
                Local       Local       Y
                Local       Docker      Y
                DOcker      Local      Not possible
                Docker      Docker

# docker run --rm -d -e ASPNETCORE_ENVIRONMENT=Development -p 7077:7077 dockeredapi:latest --network nwkmydemoapi
 

# docker run --rm -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Pass@word1!" -e "MSSQL_PID=Express" --name "sqlserver1" -v volmydemoapi:/var/opt/mssql -p 1466:1433 -d mcr.microsoft.com/mssql/server:2019-latest --network nwkmydemoapi



#docker sql and local api use =>   "ConStr": "Data Source=localhost,1466;Initial Catalog=ecomDB;User Id=sa; Password=Pass@word1!;TrustServerCertificate=True;",


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release

#pwd
WORKDIR /app   

#ls > pwd/MyDemoApi         
#COPY ["./MyDemoApi.csproj", "MyDemoApi/"]

COPY [MyDemoApi.csproj, MyDemoApi/] 

#RUN dotnet restore "./MyDemoApi.csproj"
#RUN dotnet restore ./MyDemoApi/MyDemoApi.csproj


#COPY . ./
#WORKDIR /app/MyDemoApi
#RUN dotnet build ./MyDemoApi.csproj -c $configuration -o /app/build

#summary ------> pwd is app and ls at this level show all the cotents are there



# FROM build AS publish
# ARG configuration=Release
# RUN dotnet publish "MyDemoApi.csproj" -c $configuration -o /app/publish /p:UseAppHost=false


# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# WORKDIR /app
# EXPOSE 7077

# ENV ASPNETCORE_URLS=http://+:7077

# USER app

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .

# EXPOSE 7077
# ENTRYPOINT ["dotnet", "MyDemoApi.dll"]



 No store type was specified for the decimal property 'Price' on entity type 'Product'. This will cause values to be silently truncated if they do not fit in the default precision and scale. Explicitly specify the SQL server column type that can accommodate all the values in 'OnModelCreating' using 'HasColumnType', specify precision and scale using 'HasPrecision', or configure a value converter using 'HasConversion'.



 ##### back up of docker compose file


 
#version: '3.8'
name: highcontainercluster
services:
#===========================================
  minimalapi:
    container_name: ctnmydemoapi
    image: dockeredapi:latest
    build:
      context: ./MyDemoApi
      dockerfile: MyDemoApi/Dockerfile
    environment:
      - ASPNETCORE_ENVIRONMENT=Development 
      #- ConStr=Data Source=localhost,1433;Initial Catalog=ecomDB;User Id=sa; Password=Pass@word1!;TrustServerCertificate=True;
    ports:
      - 7077:7077
    depends_on:
      - dbmydatabase
    networks:
      - nwkmydemoapi
    restart: on-failure
#==========================================='
  dbmydatabase:
    image: mcr.microsoft.com/mssql/server:2019-latest
    container_name: ctndbmydemoapi
    environment:
      - ACCEPT_EULA=Y
      - MSSQL_SA_PASSWORD=Pass@word1!
      - MSSQL_PID=Express
    ports:
      - "1466:1433"
    volumes:
      - volmydemoapi:/var/opt/mssql
    networks:
      - nwkmydemoapi       
#===============================================
volumes:
  volmydemoapi:
    name: volmydemoapi 
#==============================================
networks:
  nwkmydemoapi:
    #driver: bridge