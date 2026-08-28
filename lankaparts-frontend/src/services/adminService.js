import api from './api'

const adminService = {
  async getDashboard() {
    const { data } = await api.get('/admin/dashboard')
    return data
  },
  async getUsers(params = {}) {
    const { data } = await api.get('/admin/users', { params })
    return data
  },
  async setUserActive(userId, active) {
    const action = active ? 'activate' : 'deactivate'
    const { data } = await api.patch(`/admin/users/${userId}/${action}`)
    return data
  },
  async getSellers(status) {
    const { data } = await api.get('/admin/seller-companies', { params: status ? { status } : undefined })
    return data
  },
  async approveSeller(companyId, note) {
    const { data } = await api.patch(`/admin/seller-companies/${companyId}/approve`, { note })
    return data
  },
  async rejectSeller(companyId, note) {
    const { data } = await api.patch(`/admin/seller-companies/${companyId}/reject`, { note })
    return data
  },
}

export default adminService
