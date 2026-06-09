import { apiClient } from '../common';

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
  displayName: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  expiresAt: string;
  userId: number;
  username: string;
  email: string;
  displayName: string;
  avatarUrl?: string;
  role: string;
}

export interface ProfileResponse {
  id: number;
  username: string;
  email: string;
  displayName: string;
  avatarUrl?: string;
  bio?: string;
  role: string;
  createdAt: string;
}

export interface PublicUserResponse {
  id: number;
  username: string;
  displayName: string;
  avatarUrl?: string;
  bio?: string;
  createdAt: string;
}

export const authApi = {
  register: (data: RegisterRequest) =>
    apiClient.post<{ userId: number; token: string; username: string; email: string }>('/api/auth/register', data),

  login: (data: LoginRequest) =>
    apiClient.post<LoginResponse>('/api/auth/login', data),

  getMe: () =>
    apiClient.get<ProfileResponse>('/api/auth/me'),

  getUserById: (id: number) =>
    apiClient.get<PublicUserResponse>(`/api/users/${id}`),

  getUserPosts: (id: number, pageIndex = 1, pageSize = 12) =>
    apiClient.get(`/api/users/${id}/posts`, { params: { pageIndex, pageSize } }),

  updateProfile: (data: { displayName: string; bio?: string; avatarUrl?: string }) =>
    apiClient.put('/api/auth/profile', data),

  changePassword: (data: { currentPassword: string; newPassword: string }) =>
    apiClient.put('/api/auth/password', data),
};
