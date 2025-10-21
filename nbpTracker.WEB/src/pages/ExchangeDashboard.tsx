import { useExchangeTable } from "../hooks/useExchangeTable";

export function ExchangeDashboard() {
  const { table, loading, error } = useExchangeTable();

  if (loading)
    return <p className="text-slate-500 text-center mt-4">Loading...</p>;
  if (error)
    return <p className="text-red-500 text-center mt-4">Error: {error}</p>;
  if (!table)
    return <p className="text-slate-400 text-center mt-4">No data</p>;

  return (
    <div className="max-w-[500px] mt-8 p-6 bg-gradient-to-br from-slate-50 to-white shadow-lg rounded-xl border border-slate-200">
      <h2 className="text-[24px] font-bold text-indigo-600 mb-1 cursor-default">
        {table.tableName} <span className="text-slate-500">({table.no})</span>
      </h2>
      <p className="text-sm text-slate-500 mb-4 cursor-default">
        {new Date(table.effectiveDate).toLocaleDateString()}
      </p>
      <ul className="flex flex-col max-h-[500px] gap-2 overflow-y-auto pr-2 scroll-smooth">
        {table.currencyRates.map((rate) => (
          <li
            key={rate.id}
            className="flex justify-between items-center bg-slate-50 p-3 rounded-md hover:bg-slate-100 transition cursor-default"
          >
            <span className="font-medium text-slate-700">
              {`${rate.currencyCode} (${rate.currencyName})`}
            </span>
            <span className="text-indigo-500 font-semibold">{rate.mid}</span>
          </li>
        ))}
      </ul>
    </div>
  );
}