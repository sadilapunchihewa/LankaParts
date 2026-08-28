import api from './api'

const productService = {
  async browse(params = {}) {
    const { data } = await api.get('/parts', { params })
    return data
  },
  async getById(partId) {
    const { data } = await api.get(`/parts/${partId}`)
    return data
  },
  async getCategories() {
    const { data } = await api.get('/parts/categories')
    return data
  },
}

export default productService
