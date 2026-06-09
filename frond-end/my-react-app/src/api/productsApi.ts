import apiClient from '../common';

export interface CategoryDto {
  id: number;
  name: string;
  slug: string;
  parentId?: number;
  children: CategoryDto[];
}

export interface ProductDto {
  id: number;
  name: string;
  description: string;
  price: number;
  categoryId?: number;
  categoryName?: string;
  sellerId?: number;
  sellerDisplayName?: string;
  stockQuantity: number;
  imageUrl?: string;
  isActive: boolean;
  createdAt: string;
}

export interface CartItemDto {
  id: number;
  productId: number;
  productName: string;
  productImageUrl?: string;
  unitPrice: number;
  quantity: number;
}

export interface OrderDto {
  id: number;
  status: number;
  totalAmount: number;
  shippingAddress: string;
  itemCount: number;
  createdAt: string;
}

const productsApi = {
  getProducts: (params?: { categoryId?: number; search?: string; pageIndex?: number; pageSize?: number }) =>
    apiClient.get<{ items: ProductDto[]; totalCount: number; pageIndex: number; pageSize: number }>(
      '/api/products',
      { params: { pageIndex: 1, pageSize: 20, ...params } }
    ),
  getProduct: (id: number) => apiClient.get<ProductDto>(`/api/products/${id}`),
  getCategories: () => apiClient.get<{ items: CategoryDto[] }>('/api/categories'),
  getCart: () => apiClient.get<{ cartId: number; items: CartItemDto[]; total: number }>('/api/cart'),
  addToCart: (productId: number, quantity = 1) => apiClient.post('/api/cart', { productId, quantity }),
  removeFromCart: (itemId: number) => apiClient.delete(`/api/cart/items/${itemId}`),
  getOrders: (pageIndex = 1) => apiClient.get<{ items: OrderDto[]; totalCount: number }>(`/api/orders?pageIndex=${pageIndex}`),
  placeOrder: (shippingAddress: string) => apiClient.post('/api/orders', { shippingAddress }),
};

export default productsApi;
