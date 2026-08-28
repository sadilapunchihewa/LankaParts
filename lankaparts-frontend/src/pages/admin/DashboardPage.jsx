import { useEffect, useMemo, useState } from 'react'
import { BarChart3, Boxes, Building2, Clock3, PackageSearch, ReceiptText, TrendingUp, Users, Wallet } from 'lucide-react'
import { useAuth } from '../../contexts/AuthContext'
import { adminOrders, adminProducts, adminSellers, monthlySales, popularCategories } from '../../data/admin'
import adminService from '../../services/adminService'
import { formatLKR } from '../../utils/currency'
import { PageHeader, StatCard, StatusBadge } from '../../components/common/AdminUi'

export default function AdminDashboardPage() {
  const { user } = useAuth()
  const [remoteStats, setRemoteStats] = useState(null)
  useEffect(() => { adminService.getDashboard().then(setRemoteStats).catch(() => null) }, [])
  const stats = useMemo(() => {
    const totalSales = adminOrders.reduce((sum, order) => sum + order.sellerSubtotal, 0)
    return {
      totalUsers: remoteStats?.totalUsers || 1248,
      totalSellers: remoteStats?.activeSellers || adminSellers.length,
      pendingSellers: remoteStats?.pendingSellerCompanies || adminSellers.filter((seller) => seller.status === 'Pending').length,
      totalProducts: remoteStats?.activePartListings || adminProducts.length,
      pendingProducts: adminProducts.filter((product) => product.approvalStatus === 'Pending').length,
      totalOrders: remoteStats?.totalOrders || adminOrders.length,
      totalSales: remoteStats?.paidRevenue || totalSales,
      platformRevenue: (remoteStats?.paidRevenue || totalSales) * 0.03,
    }
  }, [remoteStats])
  const cards = [
    { label: 'Total Users', value: stats.totalUsers, icon: Users },
    { label: 'Total Sellers', value: stats.totalSellers, icon: Building2 },
    { label: 'Pending Sellers', value: stats.pendingSellers, icon: Clock3 },
    { label: 'Total Products', value: stats.totalProducts, icon: Boxes },
    { label: 'Pending Products', value: stats.pendingProducts, icon: PackageSearch },
    { label: 'Total Orders', value: stats.totalOrders, icon: ReceiptText },
    { label: 'Total Sales', value: formatLKR(stats.totalSales), icon: TrendingUp },
    { label: 'Platform Revenue', value: formatLKR(stats.platformRevenue), icon: Wallet },
  ]
  return <section className="seller-page"><PageHeader eyebrow="Marketplace Administration" title={`Welcome, ${user?.firstName || 'Admin'}`} description="Monitor marketplace health, approvals, orders, revenue, sellers, and categories." /><div className="admin-stat-grid">{cards.map((card) => <StatCard key={card.label} {...card} />)}</div><div className="admin-dashboard-grid"><ChartPanel title="Monthly sales" values={monthlySales} /><ChartPanel title="Order trends" values={[18, 24, 17, 31, 27, 36]} /><section className="seller-panel-block"><div className="panel-heading"><div><h2>Marketplace commission</h2><p>Commission revenue at the current 3% rate.</p></div><BarChart3 /></div><dl className="seller-summary-list"><div><dt>Total Sales</dt><dd>{formatLKR(stats.totalSales)}</dd></div><div><dt>Rate</dt><dd>3%</dd></div><div><dt>Platform Revenue</dt><dd>{formatLKR(stats.platformRevenue)}</dd></div></dl></section><section className="seller-panel-block"><div className="panel-heading"><div><h2>Top sellers</h2><p>Highest performing seller companies.</p></div></div>{adminSellers.slice(0, 4).map((seller) => <div className="seller-list-row" key={seller.id}><span><strong>{seller.companyName}</strong><small>{seller.city}</small></span><StatusBadge status={seller.status} /></div>)}</section><section className="seller-panel-block"><div className="panel-heading"><div><h2>Popular categories</h2><p>Most visited product groups.</p></div></div>{popularCategories.map((category, index) => <div className="seller-list-row" key={category}><span><strong>{category}</strong><small>{1200 - index * 160} views</small></span><strong>{index + 1}</strong></div>)}</section></div></section>
}

function ChartPanel({ title, values }) {
  const max = Math.max(...values)
  return <section className="seller-panel-block"><div className="panel-heading"><div><h2>{title}</h2><p>Clean native chart placeholder without extra dependencies.</p></div><BarChart3 /></div><div className="sales-bars large">{values.map((value, index) => <span key={`${title}-${index}`} style={{ '--height': `${(value / max) * 92}%` }}><i></i><small>{['Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug'][index]}</small></span>)}</div></section>
}
