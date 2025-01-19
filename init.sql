-- Create Command database for write operations and Hangfire job storage
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'HahnCurrencyRates')
BEGIN
    CREATE DATABASE HahnCurrencyRates;
END

-- Create Query database for read operations
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'HahnCurrencyRatesQuery')
BEGIN
    CREATE DATABASE HahnCurrencyRatesQuery;
END
