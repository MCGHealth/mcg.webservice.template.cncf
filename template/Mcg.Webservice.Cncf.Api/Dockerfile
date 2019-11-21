FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine3.10 AS build

RUN wget -O dotnet.tar.gz https://download.visualstudio.microsoft.com/download/pr/64d8b249-f3d0-4ad9-89f9-9adfc909d3b0/aad4f4a97b6e9dcc97263d986eec1627/aspnetcore-runtime-2.1.14-linux-musl-x64.tar.gz \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && rm dotnet.tar.gz

WORKDIR /app

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

COPY Mcg.Webservice.Cncf.Api.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -r linux-musl-x64 -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine3.10 AS publish

ENV APP_ASPNETCORE_ENVIRONMENT=Production
ENV APP_JAEGER_AGENT_HOST=jaeger
ENV APP_JAEGER_AGENT_PORT=6831
ENV APP_JAEGER_SERVICE_NAME=example-1
ENV APP_JAEGER_SAMPLER_TYPE=const
ENV APP_LOG_LEVEL=information
ENV APP_ELASTIC_SEARCH_URI=http://elasticsearch:9200
ENV APP_CORS_ALLOWED_URLS=*

WORKDIR /app
EXPOSE 80 443 6831/udp 5672 5673
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "Mcg.Webservice.Cncf.Api.dll"] 