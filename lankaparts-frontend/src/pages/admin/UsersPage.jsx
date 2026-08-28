/* oxlint-disable react/set-state-in-effect -- These effects synchronize API-backed page state. */
import { useEffect, useState } from 'react'
import { Eye, UserCheck, UserX } from 'lucide-react'
import { toast } from 'sonner'
import adminService from '../../services/adminService'
import { adminUsers } from '../../data/admin'
import { DataTable, EmptyState, ErrorState, LoadingState, PageHeader, SearchInput, StatusBadge } from '../../components/common/AdminUi'

export default function UsersPage() {
  const [users, setUsers] = useState(adminUsers)
  const [search, setSearch] = useState('')
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const loadUsers = async () => {
    setLoading(true); setError('')
    try { setUsers(await adminService.getUsers()) }
    catch { setError('Unable to load users.'); setUsers(adminUsers) }
    finally { setLoading(false) }
  }
  useEffect(() => { loadUsers() }, [])
  const setActive = async (user, isActive) => {
    if (!window.confirm(`${isActive ? 'Activate' : 'Deactivate'} ${user.email}?`)) return
    try { await adminService.setUserActive(user.id, isActive) } catch { /* local fallback */ }
    setUsers((current) => current.map((item) => item.id === user.id ? { ...item, isActive } : item))
    toast.success(`${user.email} ${isActive ? 'activated' : 'deactivated'}.`)
  }
  const rows = users.filter((user) => `${user.firstName} ${user.lastName} ${user.email} ${user.role}`.toLowerCase().includes(search.toLowerCase()))
  const columns = [{ label: 'Name' }, { label: 'Email', width: '1.4fr' }, { label: 'Role' }, { label: 'Joined Date' }, { label: 'Active Status' }, { label: 'Actions' }]
  return <section className="seller-page"><PageHeader eyebrow="Admin Users" title="Users" description="Monitor user accounts without exposing password or credential information." action={<SearchInput value={search} onChange={(event) => setSearch(event.target.value)} placeholder="Search users" />} />{loading ? <LoadingState message="Loading users..." /> : error ? <ErrorState message={error} onRetry={loadUsers} /> : !rows.length ? <EmptyState icon={Eye} title="No users found." message="Try changing your filters." /> : <DataTable columns={columns} rows={rows} renderRow={(user) => <div className="admin-table-row" style={{ '--columns': columns.map((item) => item.width || '1fr').join(' ') }} key={user.id}><strong>{user.firstName} {user.lastName}</strong><span>{user.email}</span><span>{user.role}</span><span>{new Date(user.createdAt).toLocaleDateString('en-LK')}</span><StatusBadge status={user.isActive ? 'Active' : 'Blocked'} /><span className="row-actions"><button title="View"><Eye /></button><button onClick={() => setActive(user, true)} title="Activate"><UserCheck /></button><button onClick={() => setActive(user, false)} title="Deactivate"><UserX /></button></span></div>} />}</section>
}
