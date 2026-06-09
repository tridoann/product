import React, { useEffect, useState } from 'react';
import { Button, Empty, Form, Input, InputNumber, Modal, Space, Table, Typography } from 'antd';
import { DeleteOutlined, ShoppingCartOutlined } from '@ant-design/icons';
import productsApi, { CartItemDto } from '../../api/productsApi';

const { Title, Text } = Typography;

const CartPage: React.FC = () => {
  const [items, setItems] = useState<CartItemDto[]>([]);
  const [total, setTotal] = useState(0);
  const [loading, setLoading] = useState(true);
  const [checkoutOpen, setCheckoutOpen] = useState(false);
  const [form] = Form.useForm();

  const load = async () => {
    const { data } = await productsApi.getCart();
    setItems(data.items);
    setTotal(data.total);
    setLoading(false);
  };

  useEffect(() => { load(); }, []);

  const remove = async (id: number) => {
    await productsApi.removeFromCart(id);
    load();
  };

  const handleCheckout = async (values: { shippingAddress: string }) => {
    await productsApi.placeOrder(values.shippingAddress);
    setCheckoutOpen(false);
    load();
  };

  const columns = [
    { title: 'Product', dataIndex: 'productName', key: 'name' },
    { title: 'Price', dataIndex: 'unitPrice', key: 'price', render: (v: number) => `$${v.toFixed(2)}` },
    { title: 'Qty', dataIndex: 'quantity', key: 'qty' },
    { title: 'Subtotal', key: 'sub', render: (_: unknown, r: CartItemDto) => `$${(r.unitPrice * r.quantity).toFixed(2)}` },
    {
      title: '', key: 'action', render: (_: unknown, r: CartItemDto) => (
        <Button size="small" danger icon={<DeleteOutlined />} onClick={() => remove(r.id)} />
      )
    },
  ];

  return (
    <div style={{ maxWidth: 800, margin: '0 auto', padding: 24 }}>
      <Title level={4}>Shopping Cart</Title>
      {items.length === 0 && !loading ? (
        <Empty description="Your cart is empty" />
      ) : (
        <>
          <Table dataSource={items} columns={columns} rowKey="id" pagination={false} loading={loading} />
          <div style={{ textAlign: 'right', marginTop: 16 }}>
            <Text strong style={{ fontSize: 18 }}>Total: ${total.toFixed(2)}</Text>
            <Button
              type="primary"
              icon={<ShoppingCartOutlined />}
              style={{ marginLeft: 16 }}
              onClick={() => setCheckoutOpen(true)}
            >
              Checkout
            </Button>
          </div>
        </>
      )}
      <Modal title="Checkout" open={checkoutOpen} onCancel={() => setCheckoutOpen(false)} footer={null}>
        <Form form={form} layout="vertical" onFinish={handleCheckout}>
          <Form.Item name="shippingAddress" label="Shipping Address" rules={[{ required: true }]}>
            <Input.TextArea rows={3} />
          </Form.Item>
          <Button htmlType="submit" type="primary" block>Place Order</Button>
        </Form>
      </Modal>
    </div>
  );
};

export default CartPage;
