import api from './api'

const sellerService = {
  async getCompany() {
    const { data } = await api.get('/seller-companies/me')
    return data
  },
  async registerCompany(details) {
    const { data } = await api.post('/seller-companies', details)
    return data
  },
  async getProducts() {
    const { data } = await api.get('/seller/parts')
    return data
  },
  async createProduct(details) {
    const { data } = await api.post('/seller/parts', details)
    return data
  },
  async updateProduct(partId, details) {
    const { data } = await api.put(`/seller/parts/${partId}`, details)
    return data
  },
  async deleteProduct(partId) {
    await api.delete(`/seller/parts/${partId}`)
  },
  async getOrders(status) {
    const { data } = await api.get('/seller/orders', { params: status ? { status } : undefined })
    return data
  },
  async getOrder(orderId) {
    const { data } = await api.get(`/seller/orders/${orderId}`)
    return data
  },
  async updateOrderStatus(orderId, status) {
    const { data } = await api.patch(`/seller/orders/${orderId}/status`, { status })
    return data
  },
}

export default sellerService
