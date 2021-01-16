FROM nginx

COPY . /usr/share/nginx/html

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim

WORKDIR /app
COPY . .

ENTRYPOINT ["dotnet", "CETAutomation.Web.Host.dll"]
