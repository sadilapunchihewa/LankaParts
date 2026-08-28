import { Outlet } from 'react-router-dom'
import Navbar from '../components/common/Navbar'
import Footer from '../components/common/Footer'
import { useCart } from '../contexts/CartContext'
export default function MainLayout() { const { itemCount } = useCart(); return <div className="site-shell"><Navbar cartCount={itemCount} /><Outlet /><Footer /></div> }
