import React, { useEffect, useState } from 'react';
import { Button, Card, Col, Empty, Form, Input, Modal, Row, Select, Typography } from 'antd';
import { PlusOutlined, TeamOutlined } from '@ant-design/icons';
import groupsApi, { GroupDto } from '../../api/groupsApi';

const { Title, Text } = Typography;

const GroupsPage: React.FC = () => {
  const [groups, setGroups] = useState<GroupDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [createOpen, setCreateOpen] = useState(false);
  const [form] = Form.useForm();

  useEffect(() => {
    groupsApi.getGroups().then(r => setGroups(r.data.items)).finally(() => setLoading(false));
  }, []);

  const handleCreate = async (values: { name: string; description?: string; privacy: number }) => {
    await groupsApi.createGroup(values);
    setCreateOpen(false);
    form.resetFields();
    groupsApi.getGroups().then(r => setGroups(r.data.items));
  };

  const handleJoin = async (id: number) => {
    await groupsApi.joinGroup(id);
  };

  return (
    <div style={{ maxWidth: 900, margin: '0 auto', padding: 24 }}>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 24 }}>
        <Title level={4} style={{ margin: 0 }}>Groups</Title>
        <Button type="primary" icon={<PlusOutlined />} onClick={() => setCreateOpen(true)}>Create Group</Button>
      </div>

      {!loading && groups.length === 0 && <Empty description="No public groups yet" />}
      <Row gutter={[16, 16]}>
        {groups.map(g => (
          <Col key={g.id} xs={24} sm={12} md={8}>
            <Card
              cover={g.avatarUrl ? <img src={g.avatarUrl} alt={g.name} style={{ height: 120, objectFit: 'cover' }} /> : undefined}
              actions={[
                <Button key="join" size="small" type="primary" onClick={() => handleJoin(g.id)}>Join</Button>
              ]}
            >
              <Card.Meta
                title={g.name}
                description={
                  <>
                    <Text type="secondary" style={{ fontSize: 12 }}>{g.memberCount} members · {g.privacy === 0 ? 'Public' : 'Private'}</Text>
                    {g.description && <div style={{ marginTop: 4 }}>{g.description}</div>}
                  </>
                }
              />
            </Card>
          </Col>
        ))}
      </Row>

      <Modal title="Create Group" open={createOpen} onCancel={() => setCreateOpen(false)} footer={null}>
        <Form form={form} layout="vertical" onFinish={handleCreate}>
          <Form.Item name="name" label="Group Name" rules={[{ required: true, min: 3 }]}>
            <Input />
          </Form.Item>
          <Form.Item name="description" label="Description">
            <Input.TextArea rows={3} maxLength={500} showCount />
          </Form.Item>
          <Form.Item name="privacy" label="Privacy" initialValue={0}>
            <Select options={[{ value: 0, label: 'Public' }, { value: 1, label: 'Private' }]} />
          </Form.Item>
          <Button htmlType="submit" type="primary" block>Create</Button>
        </Form>
      </Modal>
    </div>
  );
};

export default GroupsPage;
