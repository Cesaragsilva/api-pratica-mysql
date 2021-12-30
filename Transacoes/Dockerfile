FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Transacoes/Transacao.API.csproj", "Transacoes/"]
RUN dotnet restore "Transacoes/Transacao.API.csproj"
COPY . .
WORKDIR "/src/Transacoes"
RUN dotnet build "Transacao.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Transacao.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Transacao.API.dll"]