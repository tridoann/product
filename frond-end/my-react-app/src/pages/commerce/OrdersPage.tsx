import React, { useEffect, useState } from 'react';
import { Empty, Table, Tag, Typography } from 'antd';
import dayjs from 'dayjs';
import productsApi, { OrderDto } from '../../api/productsApi';

const { Title } = Typography;

const STATUS_LABELS: Record<number, { text: string; color: string }> = {
  0: { text: 'Pending', color: 'orange' },
  1: { text: 'Confirmed', color: 'blue' },
  2: { text: 'Shipped', color: 'cyan' },
  3: { text: 'Delivered', color: 'green' },
  4: { text: 'Cancelled', color: 'red' },
};

const OrdersPage: React.FC = () => {
  const [orders, setOrders] = useState<OrderDto[]>([]);
  const [total, setTotal] = useState(0);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    productsApi.getOrders().then(r => {
      setOrders(r.data.items);
      setTotal(r.data.totalCount);
    }).finally(() => setLoading(false));
  }, []);

  const columns = [
    { title: 'Order #', dataIndex: 'id', key: 'id' },
    {
      title: 'Status', dataIndex: 'status', key: 'status',
      render: (s: number) => <Tag color={STATUS_LABELS[s]?.color}>{STATUS_LABELS[s]?.text}</Tag>
    },
    { title: 'Items', dataIndex: 'itemCount', key: 'items' },
    { title: 'Total', dataIndex: 'totalAmount', key: 'total', render: (v: number) => `$${v.toFixed(2)}` },
    { title: 'Date', dataIndex: 'createdAt', key: 'date', render: (v: string) => dayjs(v).format('MMM D, YYYY') },
  ];

  return (
    <div style={{ maxWidth: 900, margin: '0 auto', padding: 24 }}>
      <Title level={4}>My Orders</Title>
      {!loading && orders.length === 0 ? (
        <Empty description="No orders yet" />
      ) : (
        <Table dataSource={orders} columns={columns} rowKey="id" loading={loading} pagination={{ total, pageSize: 20 }} />
      )}
    </div>
  );
};

export default OrdersPage;
