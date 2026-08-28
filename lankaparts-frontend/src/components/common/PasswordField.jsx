import { Eye, EyeOff, LockKeyhole } from 'lucide-react'
import { useState } from 'react'

export default function PasswordField({ label, name, value, onChange, autoComplete, placeholder = 'Enter your password' }) {
  const [visible, setVisible] = useState(false)
  return <label className="form-field"><span>{label}</span><div className="input-shell"><LockKeyhole size={17} /><input type={visible ? 'text' : 'password'} name={name} value={value} onChange={onChange} autoComplete={autoComplete} placeholder={placeholder} required minLength={6} /><button type="button" onClick={() => setVisible(!visible)} aria-label={visible ? 'Hide password' : 'Show password'}>{visible ? <EyeOff size={17} /> : <Eye size={17} />}</button></div></label>
}
