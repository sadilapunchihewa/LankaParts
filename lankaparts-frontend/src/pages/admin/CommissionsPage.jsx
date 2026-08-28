import { Banknote, CreditCard, TrendingUp, Wallet } from 'lucide-react'
import { adminCommissionRows } from '../../data/admin'
import { formatLKR } from '../../utils/currency'
import { DataTable, PageHeader, StatCard } from '../../components/common/AdminUi'

export default function CommissionsPage() {
  const sales = adminCommissionRows.reduce((sum, row) => sum + row.grossSales, 0)
  const commission = sales * 0.03
  const columns = [{ label: 'Seller' }, { label: 'Gross Sales' }, { label: 'Commission Rate' }, { label: 'Commission' }, { label: 'Seller Earnings' }]
  return <section className="seller-page"><PageHeader eyebrow="Admin Commission" title="Commissions" description="Current LankaParts marketplace commission rate is 3%." /><div className="seller-card-grid"><StatCard icon={TrendingUp} label="Marketplace Sales" value={formatLKR(sales)} /><StatCard icon={Wallet} label="Commission Revenue" value={formatLKR(commission)} /><StatCard icon={CreditCard} label="Current Month Commission" value={formatLKR(adminCommissionRows[0].commission)} /><StatCard icon={Banknote} label="Pending Settlements" value={formatLKR(adminCommissionRows[0].sellerEarnings)} /></div><DataTable columns={columns} rows={adminCommissionRows} renderRow={(row) => <div className="admin-table-row" style={{ '--columns': columns.map((item) => item.width || '1fr').join(' ') }} key={row.seller}><strong>{row.seller}</strong><span>{formatLKR(row.grossSales)}</span><span>{row.commissionRate}</span><strong>{formatLKR(row.commission)}</strong><span>{formatLKR(row.sellerEarnings)}</span></div>} /></section>
}
