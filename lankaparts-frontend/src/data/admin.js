import { products } from './marketplace'
import { earningsHistory, sellerOrders, sellerProducts } from './seller'

export const adminUsers = [
  { id: 1, firstName: 'Admin', lastName: 'User', email: 'admin@lankaparts.lk', role: 'Admin', isActive: true, createdAt: '2026-06-01T08:00:00Z' },
  { id: 2, firstName: 'Kasun', lastName: 'Perera', email: 'kasun@example.com', role: 'Customer', isActive: true, createdAt: '2026-07-18T08:00:00Z' },
  { id: 3, firstName: 'Nippon', lastName: 'Parts', email: 'seller@example.com', role: 'Seller', isActive: true, createdAt: '2026-07-24T08:00:00Z' },
  { id: 4, firstName: 'Blocked', lastName: 'Buyer', email: 'blocked@example.com', role: 'Customer', isActive: false, createdAt: '2026-08-04T08:00:00Z' },
]

export const adminSellers = [
  { id: 10, companyName: 'AutoZone Lanka', sellerName: 'Nippon Parts', sellerEmail: 'seller@example.com', businessRegistrationNumber: 'BR-98421', createdAt: '2026-08-01T08:00:00Z', status: 'Pending', address: '12 Union Place', city: 'Colombo', phoneNumber: '+94 77 222 1100' },
  { id: 11, companyName: 'Hybrid Care Lanka', sellerName: 'Sahan Silva', sellerEmail: 'sahan@example.com', businessRegistrationNumber: 'BR-88410', createdAt: '2026-07-12T08:00:00Z', status: 'Approved', address: '45 High Level Road', city: 'Nugegoda', phoneNumber: '+94 71 456 2020' },
  { id: 12, companyName: 'Japan Auto Mart', sellerName: 'Ruwan Dias', sellerEmail: 'ruwan@example.com', businessRegistrationNumber: 'BR-77210', createdAt: '2026-08-18T08:00:00Z', status: 'Rejected', reviewNote: 'Business document is unreadable.', address: '22 Main Street', city: 'Kurunegala', phoneNumber: '+94 76 321 9090' },
  { id: 13, companyName: 'Suspended Spares', sellerName: 'Malan Jay', sellerEmail: 'malan@example.com', businessRegistrationNumber: 'BR-55421', createdAt: '2026-05-28T08:00:00Z', status: 'Suspended', address: '7 Lake Road', city: 'Kandy', phoneNumber: '+94 70 555 2121' },
]

export const adminProducts = [...sellerProducts, ...products.slice(0, 4).map((product) => ({ id: product.id, name: product.name, seller: product.seller, categoryName: product.category, price: product.price, stockQuantity: product.stock, approvalStatus: product.id % 2 ? 'Pending' : 'Approved', status: 'Active' }))]

export const adminOrders = sellerOrders.map((order, index) => ({ ...order, seller: ['AutoZone Lanka', 'Hybrid Care Lanka', 'Japan Auto Mart'][index] || 'AutoZone Lanka' }))

export const adminCommissionRows = adminSellers.filter((seller) => seller.status !== 'Rejected').map((seller, index) => {
  const grossSales = earningsHistory[index]?.grossSales || 180000
  return { seller: seller.companyName, grossSales, commissionRate: '3%', commission: grossSales * 0.03, sellerEarnings: grossSales * 0.97 }
})

export const adminSettlements = adminCommissionRows.map((row, index) => ({ ...row, month: ['August 2026', 'July 2026', 'June 2026'][index] || 'August 2026', status: index === 0 ? 'Pending' : 'Paid' }))

export const popularCategories = ['Brake Parts', 'Engine Parts', 'Lighting', 'Cooling System', 'Suspension']
export const monthlySales = [280000, 320000, 296000, 368000, 420000, 500000]
