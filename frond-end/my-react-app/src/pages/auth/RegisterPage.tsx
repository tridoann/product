import React from 'react';
import { Button, Card, Form, Input, message, Typography } from 'antd';
import { Link, useNavigate } from 'react-router-dom';
import { authApi } from '../../api/authApi';
import { useAuthStore } from '../../stores/authStore';

const { Title } = Typography;

const RegisterPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = React.useState(false);
  const navigate = useNavigate();
  const setAuth = useAuthStore((s) => s.setAuth);

  const onFinish = async (values: { username: string; email: string; password: string; displayName: string }) => {
    setLoading(true);
    try {
      const res = await authApi.register(values);
      if (res.status === 200) {
        // After register, log in immediately
        const loginRes = await authApi.login({ email: values.email, password: values.password });
        if (loginRes.status === 200) {
          const data = loginRes.data;
          setAuth(data.token, {
            userId: data.userId,
            username: data.username,
            email: data.email,
            displayName: data.displayName,
            avatarUrl: data.avatarUrl,
            role: data.role,
          });
          navigate('/');
        }
      } else {
        message.error('Registration failed. Please try again.');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ minHeight: '100vh', display: 'flex', alignItems: 'center', justifyContent: 'center', background: '#f0f2f5' }}>
      <Card style={{ width: 440 }}>
        <Title level={3} style={{ textAlign: 'center', marginBottom: 24 }}>Create Account</Title>
        <Form form={form} layout="vertical" onFinish={onFinish}>
          <Form.Item name="displayName" label="Display Name" rules={[{ required: true }]}>
            <Input placeholder="Your name" />
          </Form.Item>
          <Form.Item name="username" label="Username" rules={[{ required: true, min: 3, max: 50 }]}>
            <Input placeholder="username" />
          </Form.Item>
          <Form.Item name="email" label="Email" rules={[{ required: true, type: 'email' }]}>
            <Input placeholder="you@example.com" />
          </Form.Item>
          <Form.Item name="password" label="Password" rules={[{ required: true, min: 8 }]}>
            <Input.Password placeholder="At least 8 characters" />
          </Form.Item>
          <Form.Item>
            <Button type="primary" htmlType="submit" loading={loading} block>Create Account</Button>
          </Form.Item>
        </Form>
        <div style={{ textAlign: 'center' }}>
          Already have an account? <Link to="/login">Sign In</Link>
        </div>
      </Card>
    </div>
  );
};

export default RegisterPage;
