import React, { useEffect, useState } from 'react';
import { Button, Input, Switch, Table, Tag, Typography } from 'antd';
import { SearchOutlined } from '@ant-design/icons';
import dayjs from 'dayjs';
import adminApi from '../../api/adminApi';

const { Title } = Typography;

interface UserRow {
  id: number;
  username: string;
  email: string;
  displayName: string;
  role: number;
  isActive: boolean;
  lastLoginAt?: string;
  createdAt: string;
}

const AdminUsersPage: React.FC = () => {
  const [users, setUsers] = useState<UserRow[]>([]);
  const [total, setTotal] = useState(0);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');

  const load = async (q = '') => {
    setLoading(true);
    const { data } = await adminApi.getUsers(1, q) as { data: { items: UserRow[]; totalCount: number } };
    setUsers(data.items);
    setTotal(data.totalCount);
    setLoading(false);
  };

  useEffect(() => { load(); }, []);

  const toggleActive = async (id: number, active: boolean) => {
    await adminApi.setUserActive(id, active);
    setUsers(prev => prev.map(u => u.id === id ? { ...u, isActive: active } : u));
  };

  const columns = [
    { title: 'Username', dataIndex: 'username', key: 'username' },
    { title: 'Email', dataIndex: 'email', key: 'email' },
    { title: 'Role', dataIndex: 'role', key: 'role', render: (v: number) => <Tag color={v === 1 ? 'gold' : 'default'}>{v === 1 ? 'Admin' : 'Member'}</Tag> },
    {
      title: 'Active', dataIndex: 'isActive', key: 'active',
      render: (v: boolean, r: UserRow) => <Switch checked={v} onChange={checked => toggleActive(r.id, checked)} />
    },
    { title: 'Last Login', dataIndex: 'lastLoginAt', key: 'login', render: (v?: string) => v ? dayjs(v).format('MMM D, YYYY') : '—' },
    { title: 'Joined', dataIndex: 'createdAt', key: 'joined', render: (v: string) => dayjs(v).format('MMM D, YYYY') },
  ];

  return (
    <div style={{ maxWidth: 1100, margin: '0 auto', padding: 24 }}>
      <Title level={4}>User Management</Title>
      <Input
        prefix={<SearchOutlined />}
        placeholder="Search users..."
        style={{ maxWidth: 300, marginBottom: 16 }}
        value={search}
        onChange={e => setSearch(e.target.value)}
        onPressEnter={() => load(search)}
        allowClear
        onClear={() => load()}
      />
      <Table dataSource={users} columns={columns} rowKey="id" loading={loading} pagination={{ total, pageSize: 50 }} />
    </div>
  );
};

export default AdminUsersPage;
