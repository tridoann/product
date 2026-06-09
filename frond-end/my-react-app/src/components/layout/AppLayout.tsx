import React from 'react';
import { Layout } from 'antd';
import { Outlet } from 'react-router-dom';
import TopBar from './TopBar';
import Sidebar from './Sidebar';

const { Content } = Layout;

const AppLayout: React.FC = () => (
  <Layout style={{ minHeight: '100vh' }}>
    <TopBar />
    <Layout>
      <Sidebar />
      <Layout style={{ padding: '24px' }}>
        <Content style={{ background: '#fff', padding: 24, minHeight: 280, borderRadius: 8 }}>
          <Outlet />
        </Content>
      </Layout>
    </Layout>
  </Layout>
);

export default AppLayout;
