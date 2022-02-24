#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base   
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SME.Integracao.Serap.Worker/SME.Integracao.Serap.Worker.csproj", "src/SME.Integracao.Serap.Worker/"]
COPY ["SME.Integracao.Serap.IoC/SME.Integracao.Serap.IoC.csproj", "src/SME.Integracao.Serap.IoC/"]
COPY ["SME.Integracao.Serap.Dados/SME.Integracao.Serap.Dados.csproj", "src/SME.Integracao.Serap.Dados/"]
COPY ["SME.Integracao.Serap.Infra/SME.Integracao.Serap.Infra.csproj", "src/SME.Integracao.Serap.Infra/"]
COPY ["SME.Integracao.Serap.Dominio/SME.Integracao.Serap.Dominio.csproj", "src/SME.Integracao.Serap.Dominio/"]
COPY ["SME.Integracao.Serap.Aplicacao/SME.Integracao.Serap.Aplicacao.csproj", "src/SME.Integracao.Serap.Aplicacao/"]


RUN dotnet restore "src/SME.Integracao.Serap.Worker/SME.Integracao.Serap.Worker.csproj"
COPY . .
WORKDIR "/src/SME.Integracao.Serap.Worker"
RUN dotnet build "SME.Integracao.Serap.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SME.Integracao.Serap.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV TZ=America/Sao_Paulo

RUN apt-get update \
    && apt-get install -yq tzdata locales -y \
    && dpkg-reconfigure --frontend noninteractive tzdata \ 
	&& locale-gen en_US.UTF-8 \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/* 

ENTRYPOINT ["dotnet", "SME.Integracao.Serap.Prova.Worker.dll"]
