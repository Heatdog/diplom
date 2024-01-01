FROM mcr.microsoft.com/dotnet/aspnet AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk AS build
WORKDIR /src
COPY ["ElectronicDocumentManagement.csproj", "/src"]
RUN dotnet restore "ElectronicDocumentManagement.csproj"
COPY . .
RUN dotnet build "ElectronicDocumentManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ElectronicDocumentManagement.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "ElectronicDocumentManagement.dll" ]