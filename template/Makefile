IMG=""
IMG=api_cncf
SLN=Mcg.Webservice.Cncf
API=$(SLN).Api/$(SLN).Api.csproj
VER=v0.0.1

default: build

clean:
	dotnet clean

restore: clean
	dotnet restore --no-cache --force

build:
	dotnet build $(SLN).sln -c Debug --force --nologo

run: build
	dotnet run -p $(API)

test: build
	dotnet test /p:CollectCoverage=true

publish:
	dotnet publish -r linux-musl-x64 -c Release -o ./$(SLN).api/publish $(API)

docker: publish
	docker-compose up