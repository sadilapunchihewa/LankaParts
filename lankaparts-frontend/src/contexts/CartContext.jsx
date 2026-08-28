/* oxlint-disable react/only-export-components */
import { createContext, useContext, useMemo, useState } from 'react'
import { products } from '../data/marketplace'

const CartContext = createContext(null)
const initialItems = [{ ...products[0], quantity: 1 }, { ...products[3], quantity: 1 }]

export function CartProvider({ children }) {
  const [items, setItems] = useState(initialItems)
  const addItem = (product, quantity = 1) => setItems((current) => { const found = current.find((item) => item.id === product.id); return found ? current.map((item) => item.id === product.id ? { ...item, quantity: Math.min(item.quantity + quantity, item.stock) } : item) : [...current, { ...product, quantity }] })
  const updateQuantity = (id, quantity) => setItems((current) => current.map((item) => item.id === id ? { ...item, quantity: Math.max(1, Math.min(quantity, item.stock)) } : item))
  const removeItem = (id) => setItems((current) => current.filter((item) => item.id !== id))
  const clearCart = () => setItems([])
  const value = useMemo(() => ({ items, itemCount: items.reduce((sum, item) => sum + item.quantity, 0), subtotal: items.reduce((sum, item) => sum + item.price * item.quantity, 0), addItem, updateQuantity, removeItem, clearCart }), [items])
  return <CartContext.Provider value={value}>{children}</CartContext.Provider>
}
export const useCart = () => useContext(CartContext)
