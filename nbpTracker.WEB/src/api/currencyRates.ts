import type { ExchangeTable } from "../types/exchangeTable";
export const API_URL: string = "https://localhost:7020/api/CurrencyRates";

export async function fetchCurrencyRates(): Promise<ExchangeTable> {
  const res = await fetch(API_URL);
  if (!res.ok) throw new Error("API error");
  return await res.json();
}
