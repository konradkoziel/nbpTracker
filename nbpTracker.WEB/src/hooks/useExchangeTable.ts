import { useEffect, useState } from "react";
import type { ExchangeTable } from "../types/exchangeTable";
import { fetchCurrencyRates } from "../api/currencyRates";

export function useExchangeTable() {
const [table, setTable] = useState<ExchangeTable>();  
const [loading, setLoading] = useState(true);
const [error, setError] = useState("");

  useEffect(() => {
    fetchCurrencyRates()
      .then(setTable)
      .catch(err => setError(err.message))
      .finally(() => setLoading(false));
  }, []);

  return { table, loading, error };
}
