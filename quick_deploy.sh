##!/bin/bash

# Use this script to quickly deploy the app to Azure App Service without Terraform

set -e

dotnet clean
dotnet restore
dotnet publish -c Release

if [ "$OS" = "Linux" ] || [ $OS = "Darwin" ]; then # Linux or MacOS
    zip -r ./DatabaseProject/bin/Release/publish.zip ./DatabaseProject/bin/Release/net8.0/publish/* 
else # Windows
    powershell 'Compress-Archive -Path ./DatabaseProject/bin/Release/net8.0/publish/* -DestinationPath ./DatabaseProject/bin/Release/publish.zip -Force'
fi

az webapp deploy --resource-group airline-database-project --name airline-database-project-backend --src-path "./DatabaseProject/bin/Release/publish.zip"

rm ./DatabaseProject/bin/Release/publish.zip # Clean up the zip file