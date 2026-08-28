import api from './api'

const authService = {
  async login(credentials) {
    const { data } = await api.post('/Auth/login', credentials)
    return data
  },
  async register(details) {
    const { data } = await api.post('/Auth/register', details)
    return data
  },
}

export default authService
