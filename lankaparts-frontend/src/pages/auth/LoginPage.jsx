import { useState } from 'react'
import { Link, useLocation, useNavigate } from 'react-router-dom'
import { Mail } from 'lucide-react'
import { toast } from 'sonner'
import PasswordField from '../../components/common/PasswordField'
import { useAuth } from '../../contexts/AuthContext'

const roleHome = { Customer: '/', Seller: '/seller/dashboard', Admin: '/admin/dashboard' }

export default function LoginPage() {
  const { login } = useAuth()
  const navigate = useNavigate()
  const location = useLocation()
  const [form, setForm] = useState({ email: '', password: '', remember: false })
  const [loading, setLoading] = useState(false)
  const update = ({ target }) => setForm((current) => ({ ...current, [target.name]: target.type === 'checkbox' ? target.checked : target.value }))
  const submit = async (event) => {
    event.preventDefault(); setLoading(true)
    try {
      const user = await login({ email: form.email.trim(), password: form.password }, form.remember)
      toast.success(`Welcome back, ${user.firstName}`)
      const requestedPath = location.state?.from?.pathname
      const allowedRequestedPath = requestedPath && (requestedPath.startsWith(`/${user.role.toLowerCase()}/`) || user.role === 'Customer')
      navigate(allowedRequestedPath ? requestedPath : roleHome[user.role] || '/', { replace: true })
    } catch (error) { toast.error(error.response?.data?.message || 'Unable to sign in. Please try again.') } finally { setLoading(false) }
  }
  return <div className="auth-card"><div className="auth-heading"><span className="auth-kicker">WELCOME BACK</span><h2>Sign in to LankaParts</h2><p>Access your account, orders, and marketplace tools.</p></div><form className="auth-form" onSubmit={submit}><label className="form-field"><span>Email address</span><div className="input-shell"><Mail size={17} /><input type="email" name="email" value={form.email} onChange={update} autoComplete="email" placeholder="you@example.com" required /></div></label><PasswordField label="Password" name="password" value={form.password} onChange={update} autoComplete="current-password" /><div className="form-options"><label className="check-label"><input type="checkbox" name="remember" checked={form.remember} onChange={update} /> <span>Remember me</span></label><button type="button" onClick={() => toast.info('Password recovery will be available soon.')}>Forgot password?</button></div><button className="auth-submit" disabled={loading}>{loading ? <><span className="button-spinner"></span> Signing in…</> : 'Sign in'}</button></form><p className="auth-switch">New to LankaParts? <Link to="/auth/register">Create an account</Link></p></div>
}
