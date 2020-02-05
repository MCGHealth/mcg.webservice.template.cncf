FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine3.10 AS build

WORKDIR /app

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -r linux-musl-x64 -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine3.10 AS publish

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