import { Avatar, Button, Card, Col, Row, Spin, Tabs, Typography, message } from 'antd';
import { UserAddOutlined, UserOutlined } from '@ant-design/icons';
import dayjs from 'dayjs';
import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { authApi, PublicUserResponse } from '../../api/authApi';
import socialApi, { PostDto } from '../../api/socialApi';
import PostCard from '../../components/common/PostCard';
import { useAuthStore } from '../../stores/authStore';

const { Title, Text } = Typography;

const ProfilePage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const currentUser = useAuthStore((s) => s.user);

  const userId = id ? parseInt(id, 10) : currentUser?.userId ?? 0;
  const isOwnProfile = userId === currentUser?.userId;

  const [profile, setProfile] = useState<PublicUserResponse | null>(null);
  const [posts, setPosts] = useState<PostDto[]>([]);
  const [postTotal, setPostTotal] = useState(0);
  const [postPage, setPostPage] = useState(1);
  const [loadingProfile, setLoadingProfile] = useState(true);
  const [loadingPosts, setLoadingPosts] = useState(false);
  const [friendStatus, setFriendStatus] = useState<'none' | 'pending' | 'friends'>('none');

  useEffect(() => {
    if (!userId) return;
    setLoadingProfile(true);
    authApi
      .getUserById(userId)
      .then((res) => setProfile(res.data))
      .catch(() => message.error('Failed to load profile'))
      .finally(() => setLoadingProfile(false));

    loadPosts(1);
  }, [userId]);

  const loadPosts = async (page: number) => {
    setLoadingPosts(true);
    try {
      const res = await authApi.getUserPosts(userId, page);
      if (page === 1) {
        setPosts(res.data.items);
      } else {
        setPosts((prev) => [...prev, ...res.data.items]);
      }
      setPostTotal(res.data.totalCount);
      setPostPage(page);
    } catch {
      message.error('Failed to load posts');
    } finally {
      setLoadingPosts(false);
    }
  };

  const handleAddFriend = async () => {
    try {
      await socialApi.sendFriendRequest(userId);
      setFriendStatus('pending');
      message.success('Friend request sent');
    } catch {
      message.error('Failed to send friend request');
    }
  };

  if (loadingProfile) {
    return (
      <div style={{ textAlign: 'center', padding: 80 }}>
        <Spin size="large" />
      </div>
    );
  }

  if (!profile) {
    return <div style={{ padding: 24 }}>User not found.</div>;
  }

  return (
    <div style={{ maxWidth: 800, margin: '0 auto', padding: '24px 16px' }}>
      <Card style={{ marginBottom: 24 }}>
        <Row gutter={24} align="middle">
          <Col>
            <Avatar
              size={96}
              src={profile.avatarUrl}
              icon={!profile.avatarUrl && <UserOutlined />}
            />
          </Col>
          <Col flex="auto">
            <Title level={3} style={{ marginBottom: 4 }}>
              {profile.displayName}
            </Title>
            <Text type="secondary">@{profile.username}</Text>
            {profile.bio && (
              <div style={{ marginTop: 8 }}>
                <Text>{profile.bio}</Text>
              </div>
            )}
            <div style={{ marginTop: 8 }}>
              <Text type="secondary">
                Joined {dayjs(profile.createdAt).format('MMMM YYYY')}
              </Text>
            </div>
          </Col>
          <Col>
            {isOwnProfile ? (
              <Button onClick={() => navigate('/settings')}>Edit Profile</Button>
            ) : (
              friendStatus === 'friends' ? (
                <Button disabled>Friends</Button>
              ) : friendStatus === 'pending' ? (
                <Button disabled>Request Sent</Button>
              ) : (
                <Button
                  type="primary"
                  icon={<UserAddOutlined />}
                  onClick={handleAddFriend}
                >
                  Add Friend
                </Button>
              )
            )}
          </Col>
        </Row>
      </Card>

      <Tabs
        defaultActiveKey="posts"
        items={[
          {
            key: 'posts',
            label: `Posts (${postTotal})`,
            children: (
              <div>
                {posts.map((post) => (
                  <PostCard key={post.id} post={post} currentUserId={currentUser?.userId ?? 0} />
                ))}
                {posts.length < postTotal && (
                  <div style={{ textAlign: 'center', marginTop: 16 }}>
                    <Button
                      loading={loadingPosts}
                      onClick={() => loadPosts(postPage + 1)}
                    >
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
        ]}
      />
    </div>
  );
};

export default ProfilePage;
