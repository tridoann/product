import {
  Badge, Button, Col, Descriptions, Image, Row, Spin, Tag, Typography, message,
} from 'antd';
import { ShoppingCartOutlined } from '@ant-design/icons';
import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import productsApi, { ProductDto } from '../../api/productsApi';

const { Title, Paragraph, Text } = Typography;

const ProductDetailPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [product, setProduct] = useState<ProductDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [addingToCart, setAddingToCart] = useState(false);
  const [quantity, setQuantity] = useState(1);

  useEffect(() => {
    if (!id) return;
    productsApi
      .getProduct(parseInt(id, 10))
      .then((r) => setProduct(r.data))
      .catch(() => message.error('Product not found'))
      .finally(() => setLoading(false));
  }, [id]);

  const handleAddToCart = async () => {
    if (!product) return;
    setAddingToCart(true);
    try {
      await productsApi.addToCart(product.id, quantity);
      message.success(`${quantity}x "${product.name}" added to cart`);
    } catch {
      message.error('Failed to add to cart');
    } finally {
      setAddingToCart(false);
    }
  };

  if (loading) {
    return (
      <div style={{ textAlign: 'center', padding: 80 }}>
        <Spin size="large" />
      </div>
    );
  }

  if (!product) {
    return <div style={{ padding: 24 }}>Product not found.</div>;
  }

  return (
    <div style={{ maxWidth: 900, margin: '0 auto', padding: '24px 16px' }}>
      <Row gutter={32}>
        <Col xs={24} md={10}>
          {product.imageUrl ? (
            <Image
              src={product.imageUrl}
              alt={product.name}
              style={{ width: '100%', borderRadius: 8 }}
            />
          ) : (
            <div
              style={{ height: 300, background: '#f5f5f5', borderRadius: 8, display: 'flex', alignItems: 'center', justifyContent: 'center' }}
            >
              <Text type="secondary">No image</Text>
            </div>
          )}
        </Col>

        <Col xs={24} md={14}>
          <Title level={2}>{product.name}</Title>

          {product.categoryName && <Tag color="blue" style={{ marginBottom: 12 }}>{product.categoryName}</Tag>}

          <Title level={3} style={{ color: '#1677ff', marginTop: 8 }}>
            ${product.price.toFixed(2)}
          </Title>

          <Badge
            status={product.stockQuantity > 0 ? 'success' : 'error'}
            text={product.stockQuantity > 0 ? `${product.stockQuantity} in stock` : 'Out of stock'}
            style={{ marginBottom: 16, display: 'block' }}
          />

          {product.sellerDisplayName && (
            <Text type="secondary" style={{ display: 'block', marginBottom: 16 }}>
              Sold by: <Text strong>{product.sellerDisplayName}</Text>
            </Text>
          )}

          <div style={{ display: 'flex', gap: 12, marginBottom: 24, alignItems: 'center' }}>
            <Button
              onClick={() => setQuantity((q) => Math.max(1, q - 1))}
              disabled={quantity <= 1}
            >
              –
            </Button>
            <Text strong style={{ fontSize: 16, minWidth: 30, textAlign: 'center' }}>{quantity}</Text>
            <Button
              onClick={() => setQuantity((q) => Math.min(product.stockQuantity, q + 1))}
              disabled={quantity >= product.stockQuantity}
            >
              +
            </Button>
          </div>

          <Button
            type="primary"
            size="large"
            icon={<ShoppingCartOutlined />}
            loading={addingToCart}
            disabled={product.stockQuantity === 0}
            onClick={handleAddToCart}
            style={{ width: '100%' }}
          >
            {product.stockQuantity === 0 ? 'Out of Stock' : 'Add to Cart'}
          </Button>
        </Col>
      </Row>

      <Descriptions title="Details" style={{ marginTop: 32 }} column={1}>
        <Descriptions.Item label="Description">
          <Paragraph>{product.description}</Paragraph>
        </Descriptions.Item>
      </Descriptions>
    </div>
  );
};

export default ProductDetailPage;
