import React from 'react';
import { Button, Card, Form, Input, message, Typography } from 'antd';
import { Link, useNavigate } from 'react-router-dom';
import { authApi } from '../../api/authApi';
import { useAuthStore } from '../../stores/authStore';

const { Title } = Typography;

const LoginPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = React.useState(false);
  const navigate = useNavigate();
  const setAuth = useAuthStore((s) => s.setAuth);

  const onFinish = async (values: { email: string; password: string }) => {
    setLoading(true);
    try {
      const res = await authApi.login(values);
      if (res.status === 200) {
        const data = res.data;
        setAuth(data.token, {
          userId: data.userId,
          username: data.username,
          email: data.email,
          displayName: data.displayName,
          avatarUrl: data.avatarUrl,
          role: data.role,
        });
        navigate('/');
      } else {
        message.error('Invalid email or password.');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ minHeight: '100vh', display: 'flex', alignItems: 'center', justifyContent: 'center', background: '#f0f2f5' }}>
      <Card style={{ width: 400 }}>
        <Title level={3} style={{ textAlign: 'center', marginBottom: 24 }}>Sign In</Title>
        <Form form={form} layout="vertical" onFinish={onFinish}>
          <Form.Item name="email" label="Email" rules={[{ required: true, type: 'email' }]}>
            <Input placeholder="you@example.com" />
          </Form.Item>
          <Form.Item name="password" label="Password" rules={[{ required: true }]}>
            <Input.Password placeholder="Password" />
          </Form.Item>
          <Form.Item>
            <Button type="primary" htmlType="submit" loading={loading} block>Sign In</Button>
          </Form.Item>
        </Form>
        <div style={{ textAlign: 'center' }}>
          Don't have an account? <Link to="/register">Register</Link>
        </div>
      </Card>
    </div>
  );
};

export default LoginPage;
