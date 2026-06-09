import {
  Avatar, Button, Col, List, Row, Spin, Tag, Tabs, Typography, message,
} from 'antd';
import { TeamOutlined, UserOutlined, CrownOutlined } from '@ant-design/icons';
import dayjs from 'dayjs';
import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import groupsApi, { GroupDetailDto } from '../../api/groupsApi';
import PostCard from '../../components/common/PostCard';
import { useAuthStore } from '../../stores/authStore';

const { Title, Text } = Typography;

const GroupDetailPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const groupId = parseInt(id!, 10);
  const currentUser = useAuthStore((s) => s.user);

  const [group, setGroup] = useState<GroupDetailDto | null>(null);
  const [posts, setPosts] = useState<any[]>([]);
  const [postsTotal, setPostsTotal] = useState(0);
  const [postsPage, setPostsPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [loadingPosts, setLoadingPosts] = useState(false);
  const [joiningLeaving, setJoiningLeaving] = useState(false);

  useEffect(() => {
    loadGroup();
    loadPosts(1);
  }, [groupId]);

  const loadGroup = async () => {
    try {
      const res = await groupsApi.getGroup(groupId);
      setGroup(res.data);
    } catch {
      message.error('Failed to load group');
    } finally {
      setLoading(false);
    }
  };

  const loadPosts = async (page: number) => {
    setLoadingPosts(true);
    try {
      const res = await groupsApi.getGroupPosts(groupId, page);
      if (page === 1) {
        setPosts(res.data.items);
      } else {
        setPosts((prev) => [...prev, ...res.data.items]);
      }
      setPostsTotal(res.data.totalCount);
      setPostsPage(page);
    } catch {
      message.error('Failed to load posts');
    } finally {
      setLoadingPosts(false);
    }
  };

  const handleJoin = async () => {
    setJoiningLeaving(true);
    try {
      await groupsApi.joinGroup(groupId);
      message.success('Joined group');
      loadGroup();
    } catch {
      message.error('Failed to join group');
    } finally {
      setJoiningLeaving(false);
    }
  };

  const handleLeave = async () => {
    setJoiningLeaving(true);
    try {
      await groupsApi.leaveGroup(groupId);
      message.success('Left group');
      loadGroup();
    } catch {
      message.error('Failed to leave group');
    } finally {
      setJoiningLeaving(false);
    }
  };

  if (loading) {
    return (
      <div style={{ textAlign: 'center', padding: 80 }}>
        <Spin size="large" />
      </div>
    );
  }

  if (!group) return <div style={{ padding: 24 }}>Group not found.</div>;

  const isAdmin = group.currentUserRole === 1;
  const privacyLabel = group.privacy === 0 ? 'Public' : 'Private';

  return (
    <div style={{ maxWidth: 900, margin: '0 auto', padding: '24px 16px' }}>
      <Row gutter={24} align="middle" style={{ marginBottom: 24 }}>
        <Col>
          <Avatar size={80} src={group.avatarUrl} icon={<TeamOutlined />} />
        </Col>
        <Col flex="auto">
          <Title level={2} style={{ marginBottom: 4 }}>{group.name}</Title>
          <Tag color={group.privacy === 0 ? 'green' : 'orange'}>{privacyLabel}</Tag>
          <Text type="secondary" style={{ marginLeft: 8 }}>
            {group.memberCount} members
          </Text>
          {group.description && (
            <div style={{ marginTop: 8 }}>
              <Text>{group.description}</Text>
            </div>
          )}
        </Col>
        <Col>
          {group.isCurrentUserMember ? (
            <Button danger loading={joiningLeaving} onClick={handleLeave}>
              Leave Group
            </Button>
          ) : (
            <Button type="primary" loading={joiningLeaving} onClick={handleJoin}>
              Join Group
            </Button>
          )}
          {isAdmin && <Tag color="gold" style={{ marginLeft: 8 }}><CrownOutlined /> Admin</Tag>}
        </Col>
      </Row>

      <Tabs
        defaultActiveKey="posts"
        items={[
          {
            key: 'posts',
            label: `Posts (${postsTotal})`,
            children: (
              <div>
                {posts.map((post) => (
                  <PostCard key={post.id} post={post} currentUserId={currentUser?.userId ?? 0} />
                ))}
                {posts.length < postsTotal && (
                  <div style={{ textAlign: 'center', marginTop: 16 }}>
                    <Button loading={loadingPosts} onClick={() => loadPosts(postsPage + 1)}>
                      Load More
                    </Button>
                  </div>
                )}
                {!loadingPosts && posts.length === 0 && (
                  <Text type="secondary">No posts yet.</Text>
                )}
              </div>
            ),
          },
          {
            key: 'members',
            label: `Members (${group.memberCount})`,
            children: (
              <List
                dataSource={group.members}
                renderItem={(m) => (
                  <List.Item>
                    <List.Item.Meta
                      avatar={<Avatar src={m.avatarUrl} icon={<UserOutlined />} />}
                      title={
                        <span>
                          {m.displayName}
                          {m.role === 1 && (
                            <Tag color="gold" style={{ marginLeft: 8 }}>Admin</Tag>
                          )}
                        </span>
                      }
                      description={`@${m.username} · Joined ${dayjs(m.joinedAt).format('MMM YYYY')}`}
                    />
                  </List.Item>
                )}
              />
            ),
          },
        ]}
      />
    </div>
  );
};

export default GroupDetailPage;
