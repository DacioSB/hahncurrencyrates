export interface CurrencyRate {
    baseCurrency: string;
    targetCurrency: string;
    rate: number;
    lastUpdated: string;
}

export interface CurrencyRatesFilter {
    baseCurrency?: string;
    targetCurrency?: string;
    sortBy?: string;
    sortDescending?: boolean;
}
