import { Check, Eye, ShoppingCart, Star } from 'lucide-react'
import { Link } from 'react-router-dom'
import { formatLKR } from '../../utils/currency'
import PartVisual from './PartVisual'

export default function ProductCard({ product, onAdd, view = 'grid' }) {
  return <article className={`product-card ${view === 'list' ? 'list-card' : ''}`}><div className={`product-visual ${product.type}`}><span className="product-badge">{product.condition}</span><PartVisual product={product} /><Link to={`/products/${product.id}`} aria-label={`View ${product.name}`}><Eye size={18} /></Link></div><div className="product-info"><span className="product-brand">{product.brand.toUpperCase()}</span><Link to={`/products/${product.id}`}><h3>{product.name}</h3></Link><p className="compatibility">Fits: {product.vehicle}</p><div className="seller-row"><span><Check size={11} /> {product.seller}</span><span><Star size={12} fill="currentColor" /> {product.rating}</span></div><div className="product-bottom"><div><strong>{formatLKR(product.price)}</strong><small className={product.stock < 7 ? 'low-stock' : ''}>{product.stock < 7 ? `Only ${product.stock} left` : 'In stock'}</small></div><button onClick={() => onAdd(product)} aria-label={`Add ${product.name} to cart`}><ShoppingCart size={18} /></button></div></div></article>
}
