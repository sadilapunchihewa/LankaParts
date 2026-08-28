/* oxlint-disable react/set-state-in-effect -- These effects synchronize API-backed page state. */
import { useEffect, useState } from 'react'
import { Eye } from 'lucide-react'
import { Link } from 'react-router-dom'
import sellerService from '../../services/sellerService'
import { sellerOrders } from '../../data/seller'
import { formatLKR } from '../../utils/currency'
import { EmptyState, ErrorState, LoadingState } from '../../components/common/AdminUi'

export default function OrdersPage() {
  const [orders, setOrders] = useState(sellerOrders)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const loadOrders = async () => {
    setLoading(true); setError('')
    try { setOrders(await sellerService.getOrders()) }
    catch { setError('Unable to load orders.'); setOrders(sellerOrders) }
    finally { setLoading(false) }
  }
  useEffect(() => { loadOrders() }, [])
  return <section className="seller-page"><div className="seller-page-heading"><span>Seller Orders</span><h1>Orders</h1><p>Review customer orders, item counts, totals, and fulfillment status.</p></div>{loading ? <LoadingState message="Loading orders..." /> : error ? <ErrorState message={error} onRetry={loadOrders} /> : !orders.length ? <EmptyState icon={Eye} title="No orders found." message="New seller orders will appear here." /> : <div className="seller-table-wrap"><div className="seller-order-head"><span>Order ID</span><span>Customer</span><span>Date</span><span>Items</span><span>Amount</span><span>Status</span><span>Actions</span></div>{orders.map((order) => <div className="seller-order-row" key={order.orderId}><strong>{order.orderNumber}</strong><span>{order.customerName}<small>{order.customerEmail}</small></span><span>{new Date(order.createdAt).toLocaleDateString('en-LK')}</span><span>{order.items.length}</span><strong>{formatLKR(order.sellerSubtotal)}</strong><b className={`status-badge status-${order.status.toLowerCase()}`}>{order.status}</b><span className="row-actions"><Link to={`/seller/orders/${order.orderId}`} title="View order"><Eye /></Link></span></div>)}</div>}</section>
}
