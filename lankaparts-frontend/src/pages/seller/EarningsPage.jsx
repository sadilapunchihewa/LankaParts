import { Banknote, CreditCard, TrendingUp, Wallet } from 'lucide-react'
import { earningsHistory } from '../../data/seller'
import { formatLKR } from '../../utils/currency'

export default function EarningsPage() {
  const current = earningsHistory[0]
  const cards = [
    { label: 'Gross Sales', value: current.grossSales, icon: TrendingUp },
    { label: 'Platform Commission', value: current.commission, icon: CreditCard },
    { label: 'Net Earnings', value: current.netEarnings, icon: Wallet },
    { label: 'Pending Settlement', value: current.pendingSettlement, icon: Banknote },
    { label: 'Paid Settlement', value: current.paidSettlement, icon: Banknote },
  ]
  return <section className="seller-page"><div className="seller-page-heading"><span>Seller Earnings</span><h1>Earnings</h1><p>LankaParts platform commission is approximately 3% of gross sales.</p></div><div className="seller-card-grid five">{cards.map(({ label, value, icon: Icon }) => <article key={label}><Icon /><span>{label}</span><strong>{formatLKR(value)}</strong></article>)}</div><section className="seller-panel-block earnings-example"><h2>Commission example</h2><dl><div><dt>Gross Sales</dt><dd>{formatLKR(500000)}</dd></div><div><dt>Commission (3%)</dt><dd>{formatLKR(15000)}</dd></div><div><dt>Net Earnings</dt><dd>{formatLKR(485000)}</dd></div></dl></section><div className="seller-table-wrap"><div className="earnings-head"><span>Month</span><span>Gross Sales</span><span>Commission</span><span>Net Earnings</span><span>Pending Settlement</span><span>Paid Settlement</span></div>{earningsHistory.map((row) => <div className="earnings-row" key={row.month}><strong>{row.month}</strong><span>{formatLKR(row.grossSales)}</span><span>{formatLKR(row.commission)}</span><strong>{formatLKR(row.netEarnings)}</strong><span>{formatLKR(row.pendingSettlement)}</span><span>{formatLKR(row.paidSettlement)}</span></div>)}</div></section>
}
