import axios from 'axios';
import { useAuthStore } from '../stores/authStore';

declare global {
  interface Window {
    _env_?: {
      REACT_APP_API_URL?: string;
      [key: string]: any;
    };
  }
}

export const apiClient = axios.create({
  baseURL: window._env_?.REACT_APP_API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
  validateStatus: () => true,
});

apiClient.interceptors.request.use((config) => {
  const token = useAuthStore.getState().token;
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

apiClient.interceptors.response.use((response) => {
  if (response.status === 401) {
    useAuthStore.getState().clearAuth();
    window.location.href = '/login';
  }
  return response;
});

export default apiClient;