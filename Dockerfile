FROM microsoft/dotnet:2.2-sdk AS build
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

FROM microsoft/dotnet:2.2-runtime-alpine AS runtime

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT false
RUN apk add --no-cache icu-libs

WORKDIR /app
COPY --from=publish /app/FibraClickSocial/out ./
ENTRYPOINT ["dotnet", "FibraClickSocial.dll"]
