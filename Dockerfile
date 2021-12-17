FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy sln and csproj and try to restore dependencies
COPY *.sln .
COPY FibraClickSocial/*.csproj ./FibraClickSocial/
RUN dotnet restore

# Copy all srcs and compile
COPY . .
WORKDIR /app/FibraClickSocial
RUN dotnet build

FROM build AS publish
WORKDIR /app/FibraClickSocial
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS runtime

WORKDIR /app

COPY --from=publish /app/FibraClickSocial/out ./
COPY openssl.cnf /etc/ssl/openssl.cnf

ENTRYPOINT ["dotnet", "FibraClickSocial.dll"]
