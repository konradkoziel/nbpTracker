import type { CurrencyRate } from "./currencyRate";

export interface ExchangeTable {
    id:            number;
    tableName:     string;
    no:            string;
    effectiveDate: Date;
    currencyRates: CurrencyRate[];
}