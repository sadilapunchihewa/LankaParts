import { useState } from 'react'
import { Link } from 'react-router-dom'
import { LogOut, Menu, Search, ShoppingCart, UserRound, X } from 'lucide-react'
import { useAuth } from '../../contexts/AuthContext'

const roleLink = { Customer: '/account', Seller: '/seller/dashboard', Admin: '/admin/dashboard' }
const roleLabel = { Customer: 'My Account', Seller: 'Seller Dashboard', Admin: 'Admin Dashboard' }

export default function Navbar({ cartCount }) {
  const [open, setOpen] = useState(false)
  const { user, logout } = useAuth()
  const close = () => setOpen(false)
  return <header className="site-header"><div className="utility-bar"><div className="container utility-content"><span>Islandwide delivery across Sri Lanka</span><span>Seller support: +94 11 234 5678</span></div></div><nav className="container navbar" aria-label="Main navigation">
    <Link to="/" className="brand"><span className="brand-mark">LP</span><span>Lanka<span>Parts</span></span></Link><div className="desktop-links"><a href="/#home" className="active">Home</a><Link to="/products">Shop Parts</Link><a href="/#categories">Categories</a><a href="/#vehicle-search">Find by Vehicle</a></div><div className="nav-actions"><button className="icon-button" aria-label="Search"><Search size={20} /></button>{user ? <><Link className="login-link" to={roleLink[user.role] || '/'}><UserRound size={19} /> {roleLabel[user.role]}</Link><button className="logout-button" onClick={logout}><LogOut size={17} /> Logout</button></> : <><Link className="login-link" to="/auth/login"><UserRound size={19} /> Login</Link><Link className="register-link" to="/auth/register">Register</Link></>}<Link className="cart-button" to="/cart" aria-label={`Cart with ${cartCount} items`}><ShoppingCart size={21} />{cartCount > 0 && <span className="cart-count">{cartCount}</span>}</Link><button className="menu-button" aria-label="Toggle menu" aria-expanded={open} onClick={() => setOpen(!open)}>{open ? <X /> : <Menu />}</button></div>
  </nav>{open && <div className="mobile-menu"><a href="/#home" onClick={close}>Home</a><Link to="/products" onClick={close}>Shop Parts</Link><a href="/#categories" onClick={close}>Categories</a><Link to="/cart" onClick={close}>Cart ({cartCount})</Link>{user ? <><Link to={roleLink[user.role] || '/'} onClick={close}>{roleLabel[user.role]}</Link><button onClick={() => { logout(); close() }}>Logout</button></> : <><Link to="/auth/login" onClick={close}>Login</Link><Link to="/auth/register" onClick={close}>Register</Link></>}</div>}</header>
}
