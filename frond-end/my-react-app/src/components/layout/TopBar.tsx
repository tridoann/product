import React from 'react';
import { Avatar, Button, Dropdown, Layout, Space, Typography } from 'antd';
import type { MenuProps } from 'antd';
import { UserOutlined, LogoutOutlined, SettingOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import { useAuthStore } from '../../stores/authStore';

const { Header } = Layout;
const { Text } = Typography;

const TopBar: React.FC = () => {
  const navigate = useNavigate();
  const { user, clearAuth } = useAuthStore();

  const menuItems: MenuProps['items'] = [
    {
      key: 'profile',
      icon: <UserOutlined />,
      label: 'My Profile',
      onClick: () => navigate(`/profile/${user?.userId}`),
    },
    {
      key: 'settings',
      icon: <SettingOutlined />,
      label: 'Settings',
      onClick: () => navigate('/settings'),
    },
    { type: 'divider' },
    {
      key: 'logout',
      icon: <LogoutOutlined />,
      label: 'Sign Out',
      onClick: () => {
        clearAuth();
        navigate('/login');
      },
    },
  ];

  return (
    <Header style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', padding: '0 24px', background: '#fff', borderBottom: '1px solid #f0f0f0' }}>
      <Text strong style={{ fontSize: 18, cursor: 'pointer' }} onClick={() => navigate('/')}>
        SocialBuy
      </Text>
      <Space>
        <Dropdown menu={{ items: menuItems }} placement="bottomRight">
          <Space style={{ cursor: 'pointer' }}>
            <Avatar src={user?.avatarUrl} icon={<UserOutlined />} />
            <Text>{user?.displayName}</Text>
          </Space>
        </Dropdown>
      </Space>
    </Header>
  );
};

export default TopBar;
