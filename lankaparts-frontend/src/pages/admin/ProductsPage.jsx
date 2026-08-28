import { useState } from 'react'
import { Eye, ShieldCheck, Trash2, XCircle } from 'lucide-react'
import { toast } from 'sonner'
import { adminProducts } from '../../data/admin'
import { formatLKR } from '../../utils/currency'
import { Button, DataTable, Modal, PageHeader, SearchInput, StatusBadge } from '../../components/common/AdminUi'

export default function ProductsPage() {
  const [products, setProducts] = useState(adminProducts)
  const [selected, setSelected] = useState(null)
  const [search, setSearch] = useState('')
  const update = (product, status) => {
    const reason = status === 'Rejected' ? window.prompt('Rejection reason') : ''
    setProducts((current) => current.map((item) => item.id === product.id ? { ...item, approvalStatus: status, rejectionReason: reason || item.rejectionReason } : item))
    toast.success(`${product.name} marked ${status}.`)
  }
  const remove = (product) => {
    if (!window.confirm(`Remove ${product.name} from marketplace?`)) return
    setProducts((current) => current.filter((item) => item.id !== product.id))
    toast.success('Product removed.')
  }
  const rows = products.filter((product) => `${product.name} ${product.seller || product.companyName} ${product.categoryName}`.toLowerCase().includes(search.toLowerCase()))
  const columns = [{ label: 'Product', width: '1.5fr' }, { label: 'Seller' }, { label: 'Category' }, { label: 'Price' }, { label: 'Stock' }, { label: 'Status' }, { label: 'Actions', width: '1.25fr' }]
  return <section className="seller-page"><PageHeader eyebrow="Product Management" title="Products" description="Approve, reject, view, and remove spare-part listings." action={<SearchInput value={search} onChange={(event) => setSearch(event.target.value)} placeholder="Search products" />} /><DataTable columns={columns} rows={rows} renderRow={(product) => <div className="admin-table-row" style={{ '--columns': columns.map((item) => item.width || '1fr').join(' ') }} key={product.id}><strong>{product.name}<small>{product.rejectionReason}</small></strong><span>{product.seller || product.companyName || 'Seller'}</span><span>{product.categoryName}</span><span>{formatLKR(product.price)}</span><span>{product.stockQuantity}</span><StatusBadge status={product.approvalStatus || product.status} /><span className="row-actions wide"><button onClick={() => setSelected(product)} title="View"><Eye /></button><button onClick={() => update(product, 'Approved')} title="Approve"><ShieldCheck /></button><button onClick={() => update(product, 'Rejected')} title="Reject"><XCircle /></button><button onClick={() => remove(product)} title="Remove"><Trash2 /></button></span></div>} /><Modal title={selected?.name} onClose={() => setSelected(null)}>{selected && <p className="dialog-message">{selected.description || 'No description provided.'}</p>}<div className="dialog-actions"><Button variant="ghost" onClick={() => setSelected(null)}>Close</Button></div></Modal></section>
}
