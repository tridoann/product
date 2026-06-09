import '@ant-design/v5-patch-for-react-19';
import 'antd/dist/reset.css';
import React from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import AppLayout from './components/layout/AppLayout';
import ProtectedRoute from './components/common/ProtectedRoute';
import AdminRoute from './components/common/AdminRoute';
import LoginPage from './pages/auth/LoginPage';
import RegisterPage from './pages/auth/RegisterPage';
import FeedPage from './pages/social/FeedPage';
import FriendsPage from './pages/social/FriendsPage';
import GroupsPage from './pages/groups/GroupsPage';
import MessagingPage from './pages/messaging/MessagingPage';
import CartPage from './pages/commerce/CartPage';
import OrdersPage from './pages/commerce/OrdersPage';
import TicketsPage from './pages/support/TicketsPage';
import CreateTicketPage from './pages/support/CreateTicketPage';
import AdminDashboardPage from './pages/admin/AdminDashboardPage';
import AdminUsersPage from './pages/admin/AdminUsersPage';
import AdminTicketsPage from './pages/admin/AdminTicketsPage';
import AdminOrdersPage from './pages/admin/AdminOrdersPage';
import ProfilePage from './pages/profile/ProfilePage';
import SettingsPage from './pages/profile/SettingsPage';
import GroupDetailPage from './pages/groups/GroupDetailPage';
import ProductsPage from './pages/commerce/ProductsPage';
import ProductDetailPage from './pages/commerce/ProductDetailPage';
import TicketDetailPage from './pages/support/TicketDetailPage';

const App: React.FC = () => (
  <BrowserRouter>
    <Routes>
      <Route path="/login" element={<LoginPage />} />
      <Route path="/register" element={<RegisterPage />} />

      <Route element={<ProtectedRoute />}>
        <Route element={<AppLayout />}>
          <Route path="/" element={<FeedPage />} />
          <Route path="/profile/:id" element={<ProfilePage />} />
          <Route path="/profile" element={<ProfilePage />} />
          <Route path="/settings" element={<SettingsPage />} />
          <Route path="/friends" element={<FriendsPage />} />
          <Route path="/groups" element={<GroupsPage />} />
          <Route path="/groups/:id" element={<GroupDetailPage />} />
          <Route path="/messages" element={<MessagingPage />} />
          <Route path="/messages/:id" element={<MessagingPage />} />
          <Route path="/products" element={<ProductsPage />} />
          <Route path="/products/:id" element={<ProductDetailPage />} />
          <Route path="/cart" element={<CartPage />} />
          <Route path="/orders" element={<OrdersPage />} />
          <Route path="/support" element={<TicketsPage />} />
          <Route path="/support/new" element={<CreateTicketPage />} />
          <Route path="/support/:id" element={<TicketDetailPage />} />

          <Route element={<AdminRoute />}>
            <Route path="/admin" element={<AdminDashboardPage />} />
            <Route path="/admin/users" element={<AdminUsersPage />} />
            <Route path="/admin/tickets" element={<AdminTicketsPage />} />
            <Route path="/admin/orders" element={<AdminOrdersPage />} />
          </Route>
        </Route>
      </Route>
    </Routes>
  </BrowserRouter>
);

export default App;
