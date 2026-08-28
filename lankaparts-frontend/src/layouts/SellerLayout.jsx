import { BarChart3, Building2, LayoutDashboard, PackagePlus, PackageSearch, ReceiptText, ShoppingBag, Wallet } from 'lucide-react'
import { Outlet, useNavigate } from 'react-router-dom'
import { DashboardSidebar } from '../components/common/AdminUi'
import { useAuth } from '../contexts/AuthContext'
export default function SellerLayout() {
  const { logout } = useAuth(); const navigate = useNavigate(); const signOut = () => { logout(); navigate('/') }
  const links = [
    { to: '/seller/dashboard', label: 'Overview', icon: LayoutDashboard },
    { to: '/seller/products', label: 'Products', icon: ShoppingBag },
    { to: '/seller/products/new', label: 'Add Product', icon: PackagePlus },
    { to: '/seller/orders', label: 'Orders', icon: ReceiptText },
    { to: '/seller/sales', label: 'Sales', icon: BarChart3 },
    { to: '/seller/earnings', label: 'Earnings', icon: Wallet },
    { to: '/seller/company-profile', label: 'Company Profile', icon: Building2 },
    { to: '/seller/register-company', label: 'Verification', icon: PackageSearch },
  ]
  return <div className="dashboard-layout seller-layout"><DashboardSidebar brand="Lanka" role="SELLER PORTAL" links={links} onLogout={signOut} /><main><Outlet /></main></div>
}
