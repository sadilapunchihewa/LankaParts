import { Link } from 'react-router-dom'
import { BarChart3, Boxes, Clock3, PackageCheck, PackageOpen, ReceiptText, Wallet } from 'lucide-react'
import { useAuth } from '../../contexts/AuthContext'
import { sellerOrders, sellerProducts } from '../../data/seller'
import { formatLKR } from '../../utils/currency'

export default function SellerDashboardPage() {
  const { user } = useAuth()
  const activeProducts = sellerProducts.filter((item) => item.status === 'Active').length
  const pendingProducts = sellerProducts.filter((item) => item.approvalStatus === 'Pending').length
  const totalSales = sellerOrders.reduce((sum, order) => sum + order.sellerSubtotal, 0)
  const netEarnings = totalSales * 0.97
  const cards = [
    { label: 'Total Products', value: sellerProducts.length, icon: Boxes },
    { label: 'Active Products', value: activeProducts, icon: PackageCheck },
    { label: 'Pending Products', value: pendingProducts, icon: Clock3 },
    { label: 'Orders', value: sellerOrders.length, icon: ReceiptText },
    { label: 'Total Sales', value: formatLKR(totalSales), icon: BarChart3 },
    { label: 'Net Earnings', value: formatLKR(netEarnings), icon: Wallet },
  ]
  return <section className="seller-page"><div className="seller-page-heading split"><div><span>Seller Workspace</span><h1>Welcome, {user?.firstName || 'Seller'}</h1><p>Track product approvals, orders, sales activity, and earnings from one professional dashboard.</p></div><Link className="seller-primary-action" to="/seller/products/new"><PackageOpen /> Add Product</Link></div><div className="seller-card-grid">{cards.map(({ label, value, icon: Icon }) => <article key={label}><Icon /><span>{label}</span><strong>{value}</strong></article>)}</div><div className="seller-dashboard-grid"><section className="seller-panel-block"><div className="panel-heading"><div><h2>Recent orders</h2><p>Latest buyer activity requiring fulfillment attention.</p></div><Link to="/seller/orders">View all</Link></div>{sellerOrders.map((order) => <div className="seller-list-row" key={order.orderId}><span><strong>{order.orderNumber}</strong><small>{order.customerName} | {order.items.length} items</small></span><b className={`status-badge status-${order.status.toLowerCase()}`}>{order.status}</b><strong>{formatLKR(order.sellerSubtotal)}</strong></div>)}</section><section className="seller-panel-block"><div className="panel-heading"><div><h2>Sales overview</h2><p>Gross sales, commission, and settlement estimate.</p></div><Link to="/seller/earnings">Earnings</Link></div><div className="sales-bars">{[72, 54, 86, 61, 94, 78].map((height, index) => <span key={height} style={{ '--height': `${height}%` }}><i></i><small>{['Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug'][index]}</small></span>)}</div><dl className="seller-summary-list"><div><dt>Gross Sales</dt><dd>{formatLKR(totalSales)}</dd></div><div><dt>Platform Commission (3%)</dt><dd>{formatLKR(totalSales * 0.03)}</dd></div><div><dt>Net Earnings</dt><dd>{formatLKR(netEarnings)}</dd></div></dl></section></div></section>
}
