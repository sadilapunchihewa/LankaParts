import { CheckCircle2, ShieldCheck, Wrench } from 'lucide-react'
import { Link, Outlet } from 'react-router-dom'

export default function AuthLayout() {
  return <main className="auth-layout"><section className="auth-brand-panel"><Link to="/" className="brand auth-brand"><span className="brand-mark">LP</span><span>Lanka<span>Parts</span></span></Link><div className="auth-brand-copy"><span className="eyebrow">SRI LANKA'S AUTO PARTS MARKETPLACE</span><h1>The right parts.<br />The right people.</h1><p>Join a trusted marketplace connecting vehicle owners with verified spare-parts sellers across Sri Lanka.</p><ul><li><CheckCircle2 /> Vehicle-compatible part discovery</li><li><ShieldCheck /> Reviewed local seller businesses</li><li><Wrench /> Clear, genuine product information</li></ul></div><p className="auth-panel-foot">© 2026 LankaParts. Built for Sri Lankan motorists.</p></section><section className="auth-form-panel"><Link to="/" className="auth-back">← Back to marketplace</Link><Outlet /></section></main>
}
