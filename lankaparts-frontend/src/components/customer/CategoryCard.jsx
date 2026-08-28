export default function CategoryCard({ category }) {
  const Icon = category.icon
  return <a href="#products" className="category-card"><span className="category-icon"><Icon size={25} strokeWidth={1.7} /></span><strong>{category.name}</strong><small>{category.count.toLocaleString()} parts</small><span className="card-arrow">→</span></a>
}
