import React, { useEffect, useRef, useState } from 'react';
import { Avatar, Badge, Button, Input, Layout, List, Typography } from 'antd';
import { SendOutlined } from '@ant-design/icons';
import dayjs from 'dayjs';
import messagingApi, { ConversationDto, MessageDto } from '../../api/messagingApi';
import { useAuthStore } from '../../stores/authStore';

const { Sider, Content } = Layout;
const { Text } = Typography;

const MessagingPage: React.FC = () => {
  const user = useAuthStore(s => s.user);
  const [conversations, setConversations] = useState<ConversationDto[]>([]);
  const [selectedId, setSelectedId] = useState<number | null>(null);
  const [messages, setMessages] = useState<MessageDto[]>([]);
  const [text, setText] = useState('');
  const bottomRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    messagingApi.getConversations().then(r => setConversations(r.data.items));
  }, []);

  const selectConversation = async (id: number) => {
    setSelectedId(id);
    const { data } = await messagingApi.getMessages(id);
    setMessages([...data.items].reverse());
    messagingApi.markRead(id);
    setConversations(prev => prev.map(c => c.id === id ? { ...c, unreadCount: 0 } : c));
  };

  const send = async () => {
    if (!text.trim() || !selectedId) return;
    const content = text.trim();
    setText('');
    const { data: msg } = await messagingApi.sendMessage(selectedId, content);
    setMessages(prev => [...prev, msg]);
    bottomRef.current?.scrollIntoView({ behavior: 'smooth' });
    setConversations(prev =>
      prev.map(c => c.id === selectedId ? { ...c, lastMessageContent: content } : c)
    );
  };

  const getConvTitle = (c: ConversationDto) => {
    if (c.title) return c.title;
    const other = c.participants.find(p => p.userId !== user?.userId);
    return other?.displayName ?? 'Conversation';
  };

  return (
    <Layout style={{ height: 'calc(100vh - 64px)', background: '#fff' }}>
      <Sider width={280} theme="light" style={{ borderRight: '1px solid #f0f0f0', overflow: 'auto' }}>
        <List
          dataSource={conversations}
          renderItem={c => (
            <List.Item
              style={{ cursor: 'pointer', padding: '12px 16px', background: selectedId === c.id ? '#f5f5f5' : undefined }}
              onClick={() => selectConversation(c.id)}
            >
              <List.Item.Meta
                avatar={<Badge count={c.unreadCount}><Avatar>{getConvTitle(c)[0]}</Avatar></Badge>}
                title={getConvTitle(c)}
                description={<Text ellipsis style={{ fontSize: 12 }}>{c.lastMessageContent ?? 'No messages'}</Text>}
              />
            </List.Item>
          )}
        />
      </Sider>
      <Content style={{ display: 'flex', flexDirection: 'column' }}>
        {selectedId ? (
          <>
            <div style={{ flex: 1, overflow: 'auto', padding: 16 }}>
              {messages.map(m => (
                <div key={m.id} style={{ marginBottom: 12, textAlign: m.senderId === user?.userId ? 'right' : 'left' }}>
                  <div style={{
                    display: 'inline-block', background: m.senderId === user?.userId ? '#1677ff' : '#f0f0f0',
                    color: m.senderId === user?.userId ? '#fff' : undefined,
                    borderRadius: 12, padding: '8px 12px', maxWidth: '70%'
                  }}>
                    {m.content}
                  </div>
                  <div style={{ fontSize: 11, color: '#999', marginTop: 2 }}>{dayjs(m.createdAt).format('HH:mm')}</div>
                </div>
              ))}
              <div ref={bottomRef} />
            </div>
            <div style={{ padding: 16, borderTop: '1px solid #f0f0f0', display: 'flex', gap: 8 }}>
              <Input
                value={text}
                onChange={e => setText(e.target.value)}
                onPressEnter={send}
                placeholder="Type a message..."
              />
              <Button type="primary" icon={<SendOutlined />} onClick={send} />
            </div>
          </>
        ) : (
          <div style={{ flex: 1, display: 'flex', alignItems: 'center', justifyContent: 'center', color: '#999' }}>
            Select a conversation to start messaging
          </div>
        )}
      </Content>
    </Layout>
  );
};

export default MessagingPage;
