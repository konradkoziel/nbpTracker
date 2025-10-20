import { useExchangeTable } from "../hooks/useExchangeTable";

export function ExchangeDashboard() {
  const { table, loading, error } = useExchangeTable();

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error}</p>;
  if (!table) return <p>No data</p>;

  return (
    <div>
      <h2>{table.tableName} ({table.no})</h2>
        <p>{new Date(table.effectiveDate).toLocaleDateString()}</p>
      <ul>
        {table.currencyRates.map(rate => (
          <li key={rate.id}>
            {rate.currencyCode} ({rate.currencyName}): {rate.mid}
          </li>
        ))}
      </ul>
    </div>
  );
}
