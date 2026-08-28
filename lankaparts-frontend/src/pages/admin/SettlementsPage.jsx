import { adminSettlements } from '../../data/admin'
import { formatLKR } from '../../utils/currency'
import { DataTable, PageHeader, StatusBadge } from '../../components/common/AdminUi'

export default function SettlementsPage() {
  const columns = [{ label: 'Seller' }, { label: 'Month' }, { label: 'Gross Sales' }, { label: 'Commission' }, { label: 'Net Seller Amount' }, { label: 'Status' }]
  return <section className="seller-page"><PageHeader eyebrow="Admin Settlements" title="Settlements" description="Track pending and paid seller settlement batches." /><DataTable columns={columns} rows={adminSettlements} renderRow={(row) => <div className="admin-table-row" style={{ '--columns': columns.map((item) => item.width || '1fr').join(' ') }} key={`${row.seller}-${row.month}`}><strong>{row.seller}</strong><span>{row.month}</span><span>{formatLKR(row.grossSales)}</span><span>{formatLKR(row.commission)}</span><strong>{formatLKR(row.sellerEarnings)}</strong><StatusBadge status={row.status} /></div>} /></section>
}
