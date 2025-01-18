IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'HahnCurrencyRatesHangfire')
BEGIN
    CREATE DATABASE HahnCurrencyRatesHangfire;
END

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'HahnCurrencyRates')
BEGIN
    CREATE DATABASE HahnCurrencyRates;
END
