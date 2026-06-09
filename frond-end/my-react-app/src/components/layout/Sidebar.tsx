import React from 'react';
import { Layout, Menu } from 'antd';
import {
  HomeOutlined,
  TeamOutlined,
  MessageOutlined,
  ShoppingOutlined,
  CustomerServiceOutlined,
  DashboardOutlined,
  UserOutlined,
} from '@ant-design/icons';
import { useNavigate, useLocation } from 'react-router-dom';
import { useAuthStore } from '../../stores/authStore';

const { Sider } = Layout;

const Sidebar: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const isAdmin = useAuthStore((s) => s.isAdmin());

  const items = [
    { key: '/', icon: <HomeOutlined />, label: 'Feed' },
    { key: '/friends', icon: <UserOutlined />, label: 'Friends' },
    { key: '/groups', icon: <TeamOutlined />, label: 'Groups' },
    { key: '/messages', icon: <MessageOutlined />, label: 'Messages' },
    { key: '/products', icon: <ShoppingOutlined />, label: 'Shop' },
    { key: '/support', icon: <CustomerServiceOutlined />, label: 'Support' },
    ...(isAdmin ? [{ key: '/admin', icon: <DashboardOutlined />, label: 'Admin' }] : []),
  ];

  return (
    <Sider width={200} style={{ background: '#fff', borderRight: '1px solid #f0f0f0' }}>
      <Menu
        mode="inline"
        selectedKeys={[location.pathname]}
        style={{ height: '100%', borderRight: 0 }}
        items={items}
        onClick={({ key }) => navigate(key)}
      />
    </Sider>
  );
};

export default Sidebar;
