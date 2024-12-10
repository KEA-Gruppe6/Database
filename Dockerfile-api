# Use the official .NET 8 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project file and restore dependencies
COPY DatabaseProject/*.csproj ./
RUN dotnet restore

# Copy the rest of the application code
COPY . ./

# Build the application
RUN dotnet publish DatabaseProject/Database-project.csproj -c Release -o out

# Use the official .NET 8 runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Copy the SQL scripts
RUN mkdir -p SQL_Query
COPY DatabaseProject/SQL_Query/populatedb.sql /app/SQL_Query
COPY DatabaseProject/SQL_Query/passportlengthtrigger.sql /app/SQL_Query

ENV ASPNETCORE_URLS=http://*:8080

# Expose the port the application runs on
EXPOSE 8080

# Set the entry point for the application
ENTRYPOINT ["dotnet", "Database-project.dll"]