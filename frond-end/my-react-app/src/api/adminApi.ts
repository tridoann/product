import apiClient from '../common';

export interface AdminStatsDto {
  totalUsers: number;
  activeUsers: number;
  totalOrders: number;
  openTickets: number;
  totalProducts: number;
}

const adminApi = {
  getStats: () => apiClient.get<AdminStatsDto>('/api/admin/stats'),
  getUsers: (pageIndex = 1, search?: string) =>
    apiClient.get(`/api/admin/users?pageIndex=${pageIndex}${search ? `&search=${search}` : ''}`),
  setUserActive: (id: number, isActive: boolean) =>
    apiClient.put(`/api/admin/users/${id}/active`, { isActive }),
  getTickets: (pageIndex = 1) =>
    apiClient.get(`/api/admin/tickets?pageIndex=${pageIndex}`),
  getOrders: (pageIndex = 1) =>
    apiClient.get(`/api/admin/orders?pageIndex=${pageIndex}`),
  updateOrderStatus: (id: number, status: number) =>
    apiClient.put(`/api/orders/${id}/status`, { status }),
};

export default adminApi;
