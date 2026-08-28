/* oxlint-disable react/only-export-components -- Provider and its consumer hook intentionally share this module. */
import { createContext, useCallback, useContext, useEffect, useMemo, useState } from 'react'
import authService from '../services/authService'
import { AUTH_STORAGE_KEY, clearStoredAuth, getStoredAuth } from '../services/api'

const AuthContext = createContext(null)

export function AuthProvider({ children }) {
  const [user, setUser] = useState(() => getStoredAuth())

  const logout = useCallback(() => {
    clearStoredAuth()
    setUser(null)
  }, [])

  useEffect(() => {
    window.addEventListener('lankaparts:unauthorized', logout)
    return () => window.removeEventListener('lankaparts:unauthorized', logout)
  }, [logout])

  const login = useCallback(async (credentials, remember = false) => {
    const authenticatedUser = await authService.login(credentials)
    clearStoredAuth()
    const storage = remember ? localStorage : sessionStorage
    storage.setItem(AUTH_STORAGE_KEY, JSON.stringify(authenticatedUser))
    setUser(authenticatedUser)
    return authenticatedUser
  }, [])

  const register = useCallback((details) => authService.register(details), [])
  const value = useMemo(() => ({ user, isAuthenticated: Boolean(user?.token), login, register, logout }), [user, login, register, logout])
  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}

export function useAuth() {
  const context = useContext(AuthContext)
  if (!context) throw new Error('useAuth must be used inside AuthProvider')
  return context
}
