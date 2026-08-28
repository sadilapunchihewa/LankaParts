import { adminOrders } from '../../data/admin'
import { formatLKR } from '../../utils/currency'
import { DataTable, PageHeader, StatusBadge } from '../../components/common/AdminUi'

export default function OrdersPage() {
  const columns = [{ label: 'Order' }, { label: 'Customer' }, { label: 'Seller' }, { label: 'Date' }, { label: 'Total' }, { label: 'Status' }]
  return <section className="seller-page"><PageHeader eyebrow="Order Monitoring" title="Orders" description="Monitor all marketplace orders across customers and sellers." /><DataTable columns={columns} rows={adminOrders} renderRow={(order) => <div className="admin-table-row" style={{ '--columns': columns.map((item) => item.width || '1fr').join(' ') }} key={order.orderId}><strong>{order.orderNumber}</strong><span>{order.customerName}</span><span>{order.seller}</span><span>{new Date(order.createdAt).toLocaleDateString('en-LK')}</span><strong>{formatLKR(order.sellerSubtotal)}</strong><StatusBadge status={order.status} /></div>} /></section>
}
