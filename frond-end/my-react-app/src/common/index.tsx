import axios from 'axios';

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
    validateStatus: (status) => true
});