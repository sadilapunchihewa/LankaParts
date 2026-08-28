export default function SectionHeader({ eyebrow, title, description, action }) {
  return <div className="section-header"><div><span>{eyebrow}</span><h2>{title}</h2>{description && <p>{description}</p>}</div>{action && <a href="#products">{action} <span>→</span></a>}</div>
}
