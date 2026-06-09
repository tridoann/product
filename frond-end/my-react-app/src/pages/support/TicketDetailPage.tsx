import {
  Avatar, Button, Card, Input, Select, Space, Spin, Tag, Timeline, Typography, message,
} from 'antd';
import { UserOutlined, CustomerServiceOutlined } from '@ant-design/icons';
import dayjs from 'dayjs';
import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import supportApi, { TicketDetailDto } from '../../api/supportApi';
import { useAuthStore } from '../../stores/authStore';

const { Title, Text, Paragraph } = Typography;

const STATUS_LABELS: Record<number, string> = { 0: 'Open', 1: 'In Progress', 2: 'Resolved', 3: 'Closed' };
const STATUS_COLORS: Record<number, string> = { 0: 'blue', 1: 'orange', 2: 'green', 3: 'default' };
const PRIORITY_LABELS: Record<number, string> = { 0: 'Low', 1: 'Medium', 2: 'High' };
const PRIORITY_COLORS: Record<number, string> = { 0: 'default', 1: 'orange', 2: 'red' };

const TicketDetailPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const ticketId = parseInt(id!, 10);
  const user = useAuthStore((s) => s.user);
  const isAdmin = user?.role === 'Admin';

  const [ticket, setTicket] = useState<TicketDetailDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [replyText, setReplyText] = useState('');
  const [replying, setReplying] = useState(false);
  const [updatingStatus, setUpdatingStatus] = useState(false);

  useEffect(() => {
    loadTicket();
  }, [ticketId]);

  const loadTicket = async () => {
    try {
      const res = await supportApi.getTicket(ticketId);
      setTicket(res.data);
    } catch {
      message.error('Failed to load ticket');
    } finally {
      setLoading(false);
    }
  };

  const handleReply = async () => {
    if (!replyText.trim()) return;
    setReplying(true);
    try {
      await supportApi.replyToTicket(ticketId, replyText);
      setReplyText('');
      loadTicket();
      message.success('Reply sent');
    } catch {
      message.error('Failed to send reply');
    } finally {
      setReplying(false);
    }
  };

  const handleStatusChange = async (status: number) => {
    setUpdatingStatus(true);
    try {
      await supportApi.updateStatus(ticketId, status);
      loadTicket();
      message.success('Status updated');
    } catch {
      message.error('Failed to update status');
    } finally {
      setUpdatingStatus(false);
    }
  };

  if (loading) {
    return (
      <div style={{ textAlign: 'center', padding: 80 }}>
        <Spin size="large" />
      </div>
    );
  }

  if (!ticket) return <div style={{ padding: 24 }}>Ticket not found.</div>;

  const timelineItems = [
    {
      dot: <CustomerServiceOutlined style={{ fontSize: 16 }} />,
      color: 'blue',
      children: (
        <div>
          <Text strong>Ticket opened</Text>
          <br />
          <Paragraph style={{ marginTop: 8, background: '#f5f5f5', padding: 12, borderRadius: 6 }}>
            {ticket.description}
          </Paragraph>
          <Text type="secondary" style={{ fontSize: 12 }}>
            {dayjs(ticket.createdAt).format('MMM D, YYYY HH:mm')} by @{ticket.submittedByUsername}
          </Text>
        </div>
      ),
    },
    ...ticket.replies.map((r) => ({
      dot: (
        <Avatar
          size="small"
          icon={<UserOutlined />}
          style={{ background: r.isAdminReply ? '#1677ff' : '#52c41a' }}
        />
      ),
      children: (
        <div>
          <Text strong>{r.authorDisplayName}</Text>
          {r.isAdminReply && <Tag color="blue" style={{ marginLeft: 8 }}>Support</Tag>}
          <br />
          <Paragraph style={{ marginTop: 8, background: r.isAdminReply ? '#e6f4ff' : '#f6ffed', padding: 12, borderRadius: 6 }}>
            {r.content}
          </Paragraph>
          <Text type="secondary" style={{ fontSize: 12 }}>
            {dayjs(r.createdAt).format('MMM D, YYYY HH:mm')}
          </Text>
        </div>
      ),
    })),
  ];

  return (
    <div style={{ maxWidth: 760, margin: '0 auto', padding: '24px 16px' }}>
      <Card style={{ marginBottom: 24 }}>
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
          <div>
            <Title level={3} style={{ marginBottom: 8 }}>{ticket.subject}</Title>
            <Space>
              <Tag color={STATUS_COLORS[ticket.status]}>{STATUS_LABELS[ticket.status]}</Tag>
              <Tag color={PRIORITY_COLORS[ticket.priority]}>{PRIORITY_LABELS[ticket.priority]} Priority</Tag>
            </Space>
          </div>
          {isAdmin && (
            <Select
              value={ticket.status}
              style={{ width: 140 }}
              loading={updatingStatus}
              onChange={handleStatusChange}
              options={[
                { value: 0, label: 'Open' },
                { value: 1, label: 'In Progress' },
                { value: 2, label: 'Resolved' },
                { value: 3, label: 'Closed' },
              ]}
            />
          )}
        </div>
        <Text type="secondary" style={{ marginTop: 8, display: 'block' }}>
          Submitted by @{ticket.submittedByUsername} on {dayjs(ticket.createdAt).format('MMM D, YYYY')}
        </Text>
        {ticket.resolvedAt && (
          <Text type="secondary" style={{ display: 'block' }}>
            Resolved on {dayjs(ticket.resolvedAt).format('MMM D, YYYY')}
          </Text>
        )}
      </Card>

      <Timeline items={timelineItems} style={{ marginBottom: 24 }} />

      {ticket.status !== 2 && ticket.status !== 3 && (
        <Card title="Add Reply">
          <Input.TextArea
            rows={4}
            placeholder="Write your reply..."
            value={replyText}
            onChange={(e) => setReplyText(e.target.value)}
            maxLength={2000}
            showCount
          />
          <Button
            type="primary"
            style={{ marginTop: 12 }}
            loading={replying}
            disabled={!replyText.trim()}
            onClick={handleReply}
          >
            Send Reply
          </Button>
        </Card>
      )}
    </div>
  );
};

export default TicketDetailPage;
