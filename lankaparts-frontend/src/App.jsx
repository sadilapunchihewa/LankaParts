import { Toaster } from 'sonner'
import { AuthProvider } from './contexts/AuthContext'
import { CartProvider } from './contexts/CartContext'
import AppRoutes from './routes/AppRoutes'
import './App.css'
export default function App() { return <AuthProvider><CartProvider><AppRoutes /><Toaster position="top-right" richColors closeButton /></CartProvider></AuthProvider> }
