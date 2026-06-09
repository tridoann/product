import React, { useEffect, useState } from 'react';
import { Avatar, Button, Card, Col, Empty, List, Row, Tabs, Typography } from 'antd';
import { UserAddOutlined, CheckOutlined, CloseOutlined } from '@ant-design/icons';
import socialApi, { FriendDto, FriendRequestDto } from '../../api/socialApi';

const { Title, Text } = Typography;

const FriendsPage: React.FC = () => {
  const [friends, setFriends] = useState<FriendDto[]>([]);
  const [requests, setRequests] = useState<FriendRequestDto[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    Promise.all([
      socialApi.getFriends(),
      socialApi.getFriendRequests(),
    ]).then(([f, r]) => {
      setFriends(f.data.friends ?? []);
      setRequests(r.data.requests ?? []);
    }).finally(() => setLoading(false));
  }, []);

  const respond = async (id: number, accept: boolean) => {
    await socialApi.respondFriendRequest(id, accept);
    setRequests(prev => prev.filter(r => r.id !== id));
  };

  return (
    <div style={{ maxWidth: 800, margin: '0 auto', padding: 24 }}>
      <Title level={4}>Friends</Title>
      <Tabs items={[
        {
          key: 'friends',
          label: `Friends (${friends.length})`,
          children: (
            <Row gutter={[16, 16]}>
              {friends.length === 0 && !loading && <Col span={24}><Empty /></Col>}
              {friends.map(f => (
                <Col key={f.userId} xs={24} sm={12} md={8}>
                  <Card size="small">
                    <Card.Meta
                      avatar={<Avatar src={f.avatarUrl}>{f.displayName[0]}</Avatar>}
                      title={f.displayName}
                      description={`@${f.username}`}
                    />
                  </Card>
                </Col>
              ))}
            </Row>
          )
        },
        {
          key: 'requests',
          label: `Requests (${requests.length})`,
          children: (
            <List
              dataSource={requests}
              locale={{ emptyText: 'No pending requests' }}
              renderItem={r => (
                <List.Item actions={[
                  <Button key="accept" size="small" type="primary" icon={<CheckOutlined />} onClick={() => respond(r.id, true)}>Accept</Button>,
                  <Button key="reject" size="small" danger icon={<CloseOutlined />} onClick={() => respond(r.id, false)}>Decline</Button>,
                ]}>
                  <List.Item.Meta
                    avatar={<Avatar src={r.senderAvatarUrl}>{r.senderDisplayName[0]}</Avatar>}
                    title={r.senderDisplayName}
                    description={`@${r.senderUsername}`}
                  />
                </List.Item>
              )}
            />
          )
        }
      ]} />
    </div>
  );
};

export default FriendsPage;
