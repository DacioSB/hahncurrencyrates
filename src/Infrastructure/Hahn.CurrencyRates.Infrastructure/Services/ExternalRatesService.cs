using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Hahn.CurrencyRates.Domain.Services;
using Hahn.CurrencyRates.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace Hahn.CurrencyRates.Infrastructure.Services;

public class ExternalRatesService : IExternalRatesService
{
    private readonly HttpClient _httpClient;
    private readonly ExternalRatesApiOptions _options;

    public ExternalRatesService(
        HttpClient httpClient,
        IOptions<ExternalRatesApiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
        
        _httpClient.BaseAddress = new Uri(_options.BaseUrl);
    }

    public async Task<ExternalRatesResponse> GetLatestRatesAsync(
        string baseCurrency,
        IEnumerable<string> targetCurrencies,
        CancellationToken cancellationToken = default)
    {
        var currencies = string.Join(",", targetCurrencies);
        var url = $"/latest" +
                 $"?base={baseCurrency}" +
                 $"&currencies={currencies}" +
                 $"&resolution={_options.Resolution}" +
                 $"&amount=1" +
                 $"&places={_options.DecimalPlaces}" +
                 $"&format={_options.Format}" +
                 $"&api_key={_options.ApiKey}";

        var response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = JsonSerializer.Deserialize<ApiResponse>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (result == null || !result.Success)
        {
            throw new Exception("Failed to fetch exchange rates");
        }

        return new ExternalRatesResponse(
            result.Success,
            result.Base,
            DateTimeOffset.FromUnixTimeSeconds(result.Timestamp).DateTime,
            result.Rates);
    }

    private class ApiResponse
    {
        public bool Success { get; set; }
        public string Base { get; set; } = string.Empty;
        public long Timestamp { get; set; }
        public Dictionary<string, decimal> Rates { get; set; } = new();
    }
}
