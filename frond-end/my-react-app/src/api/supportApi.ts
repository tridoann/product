import apiClient from '../common';

export interface TicketSummaryDto {
  id: number;
  subject: string;
  status: number;
  priority: number;
  submittedByUsername: string;
  createdAt: string;
}

export interface TicketReplyDto {
  id: number;
  authorId: number;
  authorUsername: string;
  authorDisplayName: string;
  content: string;
  isAdminReply: boolean;
  createdAt: string;
}

export interface TicketDetailDto {
  id: number;
  subject: string;
  description: string;
  status: number;
  priority: number;
  submittedById: number;
  submittedByUsername: string;
  createdAt: string;
  resolvedAt?: string;
  replies: TicketReplyDto[];
}

const supportApi = {
  getTickets: (pageIndex = 1) => apiClient.get<{ items: TicketSummaryDto[]; totalCount: number }>(`/api/support/tickets?pageIndex=${pageIndex}`),
  getTicket: (id: number) => apiClient.get<TicketDetailDto>(`/api/support/tickets/${id}`),
  createTicket: (data: { subject: string; description: string; priority: number }) =>
    apiClient.post('/api/support/tickets', data),
  replyToTicket: (id: number, content: string) =>
    apiClient.post(`/api/support/tickets/${id}/replies`, { content }),
  updateStatus: (id: number, status: number) =>
    apiClient.put(`/api/support/tickets/${id}/status`, { status }),
};

export default supportApi;
