import { Card, Col, Row, Spin, Statistic, Typography } from 'antd';
import {
  UserOutlined, ShoppingOutlined, CustomerServiceOutlined, ShoppingCartOutlined,
} from '@ant-design/icons';
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import adminApi from '../../api/adminApi';

const { Title } = Typography;

interface Stats {
  totalUsers: number;
  activeUsers: number;
  totalOrders: number;
  openTickets: number;
  totalProducts: number;
}

const AdminDashboardPage: React.FC = () => {
  const navigate = useNavigate();
  const [stats, setStats] = useState<Stats | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    adminApi.getStats()
      .then((r) => setStats(r.data))
      .finally(() => setLoading(false));
  }, []);

  if (loading) {
    return (
      <div style={{ textAlign: 'center', padding: 80 }}>
        <Spin size="large" />
      </div>
    );
  }

  return (
    <div style={{ maxWidth: 1000, margin: '0 auto', padding: 24 }}>
      <Title level={3}>Admin Dashboard</Title>

      <Row gutter={[16, 16]} style={{ marginBottom: 32 }}>
        <Col xs={24} sm={12} lg={6}>
          <Card
            hoverable
            onClick={() => navigate('/admin/users')}
            style={{ cursor: 'pointer' }}
          >
            <Statistic
              title="Total Users"
              value={stats?.totalUsers ?? 0}
              prefix={<UserOutlined />}
            />
            <div style={{ marginTop: 4, fontSize: 12, color: '#52c41a' }}>
              {stats?.activeUsers ?? 0} active
            </div>
          </Card>
        </Col>

        <Col xs={24} sm={12} lg={6}>
          <Card
            hoverable
            onClick={() => navigate('/admin/orders')}
            style={{ cursor: 'pointer' }}
          >
            <Statistic
              title="Total Orders"
              value={stats?.totalOrders ?? 0}
              prefix={<ShoppingCartOutlined />}
            />
          </Card>
        </Col>

        <Col xs={24} sm={12} lg={6}>
          <Card
            hoverable
            onClick={() => navigate('/admin/tickets')}
            style={{ cursor: 'pointer' }}
          >
            <Statistic
              title="Open Tickets"
              value={stats?.openTickets ?? 0}
              prefix={<CustomerServiceOutlined />}
              valueStyle={stats?.openTickets ? { color: '#ff4d4f' } : undefined}
            />
          </Card>
        </Col>

        <Col xs={24} sm={12} lg={6}>
          <Card
            hoverable
            onClick={() => navigate('/products')}
            style={{ cursor: 'pointer' }}
          >
            <Statistic
              title="Active Products"
              value={stats?.totalProducts ?? 0}
              prefix={<ShoppingOutlined />}
            />
          </Card>
        </Col>
      </Row>

      <Row gutter={[16, 16]}>
        <Col span={8}>
          <Card
            title="Quick Links"
            size="small"
          >
            <div style={{ display: 'flex', flexDirection: 'column', gap: 8 }}>
              <a onClick={() => navigate('/admin/users')}>Manage Users</a>
              <a onClick={() => navigate('/admin/orders')}>Manage Orders</a>
              <a onClick={() => navigate('/admin/tickets')}>Support Tickets</a>
            </div>
          </Card>
        </Col>
      </Row>
    </div>
  );
};

export default AdminDashboardPage;
