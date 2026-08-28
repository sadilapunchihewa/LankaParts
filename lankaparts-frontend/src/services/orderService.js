import api from './api'

const orderService = {
  async checkout(details) {
    const { data } = await api.post('/orders/checkout', details)
    return data
  },
  async getMine() {
    const { data } = await api.get('/orders')
    return data
  },
  async getById(orderId) {
    const { data } = await api.get(`/orders/${orderId}`)
    return data
  },
}

export default orderService
