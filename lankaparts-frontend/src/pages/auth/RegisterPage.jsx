import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { Building2, Mail, Phone, UserRound } from 'lucide-react'
import { toast } from 'sonner'
import PasswordField from '../../components/common/PasswordField'
import { useAuth } from '../../contexts/AuthContext'

const initialForm = { firstName: '', lastName: '', email: '', phoneNumber: '', password: '', confirmPassword: '', role: 'Customer' }

export default function RegisterPage() {
  const { register } = useAuth(); const navigate = useNavigate(); const [form, setForm] = useState(initialForm); const [loading, setLoading] = useState(false)
  const update = ({ target }) => setForm((current) => ({ ...current, [target.name]: target.value }))
  const submit = async (event) => {
    event.preventDefault()
    if (form.password !== form.confirmPassword) return toast.error('Passwords do not match.')
    setLoading(true)
    try { await register({ firstName: form.firstName.trim(), lastName: form.lastName.trim(), email: form.email.trim(), phoneNumber: form.phoneNumber.trim(), password: form.password, role: form.role }); toast.success('Account created. You can now sign in.'); navigate('/auth/login', { replace: true }) }
    catch (error) { toast.error(error.response?.data?.message || 'Unable to create your account.') } finally { setLoading(false) }
  }
  return <div className="auth-card register-card"><div className="auth-heading"><span className="auth-kicker">JOIN THE MARKETPLACE</span><h2>Create your account</h2><p>Register as a customer or start your seller journey.</p></div><form className="auth-form" onSubmit={submit}><fieldset className="role-picker"><legend>I want to join as</legend><label className={form.role === 'Customer' ? 'selected' : ''}><input type="radio" name="role" value="Customer" checked={form.role === 'Customer'} onChange={update} /><UserRound /><span><strong>Customer</strong><small>Find and order vehicle parts</small></span></label><label className={form.role === 'Seller' ? 'selected' : ''}><input type="radio" name="role" value="Seller" checked={form.role === 'Seller'} onChange={update} /><Building2 /><span><strong>Seller</strong><small>List parts after approval</small></span></label></fieldset><div className="form-grid"><label className="form-field"><span>First name</span><div className="input-shell"><UserRound size={17} /><input name="firstName" value={form.firstName} onChange={update} autoComplete="given-name" placeholder="Kasun" required maxLength={100} /></div></label><label className="form-field"><span>Last name</span><div className="input-shell"><UserRound size={17} /><input name="lastName" value={form.lastName} onChange={update} autoComplete="family-name" placeholder="Perera" required maxLength={100} /></div></label></div><label className="form-field"><span>Email address</span><div className="input-shell"><Mail size={17} /><input type="email" name="email" value={form.email} onChange={update} autoComplete="email" placeholder="you@example.com" required /></div></label><label className="form-field"><span>Phone number</span><div className="input-shell"><Phone size={17} /><input type="tel" name="phoneNumber" value={form.phoneNumber} onChange={update} autoComplete="tel" placeholder="+94 77 123 4567" maxLength={20} /></div></label><div className="form-grid"><PasswordField label="Password" name="password" value={form.password} onChange={update} autoComplete="new-password" placeholder="Minimum 6 characters" /><PasswordField label="Confirm password" name="confirmPassword" value={form.confirmPassword} onChange={update} autoComplete="new-password" placeholder="Repeat password" /></div><button className="auth-submit" disabled={loading}>{loading ? <><span className="button-spinner"></span> Creating account…</> : 'Create account'}</button><p className="form-legal">By creating an account, you agree to LankaParts' Terms of Service and Privacy Policy.</p></form><p className="auth-switch">Already have an account? <Link to="/auth/login">Sign in</Link></p></div>
}
