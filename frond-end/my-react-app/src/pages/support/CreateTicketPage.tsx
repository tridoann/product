import React from 'react';
import { Button, Form, Input, Select, Space, Typography } from 'antd';
import { useNavigate } from 'react-router-dom';
import supportApi from '../../api/supportApi';

const { Title } = Typography;

const CreateTicketPage: React.FC = () => {
  const navigate = useNavigate();
  const [form] = Form.useForm();

  const handleSubmit = async (values: { subject: string; description: string; priority: number }) => {
    await supportApi.createTicket(values);
    navigate('/support');
  };

  return (
    <div style={{ maxWidth: 600, margin: '0 auto', padding: 24 }}>
      <Title level={4}>Submit Support Ticket</Title>
      <Form form={form} layout="vertical" onFinish={handleSubmit}>
        <Form.Item name="subject" label="Subject" rules={[{ required: true, max: 200 }]}>
          <Input />
        </Form.Item>
        <Form.Item name="priority" label="Priority" initialValue={1}>
          <Select options={[
            { value: 0, label: 'Low' },
            { value: 1, label: 'Medium' },
            { value: 2, label: 'High' },
          ]} />
        </Form.Item>
        <Form.Item name="description" label="Description" rules={[{ required: true }]}>
          <Input.TextArea rows={6} maxLength={2000} showCount />
        </Form.Item>
        <Space>
          <Button htmlType="submit" type="primary">Submit Ticket</Button>
          <Button onClick={() => navigate('/support')}>Cancel</Button>
        </Space>
      </Form>
    </div>
  );
};

export default CreateTicketPage;
