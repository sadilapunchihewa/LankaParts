/* oxlint-disable react/set-state-in-effect -- These effects synchronize API-backed page state. */
import { useEffect, useMemo, useState } from 'react'
import { Eye, PackageSearch, Pencil, Plus, Trash2 } from 'lucide-react'
import { Link } from 'react-router-dom'
import { toast } from 'sonner'
import sellerService from '../../services/sellerService'
import { sellerProducts } from '../../data/seller'
import { formatLKR } from '../../utils/currency'
import { EmptyState, ErrorState, LoadingState } from '../../components/common/AdminUi'

const normalizeProduct = (part) => ({ ...part, categoryName: part.categoryName || 'Vehicle Parts', stockQuantity: part.stockQuantity ?? part.stock ?? 0, approvalStatus: part.approvalStatus || 'Pending', status: part.isActive === false ? 'Inactive' : 'Active' })

export default function ProductsPage() {
  const [products, setProducts] = useState(sellerProducts)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const loadProducts = async () => {
    setLoading(true); setError('')
    try { setProducts((await sellerService.getProducts()).map(normalizeProduct)) }
    catch { setError('Unable to load products.'); setProducts(sellerProducts) }
    finally { setLoading(false) }
  }
  useEffect(() => { loadProducts() }, [])
  const totals = useMemo(() => ({ all: products.length, active: products.filter((item) => item.status === 'Active').length, pending: products.filter((item) => item.approvalStatus === 'Pending').length }), [products])
  const remove = async (product) => {
    if (!window.confirm(`Delete ${product.name}? This will remove it from active selling.`)) return
    try { await sellerService.deleteProduct(product.id); setProducts((current) => current.filter((item) => item.id !== product.id)); toast.success('Product removed.') }
    catch { setProducts((current) => current.filter((item) => item.id !== product.id)); toast.success('Product removed locally.') }
  }
  return <section className="seller-page"><div className="seller-page-heading split"><div><span>Product Management</span><h1>Products</h1><p>Manage spare parts, inventory, marketplace status, and approval feedback.</p></div><Link className="seller-primary-action" to="/seller/products/new"><Plus /> Add Product</Link></div>{loading ? <LoadingState message="Loading products..." /> : error ? <ErrorState message={error} onRetry={loadProducts} /> : !products.length ? <EmptyState icon={PackageSearch} title="No products found." message="Try adding your first spare part or changing your filters." /> : <><div className="seller-mini-stats"><article><strong>{totals.all}</strong><span>Total Products</span></article><article><strong>{totals.active}</strong><span>Active Products</span></article><article><strong>{totals.pending}</strong><span>Pending Approval</span></article></div><div className="seller-table-wrap"><div className="seller-product-head"><span>Image</span><span>Product</span><span>Category</span><span>Price</span><span>Stock</span><span>Approval Status</span><span>Status</span><span>Actions</span></div>{products.map((product) => <div className="seller-product-row" key={product.id}><span className="product-thumb">{product.imageUrl ? <img src={product.imageUrl} alt="" /> : <PackageSearch />}</span><span><strong>{product.name}</strong><small>{product.brand || 'Brand not set'} | OEM {product.partNumber}</small>{product.rejectionReason && <em>{product.rejectionReason}</em>}</span><span>{product.categoryName}</span><span>{formatLKR(product.price)}</span><span>{product.stockQuantity}</span><span><b className={`status-badge status-${product.approvalStatus.toLowerCase()}`}>{product.approvalStatus}</b></span><span><b className={`status-badge ${product.status === 'Active' ? 'status-delivered' : 'status-cancelled'}`}>{product.status}</b></span><span className="row-actions"><Link to={`/products/${product.id}`} title="View"><Eye /></Link><Link to={`/seller/products/${product.id}/edit`} title="Edit"><Pencil /></Link><button onClick={() => remove(product)} title="Delete"><Trash2 /></button></span></div>)}</div></>}</section>
}
