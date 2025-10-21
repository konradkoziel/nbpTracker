import type { ExchangeTable } from "../types/exchangeTable";
export const API_URL: string = import.meta.env.VITE_API_URL;

export async function fetchCurrencyRates(): Promise<ExchangeTable> {
  const res = await fetch(API_URL);
  if (!res.ok) throw new Error("API error");
  return await res.json();
}
