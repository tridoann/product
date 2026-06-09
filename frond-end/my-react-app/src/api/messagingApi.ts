import apiClient from '../common';

export interface ConversationDto {
  id: number;
  type: number;
  title?: string;
  participants: ParticipantDto[];
  lastMessageContent?: string;
  lastMessageAt?: string;
  unreadCount: number;
}

export interface ParticipantDto {
  userId: number;
  displayName: string;
  avatarUrl?: string;
  lastReadAt?: string;
}

export interface MessageDto {
  id: number;
  senderId: number;
  senderDisplayName: string;
  senderAvatarUrl?: string;
  content: string;
  mediaUrl?: string;
  createdAt: string;
}

const messagingApi = {
  getConversations: () =>
    apiClient.get<{ items: ConversationDto[] }>('/api/conversations'),
  getOrCreateDirect: (otherUserId: number) =>
    apiClient.post<{ conversationId: number }>('/api/conversations/direct', { otherUserId }),
  getMessages: (id: number, pageIndex = 1, pageSize = 50) =>
    apiClient.get<{ items: MessageDto[]; totalCount: number }>(`/api/conversations/${id}/messages?pageIndex=${pageIndex}&pageSize=${pageSize}`),
  sendMessage: (conversationId: number, content: string) =>
    apiClient.post<MessageDto>(`/api/conversations/${conversationId}/messages`, { content }),
  markRead: (id: number) => apiClient.put(`/api/conversations/${id}/read`),
};

export default messagingApi;
