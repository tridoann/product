import {
  Button, Card, Col, Input, Row, Select, Spin, Tag, Typography, message,
} from 'antd';
import { ShoppingCartOutlined } from '@ant-design/icons';
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import productsApi, { CategoryDto, ProductDto } from '../../api/productsApi';

const { Title, Text } = Typography;
const { Search } = Input;

const ProductsPage: React.FC = () => {
  const navigate = useNavigate();
  const [products, setProducts] = useState<ProductDto[]>([]);
  const [categories, setCategories] = useState<CategoryDto[]>([]);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');
  const [categoryId, setCategoryId] = useState<number | undefined>();
  const [loading, setLoading] = useState(false);
  const [addingId, setAddingId] = useState<number | null>(null);

  useEffect(() => {
    productsApi.getCategories().then((r) => setCategories(r.data.items)).catch(() => {});
  }, []);

  useEffect(() => {
    loadProducts(1);
  }, [search, categoryId]);

  const loadProducts = async (p: number) => {
    setLoading(true);
    try {
      const res = await productsApi.getProducts({ search, categoryId, pageIndex: p, pageSize: 20 });
      if (p === 1) {
        setProducts(res.data.items);
      } else {
        setProducts((prev) => [...prev, ...res.data.items]);
      }
      setTotal(res.data.totalCount);
      setPage(p);
    } catch {
      message.error('Failed to load products');
    } finally {
      setLoading(false);
    }
  };

  const handleAddToCart = async (productId: number) => {
    setAddingId(productId);
    try {
      await productsApi.addToCart(productId, 1);
      message.success('Added to cart');
    } catch {
      message.error('Failed to add to cart');
    } finally {
      setAddingId(null);
    }
  };

  return (
    <div style={{ padding: '24px 16px' }}>
      <Title level={3}>Shop</Title>

      <Row gutter={12} style={{ marginBottom: 16 }}>
        <Col flex="auto">
          <Search
            placeholder="Search products..."
            allowClear
            onSearch={(v) => setSearch(v)}
          />
        </Col>
        <Col>
          <Select
            style={{ width: 160 }}
            placeholder="Category"
            allowClear
            onChange={(v) => setCategoryId(v)}
            options={categories.map((c) => ({ value: c.id, label: c.name }))}
          />
        </Col>
      </Row>

      {loading && page === 1 ? (
        <div style={{ textAlign: 'center', padding: 60 }}>
          <Spin size="large" />
        </div>
      ) : (
        <>
          <Row gutter={[16, 16]}>
            {products.map((p) => (
              <Col key={p.id} xs={24} sm={12} md={8} lg={6}>
                <Card
                  hoverable
                  cover={
                    p.imageUrl ? (
                      <img
                        alt={p.name}
                        src={p.imageUrl}
                        style={{ height: 180, objectFit: 'cover' }}
                        onClick={() => navigate(`/products/${p.id}`)}
                      />
                    ) : (
                      <div
                        style={{ height: 180, background: '#f5f5f5', display: 'flex', alignItems: 'center', justifyContent: 'center', cursor: 'pointer' }}
                        onClick={() => navigate(`/products/${p.id}`)}
                      >
                        <Text type="secondary">No image</Text>
                      </div>
                    )
                  }
                  actions={[
                    <Button
                      key="cart"
                      type="primary"
                      icon={<ShoppingCartOutlined />}
                      loading={addingId === p.id}
                      disabled={p.stockQuantity === 0}
                      onClick={() => handleAddToCart(p.id)}
                    >
                      {p.stockQuantity === 0 ? 'Out of stock' : 'Add to Cart'}
                    </Button>,
                  ]}
                >
                  <Card.Meta
                    title={
                      <span
                        style={{ cursor: 'pointer' }}
                        onClick={() => navigate(`/products/${p.id}`)}
                      >
                        {p.name}
                      </span>
                    }
                    description={
                      <div>
                        {p.categoryName && <Tag color="blue">{p.categoryName}</Tag>}
                        <div style={{ marginTop: 4 }}>
                          <Text strong style={{ fontSize: 16 }}>
                            ${p.price.toFixed(2)}
                          </Text>
                        </div>
                        <Text type="secondary" style={{ fontSize: 12 }}>
                          {p.stockQuantity} in stock
                        </Text>
                      </div>
                    }
                  />
                </Card>
              </Col>
            ))}
          </Row>

          {products.length < total && (
            <div style={{ textAlign: 'center', marginTop: 24 }}>
              <Button loading={loading} onClick={() => loadProducts(page + 1)}>
                Load More
              </Button>
            </div>
          )}

          {!loading && products.length === 0 && (
            <div style={{ textAlign: 'center', padding: 60 }}>
              <Text type="secondary">No products found.</Text>
            </div>
          )}
        </>
      )}
    </div>
  );
};

export default ProductsPage;
