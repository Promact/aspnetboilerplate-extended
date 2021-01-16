FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim

WORKDIR /app
COPY src/..
RUN cd BoilerPlateDemo_App.Web.Host && dotnet publish -c Release -o out
 
FROM node as angular-build
WORKDIR /app
COPY src/BoilerPlateDemo_App.Web.Host/package*.json ./
RUN npm install
COPY src/BoilerPlateDemo_App.Web.Host/. .
RUN npm run ng -- build --prod

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim as runtime
WORKDIR /app
COPY --from=dotnet-build /app/BoilerPlateDemo_App.Web.Host/out ./
COPY --from=angular-build /app/dist ./wwwroot
ENTRYPOINT ["dotnet", "BoilerPlateDemo_App.Web.Host.dll"]
