// services/orderService.ts
import { api } from "../api";

export interface UserOrder {
  id: string;
  createdAt: string;
  status: string;
  totalPrice: number;
}

export const getUserOrders = async (): Promise<UserOrder[]> => {
  const response = await api.get<UserOrder[]>("/api/order/my-orders");
  return response.data;
};
