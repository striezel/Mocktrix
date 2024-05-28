# Copyright (C) 2024  Dirk Stolle
#
# SPDX-License-Identifier: GPL-3.0-or-later

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . .
RUN dotnet restore ./Mocktrix/Mocktrix.sln
RUN dotnet build ./Mocktrix/Mocktrix.csproj -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish ./Mocktrix/Mocktrix.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
LABEL org.opencontainers.image.authors="Dirk Stolle <striezel-dev@web.de>"
LABEL org.opencontainers.image.source=https://gitlab.com/striezel/mocktrix
LABEL org.opencontainers.image.licenses=GPL-3.0-or-later
USER app
WORKDIR /app
COPY --from=builder /app/publish .
EXPOSE 8080
EXPOSE 8081
ENTRYPOINT ["dotnet", "Mocktrix.dll"]
