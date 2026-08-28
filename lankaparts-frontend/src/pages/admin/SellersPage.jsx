/* oxlint-disable react/set-state-in-effect -- These effects synchronize API-backed page state. */
import { useEffect, useState } from 'react'
import { Eye, ShieldCheck, UserX, XCircle } from 'lucide-react'
import { toast } from 'sonner'
import adminService from '../../services/adminService'
import { adminSellers } from '../../data/admin'
import { Button, DataTable, EmptyState, ErrorState, LoadingState, Modal, PageHeader, SearchInput, StatusBadge } from '../../components/common/AdminUi'

export default function SellersPage() {
  const [sellers, setSellers] = useState(adminSellers)
  const [selected, setSelected] = useState(null)
  const [search, setSearch] = useState('')
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const loadSellers = async () => {
    setLoading(true); setError('')
    try { setSellers(await adminService.getSellers()) }
    catch { setError('Unable to load sellers.'); setSellers(adminSellers) }
    finally { setLoading(false) }
  }
  useEffect(() => { loadSellers() }, [])
  const review = async (seller, status) => {
    const note = status === 'Approved' ? 'Approved by marketplace admin.' : window.prompt('Rejection or suspension note') || 'Requires additional review.'
    try {
      if (status === 'Approved') await adminService.approveSeller(seller.id, note)
      else await adminService.rejectSeller(seller.id, note)
    } catch { /* local fallback */ }
    setSellers((current) => current.map((item) => item.id === seller.id ? { ...item, status, reviewNote: note } : item))
    toast.success(`${seller.companyName} marked ${status}.`)
  }
  const rows = sellers.filter((seller) => `${seller.companyName} ${seller.sellerName} ${seller.businessRegistrationNumber}`.toLowerCase().includes(search.toLowerCase()))
  const columns = [{ label: 'Company', width: '1.4fr' }, { label: 'Owner' }, { label: 'Registration Number' }, { label: 'Joined' }, { label: 'Status' }, { label: 'Actions', width: '1.4fr' }]
  return <section className="seller-page"><PageHeader eyebrow="Seller Management" title="Sellers" description="Review, approve, reject, or suspend seller company applications." action={<SearchInput value={search} onChange={(event) => setSearch(event.target.value)} placeholder="Search sellers" />} />{loading ? <LoadingState message="Loading sellers..." /> : error ? <ErrorState message={error} onRetry={loadSellers} /> : !rows.length ? <EmptyState icon={Eye} title="No sellers found." message="Try changing your filters." /> : <DataTable columns={columns} rows={rows} renderRow={(seller) => <div className="admin-table-row" style={{ '--columns': columns.map((item) => item.width || '1fr').join(' ') }} key={seller.id}><strong>{seller.companyName}</strong><span>{seller.sellerName}<small>{seller.sellerEmail}</small></span><span>{seller.businessRegistrationNumber}</span><span>{new Date(seller.createdAt).toLocaleDateString('en-LK')}</span><StatusBadge status={seller.status} /><span className="row-actions wide"><button onClick={() => setSelected(seller)} title="View"><Eye /></button><button onClick={() => review(seller, 'Approved')} title="Approve"><ShieldCheck /></button><button onClick={() => review(seller, 'Rejected')} title="Reject"><XCircle /></button><button onClick={() => review(seller, 'Suspended')} title="Suspend"><UserX /></button></span></div>} />}<Modal title={selected?.companyName} onClose={() => setSelected(null)}>{selected && <dl className="modal-details"><div><dt>Owner</dt><dd>{selected.sellerName}</dd></div><div><dt>Email</dt><dd>{selected.sellerEmail}</dd></div><div><dt>Registration</dt><dd>{selected.businessRegistrationNumber}</dd></div><div><dt>Address</dt><dd>{selected.address}, {selected.city}</dd></div><div><dt>Phone</dt><dd>{selected.phoneNumber || 'Not provided'}</dd></div><div><dt>Status</dt><dd><StatusBadge status={selected.status} /></dd></div>{selected.reviewNote && <div><dt>Review Note</dt><dd>{selected.reviewNote}</dd></div>}</dl>}<div className="dialog-actions"><Button variant="ghost" onClick={() => setSelected(null)}>Close</Button></div></Modal></section>
}
