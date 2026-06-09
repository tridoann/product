import apiClient from '../common';

export interface PostDto {
  id: number;
  authorId: number;
  authorUsername: string;
  authorDisplayName: string;
  authorAvatarUrl?: string;
  groupId?: number;
  content: string;
  mediaUrl?: string;
  mediaType: number;
  likeCount: number;
  commentCount: number;
  recentComments: CommentDto[];
  createdAt: string;
}

export interface CommentDto {
  id: number;
  authorId: number;
  authorDisplayName: string;
  authorAvatarUrl?: string;
  content: string;
  createdAt: string;
}

export interface FriendDto {
  userId: number;
  username: string;
  displayName: string;
  avatarUrl?: string;
}

export interface FriendRequestDto {
  id: number;
  senderId: number;
  senderUsername: string;
  senderDisplayName: string;
  senderAvatarUrl?: string;
  createdAt: string;
}

const socialApi = {
  getFeed: (pageIndex = 1, pageSize = 20) =>
    apiClient.get<{ items: PostDto[]; totalCount: number; pageIndex: number; pageSize: number }>(
      `/api/posts/feed?pageIndex=${pageIndex}&pageSize=${pageSize}`
    ),
  createPost: (data: { content: string; mediaUrl?: string; mediaType?: number; groupId?: number }) =>
    apiClient.post('/api/posts', data),
  deletePost: (id: number) => apiClient.delete(`/api/posts/${id}`),
  likePost: (id: number) => apiClient.post<{ liked: boolean }>(`/api/posts/${id}/like`),
  commentOnPost: (id: number, content: string) => apiClient.post(`/api/posts/${id}/comments`, { content }),

  getFriends: () => apiClient.get<{ friends: FriendDto[] }>('/api/friends'),
  getFriendRequests: () => apiClient.get<{ requests: FriendRequestDto[] }>('/api/friends/requests'),
  sendFriendRequest: (receiverId: number) => apiClient.post('/api/friends/request', { receiverId }),
  respondFriendRequest: (id: number, accept: boolean) =>
    apiClient.put(`/api/friends/request/${id}`, { accept }),
};

export default socialApi;
