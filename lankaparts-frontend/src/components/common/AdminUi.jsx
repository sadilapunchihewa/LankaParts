import { useState } from 'react'
import { NavLink } from 'react-router-dom'
import { Loader2, LogOut, Menu, Search, X } from 'lucide-react'

export function Button({ children, variant = 'primary', ...props }) {
  return <button className={`ui-button ${variant}`} {...props}>{children}</button>
}

export function Input(props) {
  return <input className="ui-input" {...props} />
}

export function Select({ children, ...props }) {
  return <select className="ui-input" {...props}>{children}</select>
}

export function Modal({ title, children, onClose }) {
  if (!title) return null
  return <div className="modal-overlay" onMouseDown={(event) => event.target === event.currentTarget && onClose?.()}><section className="ui-modal"><header><h2>{title}</h2><button onClick={onClose}>Close</button></header>{children}</section></div>
}

export function ConfirmDialog({ title, message, onConfirm, onCancel }) {
  if (!title) return null
  return <Modal title={title} onClose={onCancel}><p className="dialog-message">{message}</p><div className="dialog-actions"><Button variant="ghost" onClick={onCancel}>Cancel</Button><Button onClick={onConfirm}>Confirm</Button></div></Modal>
}

export function LoadingSpinner() {
  return <Loader2 className="loading-spinner" />
}

export function LoadingState({ message = 'Loading...' }) {
  return <div className="loading-state"><LoadingSpinner /><span>{message}</span></div>
}

export function EmptyState({ icon: Icon, title, message }) {
  return <div className="empty-state compact">{Icon && <Icon />}<h2>{title}</h2><p>{message}</p></div>
}

export function ErrorState({ message, onRetry }) {
  return <div className="error-state"><strong>{message || 'Unable to load data.'}</strong>{onRetry && <Button variant="ghost" onClick={onRetry}>Retry</Button>}</div>
}

export function StatusBadge({ status }) {
  const normalized = String(status || 'Pending').toLowerCase()
  return <b className={`status-badge status-${normalized}`}>{status || 'Pending'}</b>
}

export function Pagination({ page = 1 }) {
  return <nav className="pagination compact" aria-label="Pagination"><button disabled={page === 1}>←</button><button className="active">{page}</button><button>2</button><button>3</button><button>→</button></nav>
}

export function SearchInput({ value, onChange, placeholder = 'Search' }) {
  return <label className="search-input"><Search /><input value={value} onChange={onChange} placeholder={placeholder} /></label>
}

export function AdminProductCard({ product }) {
  return <article className="admin-product-card"><strong>{product.name}</strong><span>{product.seller} | {product.category}</span></article>
}

export function StatCard({ label, value, icon: Icon }) {
  return <article className="seller-card-grid-item">{Icon && <Icon />}<span>{label}</span><strong>{value}</strong></article>
}

export function DashboardSidebar({ brand, role, links, onLogout }) {
  const [open, setOpen] = useState(false)
  const close = () => setOpen(false)
  return <><button className="dashboard-menu-button" onClick={() => setOpen(true)} aria-label="Open dashboard menu"><Menu /></button><div className={`dashboard-drawer ${open ? 'open' : ''}`} onMouseDown={(event) => event.target === event.currentTarget && close()}><aside><button className="drawer-close" onClick={close} aria-label="Close dashboard menu"><X /></button><NavLink to="/" className="brand dashboard-brand" onClick={close}><span className="brand-mark">LP</span><span>{brand}<span>Parts</span></span></NavLink><span className="dashboard-role">{role}</span><nav>{links.map(({ to, label, icon: Icon }) => <NavLink key={to} to={to} end={to.endsWith('/dashboard')} onClick={close}><Icon /> {label}</NavLink>)}</nav><button onClick={onLogout}><LogOut /> Logout</button></aside></div></>
}

export function DataTable({ columns, rows, renderRow }) {
  return <div className="seller-table-wrap"><div className="admin-table-head" style={{ '--columns': columns.map((item) => item.width || '1fr').join(' ') }}>{columns.map((column) => <span key={column.label}>{column.label}</span>)}</div>{rows.map(renderRow)}</div>
}

export function PageHeader({ eyebrow, title, description, action }) {
  return <div className="seller-page-heading split"><div><span>{eyebrow}</span><h1>{title}</h1><p>{description}</p></div>{action}</div>
}
