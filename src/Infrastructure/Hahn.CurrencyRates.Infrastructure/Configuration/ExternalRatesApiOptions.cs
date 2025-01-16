namespace Hahn.CurrencyRates.Infrastructure.Configuration;

public class ExternalRatesApiOptions
{
    public const string SectionName = "ExternalRatesApi";
    
    public string BaseUrl { get; set; } = "https://api.fxratesapi.com";
    public string ApiKey { get; set; } = string.Empty;
    public string Resolution { get; set; } = "1m";
    public int DecimalPlaces { get; set; } = 6;
    public string Format { get; set; } = "json";
}
