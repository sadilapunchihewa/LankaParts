import api from './api'

const cartService = {
  async getCart() {
    const { data } = await api.get('/cart')
    return data
  },
  async addItem(details) {
    const { data } = await api.post('/cart/items', details)
    return data
  },
  async updateItem(itemId, details) {
    const { data } = await api.put(`/cart/items/${itemId}`, details)
    return data
  },
  async removeItem(itemId) {
    await api.delete(`/cart/items/${itemId}`)
  },
}

export default cartService
