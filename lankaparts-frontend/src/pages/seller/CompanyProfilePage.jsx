import { Link } from 'react-router-dom'
import { Building2, FileCheck2 } from 'lucide-react'

export default function CompanyProfilePage() {
  return <section className="seller-page"><div className="seller-page-heading split"><div><span>Company Profile</span><h1>Company profile</h1><p>Keep your seller company details and verification records ready for LankaParts review.</p></div><Link className="seller-primary-action" to="/seller/register-company"><FileCheck2 /> Verification</Link></div><section className="verification-card neutral"><Building2 /><div><span>Seller Verification</span><h2>Status: Pending</h2><p>Your business registration is currently being reviewed by LankaParts.</p></div></section></section>
}
