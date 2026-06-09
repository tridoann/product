import apiClient from '../common';

export interface GroupDto {
  id: number;
  name: string;
  description?: string;
  avatarUrl?: string;
  privacy: number;
  memberCount: number;
  createdAt: string;
}

export interface GroupMemberDto {
  userId: number;
  username: string;
  displayName: string;
  avatarUrl?: string;
  role: number;
  joinedAt: string;
}

export interface GroupDetailDto extends GroupDto {
  isCurrentUserMember: boolean;
  currentUserRole?: number;
  members: GroupMemberDto[];
}

const groupsApi = {
  getGroups: (pageIndex = 1, pageSize = 20, search?: string) =>
    apiClient.get<{ items: GroupDto[]; totalCount: number }>(`/api/groups?pageIndex=${pageIndex}&pageSize=${pageSize}${search ? `&search=${search}` : ''}`),
  getGroup: (id: number) => apiClient.get<GroupDetailDto>(`/api/groups/${id}`),
  getGroupPosts: (id: number, pageIndex = 1) =>
    apiClient.get(`/api/groups/${id}/posts`, { params: { pageIndex } }),
  createGroup: (data: { name: string; description?: string; privacy: number }) =>
    apiClient.post('/api/groups', data),
  joinGroup: (id: number) => apiClient.post(`/api/groups/${id}/join`),
  leaveGroup: (id: number) => apiClient.delete(`/api/groups/${id}/leave`),
};

export default groupsApi;
