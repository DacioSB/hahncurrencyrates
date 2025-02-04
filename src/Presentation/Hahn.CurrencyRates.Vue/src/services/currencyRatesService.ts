import axios from 'axios';
import type { CurrencyRate, CurrencyRatesFilter } from '../types/CurrencyRate';

const API_URL = 'http://localhost:5294/api';

export const currencyRatesService = {
    async getCurrencyRates(filter?: CurrencyRatesFilter): Promise<CurrencyRate[]> {
        const params = new URLSearchParams();
        
        if (filter?.baseCurrency) {
            params.append('baseCurrency', filter.baseCurrency);
        }
        if (filter?.targetCurrencies?.length) {
            filter.targetCurrencies.forEach(currency => {
                params.append('targetCurrencies', currency);
            });
        }
        if (filter?.sortBy) {
            params.append('sortBy', filter.sortBy);
        }
        if (filter?.sortDescending !== undefined) {
            params.append('sortDescending', filter.sortDescending.toString());
        }

        const response = await axios.get<CurrencyRate[]>(`${API_URL}/currencyrates`, { params });
        return response.data;
    }
};
