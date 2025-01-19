export interface CurrencyRate {
    baseCurrency: string;
    targetCurrency: string;
    rate: number;
    lastUpdated: string;
}

export interface CurrencyRatesFilter {
    baseCurrency?: string;
    targetCurrencies?: string[];
    sortBy?: string;
    sortDescending?: boolean;
}
