import React, { useCallback, useEffect, useRef, useState } from 'react';
import { Button, Empty, Form, Input, Spin, Typography } from 'antd';
import PostCard from '../../components/common/PostCard';
import socialApi, { PostDto } from '../../api/socialApi';
import { useAuthStore } from '../../stores/authStore';

const { Title } = Typography;
const { TextArea } = Input;

const FeedPage: React.FC = () => {
  const user = useAuthStore(s => s.user);
  const [posts, setPosts] = useState<PostDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [hasMore, setHasMore] = useState(true);
  const pageRef = useRef(1);
  const [form] = Form.useForm();

  const loadMore = useCallback(async () => {
    if (loading || !hasMore) return;
    setLoading(true);
    try {
      const { data } = await socialApi.getFeed(pageRef.current, 20);
      setPosts(prev => [...prev, ...data.items]);
      setHasMore(pageRef.current * 20 < data.totalCount);
      pageRef.current += 1;
    } finally {
      setLoading(false);
    }
  }, [loading, hasMore]);

  useEffect(() => { loadMore(); }, []);

  const handleCreatePost = async (values: { content: string }) => {
    await socialApi.createPost({ content: values.content });
    form.resetFields();
    pageRef.current = 1;
    setPosts([]);
    setHasMore(true);
    loadMore();
  };

  const handleDelete = async (id: number) => {
    await socialApi.deletePost(id);
    setPosts(prev => prev.filter(p => p.id !== id));
  };

  return (
    <div style={{ maxWidth: 680, margin: '0 auto', padding: 24 }}>
      <Title level={4}>News Feed</Title>
      <Form form={form} onFinish={handleCreatePost} style={{ marginBottom: 24 }}>
        <Form.Item name="content" rules={[{ required: true, min: 1 }]}>
          <TextArea rows={3} placeholder="What's on your mind?" maxLength={2000} showCount />
        </Form.Item>
        <Button htmlType="submit" type="primary">Post</Button>
      </Form>
      {posts.map(p => (
        <PostCard
          key={p.id}
          post={p}
          currentUserId={user?.userId ?? 0}
          onDelete={handleDelete}
        />
      ))}
      {loading && <Spin style={{ display: 'block', textAlign: 'center', margin: 24 }} />}
      {!loading && !hasMore && posts.length > 0 && (
        <div style={{ textAlign: 'center', color: '#999', padding: 16 }}>No more posts</div>
      )}
      {!loading && posts.length === 0 && <Empty description="No posts yet. Add friends to see their posts!" />}
      {!loading && hasMore && (
        <Button block onClick={loadMore}>Load more</Button>
      )}
    </div>
  );
};

export default FeedPage;
