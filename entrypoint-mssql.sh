#!/bin/bash
# Start SQL Server
/opt/mssql/bin/sqlservr &

# Wait for SQL Server to start
echo "Waiting for SQL Server to start..."
until /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -Q "SELECT 1" &> /dev/null
do
  echo "SQL Server is starting up..."
  sleep 1
done

# Run the SQL script
echo "SQL Server is up. Running the script..."
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -i /docker-entrypoint-initdb.d/addusers_dev.sql

# Keep the container running
tail -f /dev/null