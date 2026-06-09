import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import { useAuthStore } from '../../stores/authStore';

const AdminRoute: React.FC = () => {
  const isAdmin = useAuthStore((s) => s.isAdmin());
  return isAdmin ? <Outlet /> : <Navigate to="/" replace />;
};

export default AdminRoute;
