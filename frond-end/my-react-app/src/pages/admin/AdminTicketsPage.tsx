import React, { useEffect, useState } from 'react';
import { Select, Table, Tag, Typography } from 'antd';
import dayjs from 'dayjs';
import { useNavigate } from 'react-router-dom';
import adminApi from '../../api/adminApi';

const { Title } = Typography;

interface TicketRow {
  id: number;
  subject: string;
  status: number;
  priority: number;
  submittedByUsername: string;
  createdAt: string;
}

const STATUS_COLORS: Record<number, string> = { 0: 'orange', 1: 'blue', 2: 'green', 3: 'default' };
const STATUS_LABELS: Record<number, string> = { 0: 'Open', 1: 'In Progress', 2: 'Resolved', 3: 'Closed' };

const AdminTicketsPage: React.FC = () => {
  const navigate = useNavigate();
  const [tickets, setTickets] = useState<TicketRow[]>([]);
  const [total, setTotal] = useState(0);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    adminApi.getTickets().then((r: any) => {
      setTickets(r.data.items);
      setTotal(r.data.totalCount);
    }).finally(() => setLoading(false));
  }, []);

  const columns = [
    { title: '#', dataIndex: 'id', key: 'id', width: 60 },
    {
      title: 'Subject', dataIndex: 'subject', key: 'subject',
      render: (v: string, r: TicketRow) => (
        <a onClick={() => navigate(`/support/${r.id}`)}>{v}</a>
      ),
    },
    { title: 'Submitted By', dataIndex: 'submittedByUsername', key: 'by' },
    {
      title: 'Status', dataIndex: 'status', key: 'status',
      render: (v: number) => <Tag color={STATUS_COLORS[v]}>{STATUS_LABELS[v]}</Tag>
    },
    { title: 'Priority', dataIndex: 'priority', key: 'priority',
      render: (v: number) => <Tag color={['default','orange','red'][v]}>{['Low','Medium','High'][v]}</Tag> },
    { title: 'Date', dataIndex: 'createdAt', key: 'date', render: (v: string) => dayjs(v).format('MMM D, YYYY') },
  ];

  return (
    <div style={{ maxWidth: 1100, margin: '0 auto', padding: 24 }}>
      <Title level={4}>All Support Tickets</Title>
      <Table
        dataSource={tickets}
        columns={columns}
        rowKey="id"
        loading={loading}
        pagination={{ total, pageSize: 20 }}
        onRow={(r) => ({ onClick: () => navigate(`/support/${r.id}`) })}
        rowClassName={() => 'clickable-row'}
      />
    </div>
  );
};

export default AdminTicketsPage;
