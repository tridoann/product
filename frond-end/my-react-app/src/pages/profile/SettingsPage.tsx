import { Button, Card, Form, Input, Tabs, Typography, message } from 'antd';
import React, { useEffect } from 'react';
import { authApi } from '../../api/authApi';
import { useAuthStore } from '../../stores/authStore';

const { Title } = Typography;

interface ProfileFormValues {
  displayName: string;
  bio?: string;
  avatarUrl?: string;
}

interface PasswordFormValues {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}

const ProfileTab: React.FC = () => {
  const [form] = Form.useForm<ProfileFormValues>();
  const user = useAuthStore((s) => s.user);
  const updateUser = useAuthStore((s) => s.updateUser);
  const [loading, setLoading] = React.useState(false);

  useEffect(() => {
    if (user) {
      form.setFieldsValue({
        displayName: user.displayName,
        bio: user.bio,
        avatarUrl: user.avatarUrl,
      });
    }
  }, [user, form]);

  const onFinish = async (values: ProfileFormValues) => {
    setLoading(true);
    try {
      await authApi.updateProfile(values);
      updateUser({ displayName: values.displayName, bio: values.bio, avatarUrl: values.avatarUrl });
      message.success('Profile updated');
    } catch {
      message.error('Failed to update profile');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Form form={form} layout="vertical" onFinish={onFinish} style={{ maxWidth: 480 }}>
      <Form.Item
        label="Display Name"
        name="displayName"
        rules={[{ required: true, message: 'Display name is required' }]}
      >
        <Input maxLength={100} />
      </Form.Item>

      <Form.Item label="Bio" name="bio">
        <Input.TextArea rows={4} maxLength={500} showCount />
      </Form.Item>

      <Form.Item label="Avatar URL" name="avatarUrl">
        <Input placeholder="https://..." />
      </Form.Item>

      <Form.Item>
        <Button type="primary" htmlType="submit" loading={loading}>
          Save Changes
        </Button>
      </Form.Item>
    </Form>
  );
};

const SecurityTab: React.FC = () => {
  const [form] = Form.useForm<PasswordFormValues>();
  const [loading, setLoading] = React.useState(false);

  const onFinish = async (values: PasswordFormValues) => {
    if (values.newPassword !== values.confirmPassword) {
      message.error('New passwords do not match');
      return;
    }
    setLoading(true);
    try {
      await authApi.changePassword({
        currentPassword: values.currentPassword,
        newPassword: values.newPassword,
      });
      message.success('Password changed');
      form.resetFields();
    } catch {
      message.error('Failed to change password. Check your current password.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Form form={form} layout="vertical" onFinish={onFinish} style={{ maxWidth: 480 }}>
      <Form.Item
        label="Current Password"
        name="currentPassword"
        rules={[{ required: true }]}
      >
        <Input.Password />
      </Form.Item>

      <Form.Item
        label="New Password"
        name="newPassword"
        rules={[{ required: true, min: 6, message: 'Password must be at least 6 characters' }]}
      >
        <Input.Password />
      </Form.Item>

      <Form.Item
        label="Confirm New Password"
        name="confirmPassword"
        rules={[{ required: true }]}
      >
        <Input.Password />
      </Form.Item>

      <Form.Item>
        <Button type="primary" htmlType="submit" loading={loading}>
          Change Password
        </Button>
      </Form.Item>
    </Form>
  );
};

const SettingsPage: React.FC = () => (
  <div style={{ maxWidth: 600, margin: '0 auto', padding: '24px 16px' }}>
    <Card>
      <Title level={3} style={{ marginBottom: 24 }}>
        Settings
      </Title>
      <Tabs
        defaultActiveKey="profile"
        items={[
          { key: 'profile', label: 'Profile', children: <ProfileTab /> },
          { key: 'security', label: 'Security', children: <SecurityTab /> },
        ]}
      />
    </Card>
  </div>
);

export default SettingsPage;
