FROM mcr.microsoft.com/dotnet/sdk:5.0 as dotnet-build
WORKDIR /app
COPY src/. .
RUN cd BoilerPlateDemo_App.Web.Host && dotnet publish -c Release -o out
 
FROM node:12 as angular-build
WORKDIR /app
COPY src/BoilerPlateDemo_App.Web.Host/package*.json ./
RUN npm install
COPY src/BoilerPlateDemo_App.Web.Host/. .
RUN npm run ng -- build --prod

FROM mcr.microsoft.com/dotnet/aspnet:5.0 as runtime
WORKDIR /app
COPY --from=dotnet-build /app/src/BoilerPlateDemo_App.Web.Host/out ./
COPY --from=angular-build /app/wwwroot/dist ./wwwroot
ENTRYPOINT ["dotnet", "BoilerPlateDemo_App.Web.Host.dll"]
