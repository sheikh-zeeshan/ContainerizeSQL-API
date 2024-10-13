FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG configuration=Release


WORKDIR /src   

COPY . /src/MyDemoApi

RUN dotnet restore ./MyDemoApi/MyDemoApi.csproj

#COPY . ./MyDemoApi
#WORKDIR /src/MyDemoApi

RUN dotnet build ./MyDemoApi/MyDemoApi.csproj -c $configuration -o /src/build --no-restore

#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish ./MyDemoApi/MyDemoApi.csproj -c $BUILD_CONFIGURATION -o /src/publish /p:UseAppHost=false

RUN dotnet publish ./MyDemoApi/MyDemoApi.csproj -c Release -o out --no-restore


# FROM build AS final
# WORKDIR /app
# COPY --from=publish /src/publish .
#EXPOSE 7077
#ENTRYPOINT ["dotnet", "MyBasicAPI.dll"]


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /src/out .
EXPOSE 8080 7077
ENTRYPOINT ["dotnet", "MyDemoApi.dll"]

