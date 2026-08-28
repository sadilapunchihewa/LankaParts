import { Navigate, Outlet, useLocation } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext'

const roleHome = { Customer: '/', Seller: '/seller/dashboard', Admin: '/admin/dashboard' }

export default function ProtectedRoute({ allowedRoles }) {
  const { user, isAuthenticated } = useAuth()
  const location = useLocation()
  if (!isAuthenticated) return <Navigate to="/auth/login" replace state={{ from: location }} />
  if (allowedRoles && !allowedRoles.includes(user.role)) return <Navigate to={roleHome[user.role] || '/'} replace />
  return <Outlet />
}
