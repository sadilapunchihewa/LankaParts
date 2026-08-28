/* oxlint-disable react/set-state-in-effect -- These effects synchronize API-backed page state. */
import { useEffect, useState } from 'react'
import { Building2, CheckCircle2, Clock3, FileCheck2, ImageUp, Mail, MapPin, Phone, XCircle } from 'lucide-react'
import { toast } from 'sonner'
import sellerService from '../../services/sellerService'
import { ErrorState, LoadingState } from '../../components/common/AdminUi'

const initialForm = { companyName: '', businessRegistrationNumber: '', phoneNumber: '', email: '', address: '', city: '', description: '', logoUrl: '', taxIdentificationNumber: '', ownerNic: '', registrationDocumentUrl: '' }
const statusCopy = { Pending: 'Your business registration is currently being reviewed by LankaParts.', Approved: 'Your seller company is approved. You can publish approved products in the marketplace.', Rejected: 'Your seller company needs changes before it can be approved.' }
const statusIcons = { Pending: Clock3, Approved: CheckCircle2, Rejected: XCircle }

export default function RegisterCompanyPage() {
  const [form, setForm] = useState(initialForm)
  const [company, setCompany] = useState(null)
  const [loading, setLoading] = useState(false)
  const [checking, setChecking] = useState(true)

  const [error, setError] = useState('')

  const loadCompany = async () => {
    setChecking(true); setError('')
    try { setCompany(await sellerService.getCompany()) }
    catch (loadError) { if (loadError.response?.status !== 404) setError('Unable to load seller verification.'); setCompany(null) }
    finally { setChecking(false) }
  }

  useEffect(() => { loadCompany() }, [])

  const update = ({ target }) => setForm((current) => ({ ...current, [target.name]: target.value }))
  const submit = async (event) => {
    event.preventDefault()
    setLoading(true)
    try {
      const data = await sellerService.registerCompany({ companyName: form.companyName.trim(), businessRegistrationNumber: form.businessRegistrationNumber.trim(), phoneNumber: form.phoneNumber.trim(), address: form.address.trim(), city: form.city.trim() })
      setCompany({ ...data, status: data.status || 'Pending', email: form.email, description: form.description, logoUrl: form.logoUrl, taxIdentificationNumber: form.taxIdentificationNumber, ownerNic: form.ownerNic, registrationDocumentUrl: form.registrationDocumentUrl })
      toast.success('Company registration submitted for review.')
    } catch (error) {
      toast.error(error.response?.data?.message || 'Unable to submit seller registration.')
    } finally {
      setLoading(false)
    }
  }

  const status = company?.status || 'Pending'
  const StatusIcon = statusIcons[status] || Clock3

  if (checking) return <section className="seller-page"><LoadingState message="Loading seller verification..." /></section>
  if (error) return <section className="seller-page"><ErrorState message={error} onRetry={loadCompany} /></section>
  if (company) return <section className="seller-page"><div className="seller-page-heading"><span>Seller Verification</span><h1>Seller Verification</h1><p>LankaParts reviews company details before marketplace selling is enabled.</p></div><div className="verification-card"><StatusIcon /><div><span>Status: {status}</span><h2>{status}</h2><p>{statusCopy[status] || statusCopy.Pending}</p>{company.reviewNote && <strong>Review note: {company.reviewNote}</strong>}</div></div><div className="status-track">{['Pending', 'Approved', 'Rejected'].map((item) => <span key={item} className={item === status ? 'active' : ''}>{item}</span>)}</div></section>

  return <section className="seller-page"><div className="seller-page-heading"><span>Seller Registration</span><h1>Register your company</h1><p>Submit your business details and verification references for LankaParts review.</p></div><form className="seller-form" onSubmit={submit}><div className="form-grid"><label className="form-field"><span>Company Name</span><div className="input-shell"><Building2 size={17} /><input name="companyName" value={form.companyName} onChange={update} required maxLength={150} /></div></label><label className="form-field"><span>Business Registration Number</span><div className="input-shell"><FileCheck2 size={17} /><input name="businessRegistrationNumber" value={form.businessRegistrationNumber} onChange={update} required maxLength={50} /></div></label></div><div className="form-grid"><label className="form-field"><span>Phone</span><div className="input-shell"><Phone size={17} /><input name="phoneNumber" value={form.phoneNumber} onChange={update} type="tel" maxLength={20} /></div></label><label className="form-field"><span>Email</span><div className="input-shell"><Mail size={17} /><input name="email" value={form.email} onChange={update} type="email" /></div></label></div><label className="form-field"><span>Address</span><div className="input-shell"><MapPin size={17} /><input name="address" value={form.address} onChange={update} required maxLength={250} /></div></label><div className="form-grid"><label className="form-field"><span>City</span><div className="input-shell"><MapPin size={17} /><input name="city" value={form.city} onChange={update} required maxLength={100} /></div></label><label className="form-field"><span>Logo URL</span><div className="input-shell"><ImageUp size={17} /><input name="logoUrl" value={form.logoUrl} onChange={update} placeholder="https://..." /></div></label></div><label className="form-field"><span>Description</span><textarea name="description" value={form.description} onChange={update} rows="4" placeholder="Tell buyers about your business, inventory, and service area." /></label><div className="form-grid three"><label className="form-field"><span>Tax Identification Number</span><div className="input-shell"><input name="taxIdentificationNumber" value={form.taxIdentificationNumber} onChange={update} /></div></label><label className="form-field"><span>Owner NIC / Passport</span><div className="input-shell"><input name="ownerNic" value={form.ownerNic} onChange={update} /></div></label><label className="form-field"><span>Registration Document URL</span><div className="input-shell"><input name="registrationDocumentUrl" value={form.registrationDocumentUrl} onChange={update} placeholder="https://..." /></div></label></div><button className="auth-submit" disabled={loading}>{loading ? 'Submitting...' : 'Submit for verification'}</button></form></section>
}
