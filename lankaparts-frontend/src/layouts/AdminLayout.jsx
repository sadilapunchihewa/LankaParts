import { Boxes, Building2, CreditCard, FileText, FolderTree, LayoutDashboard, ReceiptText, Users, Wallet } from 'lucide-react'
import { Outlet, useNavigate } from 'react-router-dom'
import { DashboardSidebar } from '../components/common/AdminUi'
import { useAuth } from '../contexts/AuthContext'
export default function AdminLayout() {
  const { logout } = useAuth(); const navigate = useNavigate(); const signOut = () => { logout(); navigate('/') }
  const links = [
    { to: '/admin/dashboard', label: 'Dashboard', icon: LayoutDashboard },
    { to: '/admin/users', label: 'Users', icon: Users },
    { to: '/admin/sellers', label: 'Sellers', icon: Building2 },
    { to: '/admin/products', label: 'Products', icon: Boxes },
    { to: '/admin/categories', label: 'Categories', icon: FolderTree },
    { to: '/admin/orders', label: 'Orders', icon: ReceiptText },
    { to: '/admin/commissions', label: 'Commissions', icon: CreditCard },
    { to: '/admin/settlements', label: 'Settlements', icon: Wallet },
    { to: '/admin/reports', label: 'Reports', icon: FileText },
  ]
  return <div className="dashboard-layout admin-layout"><DashboardSidebar brand="Lanka" role="ADMIN PORTAL" links={links} onLogout={signOut} /><main><Outlet /></main></div>
}
