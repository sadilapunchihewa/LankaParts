import { useEffect, useState } from 'react'
import { Plus, Save, Trash2 } from 'lucide-react'
import { useNavigate, useParams } from 'react-router-dom'
import { toast } from 'sonner'
import sellerService from '../../services/sellerService'
import { categories, products as catalogProducts } from '../../data/marketplace'
import { sellerProducts } from '../../data/seller'
import { ErrorState, LoadingState } from '../../components/common/AdminUi'

const emptyVehicle = { vehicleMake: '', vehicleModel: '', yearFrom: '', yearTo: '' }
const initialForm = { name: '', categoryId: '1', brand: '', partNumber: '', description: '', price: '', stockQuantity: '', condition: 'New', imageUrl: '', vehicles: [{ ...emptyVehicle }] }
const productToForm = (product) => ({ name: product.name || '', categoryId: String(product.categoryId || 1), brand: product.brand || '', partNumber: product.partNumber || product.oem || '', description: product.description || '', price: String(product.price || ''), stockQuantity: String(product.stockQuantity ?? product.stock ?? ''), condition: product.condition || 'New', imageUrl: product.imageUrl || '', vehicles: [{ vehicleMake: product.vehicleMake || product.brand || '', vehicleModel: product.vehicleModel || '', yearFrom: product.yearFrom || '', yearTo: product.yearTo || '' }] })
const localProductForm = (id) => {
  const product = sellerProducts.find((item) => String(item.id) === id) || catalogProducts.find((item) => String(item.id) === id)
  return product ? productToForm(product) : initialForm
}

export default function ProductFormPage() {
  const { id } = useParams()
  const navigate = useNavigate()
  const editing = Boolean(id)
  const [form, setForm] = useState(() => editing ? localProductForm(id) : initialForm)
  const [loading, setLoading] = useState(false)
  const [checking, setChecking] = useState(editing)
  const [error, setError] = useState('')

  useEffect(() => {
    if (!editing) return
    const loadProduct = async () => {
      setChecking(true); setError('')
      try {
        const match = (await sellerService.getProducts()).find((item) => String(item.id) === id)
        if (match) setForm(productToForm(match))
      } catch {
        setError('Unable to load product details. Using saved draft data if available.')
      } finally {
        setChecking(false)
      }
    }
    loadProduct()
  }, [editing, id])

  const update = ({ target }) => setForm((current) => ({ ...current, [target.name]: target.value }))
  const updateVehicle = (index, field, value) => setForm((current) => ({ ...current, vehicles: current.vehicles.map((vehicle, vehicleIndex) => vehicleIndex === index ? { ...vehicle, [field]: value } : vehicle) }))
  const addVehicle = () => setForm((current) => ({ ...current, vehicles: [...current.vehicles, { ...emptyVehicle }] }))
  const removeVehicle = (index) => setForm((current) => ({ ...current, vehicles: current.vehicles.filter((_, vehicleIndex) => vehicleIndex !== index) }))
  const submit = async (event) => {
    event.preventDefault()
    const firstVehicle = form.vehicles.find((vehicle) => vehicle.vehicleMake || vehicle.vehicleModel) || form.vehicles[0] || emptyVehicle
    const payload = { name: form.name.trim(), partNumber: form.partNumber.trim(), categoryId: Number(form.categoryId), brand: form.brand.trim(), description: `${form.description.trim()}\n\nCondition: ${form.condition}`.trim(), vehicleMake: firstVehicle.vehicleMake.trim(), vehicleModel: firstVehicle.vehicleModel.trim(), yearFrom: firstVehicle.yearFrom ? Number(firstVehicle.yearFrom) : null, yearTo: firstVehicle.yearTo ? Number(firstVehicle.yearTo) : null, price: Number(form.price), stockQuantity: Number(form.stockQuantity), imageUrl: form.imageUrl.trim() || null }
    setLoading(true)
    try {
      if (editing) await sellerService.updateProduct(id, payload)
      else await sellerService.createProduct(payload)
      toast.success(editing ? 'Product updated.' : 'Product submitted for approval.')
      navigate('/seller/products')
    }
    catch (error) { toast.error(error.response?.data?.message || 'Unable to save product.') }
    finally { setLoading(false) }
  }

  if (checking) return <section className="seller-page"><LoadingState message="Loading product details..." /></section>
  return <section className="seller-page"><div className="seller-page-heading"><span>{editing ? 'Edit Product' : 'Add Product'}</span><h1>{editing ? 'Edit spare part' : 'Add spare part'}</h1><p>New and edited products enter marketplace approval before buyers can purchase them.</p></div>{error && <ErrorState message={error} />}<form className="seller-form" onSubmit={submit}><div className="form-grid"><label className="form-field"><span>Product Name</span><div className="input-shell"><input name="name" value={form.name} onChange={update} required maxLength={150} /></div></label><label className="form-field"><span>Category</span><div className="input-shell"><select name="categoryId" value={form.categoryId} onChange={update}>{categories.map((category, index) => <option key={category.name} value={index + 1}>{category.name}</option>)}</select></div></label></div><div className="form-grid"><label className="form-field"><span>Brand</span><div className="input-shell"><input name="brand" value={form.brand} onChange={update} /></div></label><label className="form-field"><span>OEM Number</span><div className="input-shell"><input name="partNumber" value={form.partNumber} onChange={update} required maxLength={80} /></div></label></div><label className="form-field"><span>Description</span><textarea name="description" value={form.description} onChange={update} rows="5" required /></label><div className="form-grid three"><label className="form-field"><span>Price</span><div className="input-shell"><input name="price" value={form.price} onChange={update} type="number" min="1" step="0.01" required /></div></label><label className="form-field"><span>Stock Quantity</span><div className="input-shell"><input name="stockQuantity" value={form.stockQuantity} onChange={update} type="number" min="0" required /></div></label><label className="form-field"><span>Condition</span><div className="input-shell"><select name="condition" value={form.condition} onChange={update}><option>New</option><option>Used</option><option>Reconditioned</option></select></div></label></div><label className="form-field"><span>Images</span><div className="input-shell"><input name="imageUrl" value={form.imageUrl} onChange={update} placeholder="Image URL" /></div></label><fieldset className="compatibility-editor"><legend>Compatible Vehicles</legend>{form.vehicles.map((vehicle, index) => <div className="compatibility-row" key={`${index}-${vehicle.vehicleMake}`}><input value={vehicle.vehicleMake} onChange={(event) => updateVehicle(index, 'vehicleMake', event.target.value)} placeholder="Vehicle Brand" /><input value={vehicle.vehicleModel} onChange={(event) => updateVehicle(index, 'vehicleModel', event.target.value)} placeholder="Vehicle Model" /><input value={vehicle.yearFrom} onChange={(event) => updateVehicle(index, 'yearFrom', event.target.value)} type="number" min="1900" max="2200" placeholder="Year From" /><input value={vehicle.yearTo} onChange={(event) => updateVehicle(index, 'yearTo', event.target.value)} type="number" min="1900" max="2200" placeholder="Year To" /><button type="button" onClick={() => removeVehicle(index)} disabled={form.vehicles.length === 1}><Trash2 /> Remove</button></div>)}<button className="add-vehicle" type="button" onClick={addVehicle}><Plus /> Add Vehicle</button></fieldset><button className="auth-submit" disabled={loading}><Save size={16} /> {loading ? 'Saving...' : editing ? 'Update product' : 'Submit product'}</button></form></section>
}
