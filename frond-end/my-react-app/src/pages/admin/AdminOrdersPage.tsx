import React, { useEffect, useState } from 'react';
import { Select, Table, Tag, Typography } from 'antd';
import dayjs from 'dayjs';
import adminApi from '../../api/adminApi';

const { Title } = Typography;

interface OrderRow {
  id: number;
  buyerUsername?: string;
  status: number;
  totalAmount: number;
  itemCount: number;
  createdAt: string;
}

const STATUS_LABELS: Record<number, { text: string; color: string }> = {
  0: { text: 'Pending', color: 'orange' },
  1: { text: 'Confirmed', color: 'blue' },
  2: { text: 'Shipped', color: 'cyan' },
  3: { text: 'Delivered', color: 'green' },
  4: { text: 'Cancelled', color: 'red' },
};

const AdminOrdersPage: React.FC = () => {
  const [orders, setOrders] = useState<OrderRow[]>([]);
  const [total, setTotal] = useState(0);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    adminApi.getOrders().then((r: any) => {
      setOrders(r.data.items);
      setTotal(r.data.totalCount);
    }).finally(() => setLoading(false));
  }, []);

  const updateStatus = async (id: number, status: number) => {
    await adminApi.updateOrderStatus(id, status);
    setOrders(prev => prev.map(o => o.id === id ? { ...o, status } : o));
  };

  const columns = [
    { title: 'Order #', dataIndex: 'id', key: 'id', width: 80 },
    { title: 'Buyer', dataIndex: 'buyerUsername', key: 'buyer', render: (v?: string) => v ? `@${v}` : '—' },
    {
      title: 'Status', dataIndex: 'status', key: 'status',
      render: (v: number, r: OrderRow) => (
        <Select
          value={v}
          size="small"
          style={{ minWidth: 120 }}
          onChange={s => updateStatus(r.id, s)}
          options={Object.entries(STATUS_LABELS).map(([k, l]) => ({ value: Number(k), label: l.text }))}
          onClick={(e) => e.stopPropagation()}
        />
      )
    },
    { title: 'Items', dataIndex: 'itemCount', key: 'items', width: 70 },
    { title: 'Total', dataIndex: 'totalAmount', key: 'total', render: (v: number) => `$${v.toFixed(2)}` },
    { title: 'Date', dataIndex: 'createdAt', key: 'date', render: (v: string) => dayjs(v).format('MMM D, YYYY') },
  ];

  return (
    <div style={{ maxWidth: 1000, margin: '0 auto', padding: 24 }}>
      <Title level={4}>All Orders</Title>
      <Table dataSource={orders} columns={columns} rowKey="id" loading={loading} pagination={{ total, pageSize: 20 }} />
    </div>
  );
};

export default AdminOrdersPage;
