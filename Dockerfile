FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR authservice

EXPOSE 7017
EXPOSE 443
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:7017
ENV DOTNET_URLS=http://+:7017

WORKDIR /src
COPY ["Template2/Presentation.csproj", "Template2/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Domain/Domain.csproj", "Domain/"]

RUN dotnet restore "./Template2/Presentation.csproj"

COPY . .
WORKDIR "/src/Template2"
RUN dotnet build "Presentation.csproj" -c Release -o /authservice/build

FROM build AS publish
RUN dotnet publish "Presentation.csproj" -c Release -o /authservice/publish

FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /authservice
COPY --from=publish /authservice/publish .
COPY aspnetapp.pfx /usr/local/share/ca-certificates
COPY aspnetapp.pfx /https/
RUN chmod 644 /usr/local/share/ca-certificates/aspnetapp.pfx && update-ca-certificates
ENTRYPOINT ["dotnet", "Presentation.dll"]