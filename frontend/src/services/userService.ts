// services/userService.ts
//import axios from "axios"; // створений axios instance з базовим URL (Gateway)
import { api } from "../api";

export interface UserInfo {
  data: { email?: string | undefined; firstName?: string | undefined; lastName?: string | undefined; avatarUrl?: string | undefined; dateOfBirth?: string | undefined; };
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  avatarUrl?: string | null;
  dateOfBirth: string | null;
  phoneNumber?: string | null;
}

export const getUserInfo = async (): Promise<UserInfo> => {
  const response = await api.get<UserInfo>("/users/me");
  return response.data;
};
// export const updateUserProfile = async (userData: Partial<UserInfo>): Promise<UserInfo> => {
//   const response = await api.put<UserInfo>("/users/me", userData);
//   return response.data;
// };