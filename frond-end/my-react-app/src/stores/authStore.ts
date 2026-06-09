import { create } from 'zustand';
import { persist } from 'zustand/middleware';

export interface AuthUser {
  userId: number;
  username: string;
  email: string;
  displayName: string;
  avatarUrl?: string;
  bio?: string;
  role: string;
}

interface AuthState {
  token: string | null;
  user: AuthUser | null;
  setAuth: (token: string, user: AuthUser) => void;
  updateUser: (partial: Partial<AuthUser>) => void;
  clearAuth: () => void;
  isAuthenticated: () => boolean;
  isAdmin: () => boolean;
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set, get) => ({
      token: null,
      user: null,
      setAuth: (token, user) => set({ token, user }),
      updateUser: (partial) => set((s) => ({ user: s.user ? { ...s.user, ...partial } : s.user })),
      clearAuth: () => set({ token: null, user: null }),
      isAuthenticated: () => !!get().token,
      isAdmin: () => get().user?.role === 'Admin',
    }),
    { name: 'auth-storage' }
  )
);
