FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

#if (IsAngular)
# The publish step runs `ng build`, which requires Node.js (not included in the .NET SDK image).
RUN apt-get update \
    && apt-get install -y --no-install-recommends ca-certificates curl gnupg \
    && mkdir -p /etc/apt/keyrings \
    && curl -fsSL https://deb.nodesource.com/gpgkey/nodesource-repo.gpg.key | gpg --dearmor -o /etc/apt/keyrings/nodesource.gpg \
    && echo "deb [signed-by=/etc/apt/keyrings/nodesource.gpg] https://deb.nodesource.com/node_22.x nodistro main" > /etc/apt/sources.list.d/node.list \
    && apt-get update && apt-get install -y --no-install-recommends nodejs \
    && rm -rf /var/lib/apt/lists/*
#endif

# Copy solution metadata + project files only, so NuGet restore is a cached layer
COPY Directory.Packages.props .
COPY NetArch.Template.sln .
COPY NetArch.Template.WebAPI/NetArch.Template.WebAPI.csproj NetArch.Template.WebAPI/
COPY NetArch.Template.Infrastructure/NetArch.Template.Infrastructure.csproj NetArch.Template.Infrastructure/
COPY NetArch.Template.Tests/NetArch.Template.Tests.csproj NetArch.Template.Tests/
#if (IsClean)
COPY NetArch.Template.Domain/NetArch.Template.Domain.csproj NetArch.Template.Domain/
COPY NetArch.Template.Application/NetArch.Template.Application.csproj NetArch.Template.Application/
#endif
#if (IsNTier)
COPY NetArch.Template.BusinessLogic/NetArch.Template.BusinessLogic.csproj NetArch.Template.BusinessLogic/
#endif
RUN dotnet restore NetArch.Template.sln

COPY . .
RUN dotnet publish NetArch.Template.WebAPI/NetArch.Template.WebAPI.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "NetArch.Template.WebAPI.dll"]
