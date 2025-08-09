import { Routes, Route } from 'react-router-dom';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import HomePage from './pages/HomePage';
import UpdateProfilePage from './pages/UpdateProfilePage';
import UserDashboardPage from './pages/UserDashboardPage';
import AllProductsPage from './pages/AllProductsPage';
import CreateOrderPage from './pages/CreateOrderPage';
import MyProductsPage from './pages/MyProductPage';
import CreateProductPage from './pages/CreateProductPage';


export default function App() {
  return (
    <Routes>
      <Route path="/" element={<HomePage />} />
      <Route path="/login" element={<LoginPage />} />
      <Route path="/register" element={<RegisterPage />} />
      <Route path="/profile/edit" element={<UpdateProfilePage />} />
      <Route path="/dashboard" element={<UserDashboardPage />} />
      <Route path="/orders/create/:productId" element={<CreateOrderPage />} />
      <Route path="/products" element={<AllProductsPage />} />
      <Route path="/my-products" element={<MyProductsPage />} />
      <Route path="/products/create" element={<CreateProductPage />} />
      {/* <Route path="/my-orders" element={<MyOrdersPage />} /> */}
      
    </Routes>
  );
}
