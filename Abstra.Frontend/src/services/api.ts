import axios, { AxiosError } from 'axios';
import type { InternalAxiosRequestConfig } from 'axios';
import { API_BASE_URL, TOKEN_KEY } from '../utils/constants';

const api = axios.create({
  baseURL: API_BASE_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor to add JWT token
api.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    const token = localStorage.getItem(TOKEN_KEY);
    if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor for error handling
api.interceptors.response.use(
  (response) => response,
  (error: AxiosError) => {
    console.error('API Error:', {
      message: error.message,
      status: error.response?.status,
      data: error.response?.data,
      config: {
        url: error.config?.url,
        method: error.config?.method,
        baseURL: error.config?.baseURL,
      }
    });

    // Don't redirect to login if already on login page
    if (error.response?.status === 401 && !window.location.pathname.includes('/login')) {
      // Unauthorized - clear token and redirect to login
      localStorage.removeItem(TOKEN_KEY);
      localStorage.removeItem('abstra_user');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export default api;
