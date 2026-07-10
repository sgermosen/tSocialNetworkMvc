FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Tetas.Domain/Tetas.Domain.csproj Tetas.Domain/
COPY Tetas.Common/Tetas.Common.csproj Tetas.Common/
COPY Tetas.Infraestructure/Tetas.Infraestructure.csproj Tetas.Infraestructure/
COPY Tetas.Repositories/Tetas.Repositories.csproj Tetas.Repositories/
COPY Tetas.Web/Tetas.Web.csproj Tetas.Web/
RUN dotnet restore Tetas.Web/Tetas.Web.csproj

COPY Tetas.Domain/ Tetas.Domain/
COPY Tetas.Common/ Tetas.Common/
COPY Tetas.Infraestructure/ Tetas.Infraestructure/
COPY Tetas.Repositories/ Tetas.Repositories/
COPY Tetas.Web/ Tetas.Web/
RUN dotnet publish Tetas.Web/Tetas.Web.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .
RUN mkdir -p /app/data
ENV ASPNETCORE_URLS=http://+:8080
ENV ConnectionStrings__SqliteCnn="Data Source=/app/data/tetas.db"
EXPOSE 8080
ENTRYPOINT ["dotnet", "Tetas.Web.dll"]
