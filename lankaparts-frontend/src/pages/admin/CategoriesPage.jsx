import { categories } from '../../data/marketplace'
import { DataTable, PageHeader, StatusBadge } from '../../components/common/AdminUi'

export default function CategoriesPage() {
  const rows = categories.map((category, index) => ({ ...category, id: index + 1, status: 'Active' }))
  const columns = [{ label: 'Category' }, { label: 'Listings' }, { label: 'Status' }]
  return <section className="seller-page"><PageHeader eyebrow="Catalog" title="Categories" description="Review marketplace catalog categories and listing volume." /><DataTable columns={columns} rows={rows} renderRow={(row) => <div className="admin-table-row" style={{ '--columns': columns.map((item) => item.width || '1fr').join(' ') }} key={row.name}><strong>{row.name}</strong><span>{row.count}</span><StatusBadge status={row.status} /></div>} /></section>
}
