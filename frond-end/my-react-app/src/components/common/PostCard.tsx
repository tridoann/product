import React, { useState } from 'react';
import { Avatar, Button, Card, Input, Space, Typography } from 'antd';
import { LikeOutlined, LikeFilled, CommentOutlined } from '@ant-design/icons';
import dayjs from 'dayjs';
import relativeTime from 'dayjs/plugin/relativeTime';
import type { PostDto } from '../../api/socialApi';
import socialApi from '../../api/socialApi';

dayjs.extend(relativeTime);

const { Text, Paragraph } = Typography;

interface Props {
  post: PostDto;
  currentUserId: number;
  onDelete?: (id: number) => void;
}

const PostCard: React.FC<Props> = ({ post, currentUserId, onDelete }) => {
  const [liked, setLiked] = useState(false);
  const [likeCount, setLikeCount] = useState(post.likeCount);
  const [commentText, setCommentText] = useState('');
  const [showComments, setShowComments] = useState(false);

  const handleLike = async () => {
    const { data } = await socialApi.likePost(post.id);
    setLiked(data.liked);
    setLikeCount(prev => data.liked ? prev + 1 : prev - 1);
  };

  const handleComment = async () => {
    if (!commentText.trim()) return;
    await socialApi.commentOnPost(post.id, commentText);
    setCommentText('');
  };

  return (
    <Card
      style={{ marginBottom: 16 }}
      actions={[
        <Space key="like" onClick={handleLike} style={{ cursor: 'pointer' }}>
          {liked ? <LikeFilled style={{ color: '#1677ff' }} /> : <LikeOutlined />}
          <span>{likeCount}</span>
        </Space>,
        <Space key="comment" onClick={() => setShowComments(v => !v)} style={{ cursor: 'pointer' }}>
          <CommentOutlined />
          <span>{post.commentCount}</span>
        </Space>,
        post.authorId === currentUserId ? (
          <Text key="delete" type="danger" style={{ cursor: 'pointer' }} onClick={() => onDelete?.(post.id)}>
            Delete
          </Text>
        ) : <span key="placeholder" />,
      ]}
    >
      <Card.Meta
        avatar={<Avatar src={post.authorAvatarUrl}>{post.authorDisplayName[0]}</Avatar>}
        title={post.authorDisplayName}
        description={dayjs(post.createdAt).fromNow()}
      />
      <Paragraph style={{ marginTop: 12 }}>{post.content}</Paragraph>
      {post.mediaUrl && post.mediaType === 1 && (
        <img src={post.mediaUrl} alt="post media" style={{ maxWidth: '100%', borderRadius: 8 }} />
      )}
      {post.mediaUrl && post.mediaType === 2 && (
        <video src={post.mediaUrl} controls style={{ maxWidth: '100%', borderRadius: 8 }} />
      )}
      {showComments && (
        <div style={{ marginTop: 12 }}>
          {post.recentComments.map(c => (
            <div key={c.id} style={{ display: 'flex', gap: 8, marginBottom: 8 }}>
              <Avatar size="small" src={c.authorAvatarUrl}>{c.authorDisplayName[0]}</Avatar>
              <div>
                <Text strong style={{ fontSize: 12 }}>{c.authorDisplayName}</Text>
                <Text style={{ display: 'block', fontSize: 13 }}>{c.content}</Text>
              </div>
            </div>
          ))}
          <Space.Compact style={{ width: '100%', marginTop: 8 }}>
            <Input
              placeholder="Write a comment..."
              value={commentText}
              onChange={e => setCommentText(e.target.value)}
              onPressEnter={handleComment}
            />
            <Button onClick={handleComment}>Post</Button>
          </Space.Compact>
        </div>
      )}
    </Card>
  );
};

export default PostCard;
