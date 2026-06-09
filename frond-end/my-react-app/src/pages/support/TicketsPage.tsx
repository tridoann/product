import React, { useEffect, useState } from 'react';
import { Button, Empty, Table, Tag, Typography } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import dayjs from 'dayjs';
import { useNavigate } from 'react-router-dom';
import supportApi, { TicketSummaryDto } from '../../api/supportApi';

const { Title } = Typography;

const STATUS_COLORS: Record<number, string> = { 0: 'orange', 1: 'blue', 2: 'green', 3: 'default' };
const STATUS_LABELS: Record<number, string> = { 0: 'Open', 1: 'In Progress', 2: 'Resolved', 3: 'Closed' };
const PRIORITY_COLORS: Record<number, string> = { 0: 'default', 1: 'orange', 2: 'red' };
const PRIORITY_LABELS: Record<number, string> = { 0: 'Low', 1: 'Medium', 2: 'High' };

const TicketsPage: React.FC = () => {
  const navigate = useNavigate();
  const [tickets, setTickets] = useState<TicketSummaryDto[]>([]);
  const [total, setTotal] = useState(0);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    supportApi.getTickets().then(r => {
      setTickets(r.data.items);
      setTotal(r.data.totalCount);
    }).finally(() => setLoading(false));
  }, []);

  const columns = [
    { title: '#', dataIndex: 'id', key: 'id', width: 60 },
    { title: 'Subject', dataIndex: 'subject', key: 'subject' },
    {
      title: 'Priority', dataIndex: 'priority', key: 'priority',
      render: (v: number) => <Tag color={PRIORITY_COLORS[v]}>{PRIORITY_LABELS[v]}</Tag>
    },
    {
      title: 'Status', dataIndex: 'status', key: 'status',
      render: (v: number) => <Tag color={STATUS_COLORS[v]}>{STATUS_LABELS[v]}</Tag>
    },
    { title: 'Submitted', dataIndex: 'createdAt', key: 'date', render: (v: string) => dayjs(v).format('MMM D, YYYY') },
  ];

  return (
    <div style={{ maxWidth: 900, margin: '0 auto', padding: 24 }}>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 24 }}>
        <Title level={4} style={{ margin: 0 }}>Support Tickets</Title>
        <Button type="primary" icon={<PlusOutlined />} onClick={() => navigate('/support/new')}>New Ticket</Button>
      </div>
      {!loading && tickets.length === 0 ? (
        <Empty description="No tickets yet" />
      ) : (
        <Table dataSource={tickets} columns={columns} rowKey="id" loading={loading}
          pagination={{ total, pageSize: 20 }}
          onRow={r => ({ onClick: () => navigate(`/support/${r.id}`) })}
          rowClassName="cursor-pointer"
        />
      )}
    </div>
  );
};

export default TicketsPage;
