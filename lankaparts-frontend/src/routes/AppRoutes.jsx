import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom'
import AccountLayout from '../layouts/AccountLayout'
import AdminLayout from '../layouts/AdminLayout'
import AuthLayout from '../layouts/AuthLayout'
import MainLayout from '../layouts/MainLayout'
import SellerLayout from '../layouts/SellerLayout'
import AdminDashboardPage from '../pages/admin/DashboardPage'
import AdminCategoriesPage from '../pages/admin/CategoriesPage'
import AdminCommissionsPage from '../pages/admin/CommissionsPage'
import AdminOrdersPage from '../pages/admin/OrdersPage'
import AdminProductsPage from '../pages/admin/ProductsPage'
import AdminReportsPage from '../pages/admin/ReportsPage'
import AdminSellersPage from '../pages/admin/SellersPage'
import AdminSettlementsPage from '../pages/admin/SettlementsPage'
import AdminUsersPage from '../pages/admin/UsersPage'
import LoginPage from '../pages/auth/LoginPage'
import RegisterPage from '../pages/auth/RegisterPage'
import AccountPage from '../pages/customer/AccountPage'
import CartPage from '../pages/customer/CartPage'
import CheckoutPage from '../pages/customer/CheckoutPage'
import HomePage from '../pages/customer/HomePage'
import OrderDetailsPage from '../pages/customer/OrderDetailsPage'
import OrdersPage from '../pages/customer/OrdersPage'
import ProductDetailsPage from '../pages/customer/ProductDetailsPage'
import ProductsPage from '../pages/customer/ProductsPage'
import ProfilePage from '../pages/customer/ProfilePage'
import SellerDashboardPage from '../pages/seller/DashboardPage'
import SellerCompanyProfilePage from '../pages/seller/CompanyProfilePage'
import SellerEarningsPage from '../pages/seller/EarningsPage'
import SellerOrderDetailsPage from '../pages/seller/OrderDetailsPage'
import SellerOrdersPage from '../pages/seller/OrdersPage'
import SellerProductFormPage from '../pages/seller/ProductFormPage'
import SellerProductsPage from '../pages/seller/ProductsPage'
import SellerRegisterCompanyPage from '../pages/seller/RegisterCompanyPage'
import SellerSalesPage from '../pages/seller/SalesPage'
import ProtectedRoute from './ProtectedRoute'

export default function AppRoutes() { return <BrowserRouter><Routes>
  <Route element={<MainLayout />}>
    <Route index element={<HomePage />} /><Route path="products" element={<ProductsPage />} /><Route path="products/:id" element={<ProductDetailsPage />} /><Route path="cart" element={<CartPage />} />
    <Route element={<ProtectedRoute allowedRoles={['Customer']} />}><Route path="checkout" element={<CheckoutPage />} /><Route path="account" element={<AccountLayout />}><Route index element={<AccountPage />} /><Route path="orders" element={<OrdersPage />} /><Route path="orders/:id" element={<OrderDetailsPage />} /><Route path="profile" element={<ProfilePage />} /></Route></Route>
  </Route>
  <Route path="auth" element={<AuthLayout />}><Route index element={<Navigate to="login" replace />} /><Route path="login" element={<LoginPage />} /><Route path="register" element={<RegisterPage />} /></Route>
  <Route element={<ProtectedRoute allowedRoles={['Seller']} />}><Route path="seller" element={<SellerLayout />}><Route index element={<Navigate to="dashboard" replace />} /><Route path="register-company" element={<SellerRegisterCompanyPage />} /><Route path="dashboard" element={<SellerDashboardPage />} /><Route path="products" element={<SellerProductsPage />} /><Route path="products/new" element={<SellerProductFormPage />} /><Route path="products/:id/edit" element={<SellerProductFormPage />} /><Route path="orders" element={<SellerOrdersPage />} /><Route path="orders/:id" element={<SellerOrderDetailsPage />} /><Route path="sales" element={<SellerSalesPage />} /><Route path="earnings" element={<SellerEarningsPage />} /><Route path="company-profile" element={<SellerCompanyProfilePage />} /></Route></Route>
  <Route element={<ProtectedRoute allowedRoles={['Admin']} />}><Route path="admin" element={<AdminLayout />}><Route index element={<Navigate to="dashboard" replace />} /><Route path="dashboard" element={<AdminDashboardPage />} /><Route path="users" element={<AdminUsersPage />} /><Route path="sellers" element={<AdminSellersPage />} /><Route path="products" element={<AdminProductsPage />} /><Route path="categories" element={<AdminCategoriesPage />} /><Route path="orders" element={<AdminOrdersPage />} /><Route path="commissions" element={<AdminCommissionsPage />} /><Route path="settlements" element={<AdminSettlementsPage />} /><Route path="reports" element={<AdminReportsPage />} /></Route></Route>
  <Route path="*" element={<Navigate to="/" replace />} />
</Routes></BrowserRouter> }
