/* oxlint-disable react/set-state-in-effect, react-hooks/exhaustive-deps -- These effects synchronize API-backed page state. */
import { useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import { ArrowLeft, Truck } from 'lucide-react'
import { toast } from 'sonner'
import sellerService from '../../services/sellerService'
import { orderStatusFlow, sellerOrders } from '../../data/seller'
import { formatLKR } from '../../utils/currency'
import { ErrorState, LoadingState } from '../../components/common/AdminUi'

export default function OrderDetailsPage() {
  const { id } = useParams()
  const [order, setOrder] = useState(() => sellerOrders.find((item) => String(item.orderId) === id) || sellerOrders[0])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const loadOrder = async () => {
    setLoading(true); setError('')
    try { setOrder(await sellerService.getOrder(id)) }
    catch { setError('Unable to load order details.') }
    finally { setLoading(false) }
  }
  useEffect(() => { loadOrder() }, [id])
  const currentIndex = orderStatusFlow.indexOf(order.status)
  const nextStatus = orderStatusFlow[currentIndex + 1]
  const updateStatus = async () => {
    if (!nextStatus || !window.confirm(`Update ${order.orderNumber} to ${nextStatus}?`)) return
    try { setOrder(await sellerService.updateOrderStatus(order.orderId, nextStatus)); toast.success(`Order marked ${nextStatus}.`) }
    catch { setOrder((current) => ({ ...current, status: nextStatus })); toast.success(`Order marked ${nextStatus}.`) }
  }
  if (loading) return <section className="seller-page"><LoadingState message="Loading order details..." /></section>
  if (error && !order) return <section className="seller-page"><ErrorState message={error} onRetry={loadOrder} /></section>
  return <section className="seller-page"><Link className="account-back" to="/seller/orders"><ArrowLeft /> Back to orders</Link>{error && <ErrorState message={error} onRetry={loadOrder} />}<div className="seller-page-heading split"><div><span>Order Details</span><h1>{order.orderNumber}</h1><p>{order.customerName} | {new Date(order.createdAt).toLocaleString('en-LK')}</p></div>{nextStatus && <button className="seller-primary-action" onClick={updateStatus}><Truck /> Mark {nextStatus}</button>}</div><div className="seller-progress">{orderStatusFlow.map((status, index) => <span key={status} className={index <= currentIndex ? 'complete' : ''}>{status}</span>)}</div><div className="order-detail-grid"><section className="account-panel ordered-items"><h2>Items</h2>{order.items.map((item) => <div key={item.orderItemId}><span className="product-thumb"><Truck /></span><span><strong>{item.partName}</strong><small>OEM {item.partNumber} | Qty {item.quantity}</small></span><strong>{formatLKR(item.lineTotal)}</strong></div>)}</section><aside><section className="account-panel delivery-card"><h2><Truck /> Customer</h2><strong>{order.customerName}</strong><p>{order.customerEmail}<br />{order.contactPhone}<br />{order.shippingAddress}, {order.shippingCity}</p></section><section className="account-panel order-total"><h2>Order Total</h2><dl><div><dt>Items</dt><dd>{order.items.length}</dd></div><div><dt>Amount</dt><dd>{formatLKR(order.sellerSubtotal)}</dd></div><div><dt>Status</dt><dd>{order.status}</dd></div></dl></section></aside></div></section>
}
