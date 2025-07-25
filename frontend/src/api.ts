import axios from "axios";

const API_URL = import.meta.env.VITE_API_URL || "http://localhost:7000";

export const api = axios.create({
  baseURL: API_URL,
});

api.interceptors.request.use(config => {
  const token = localStorage.getItem("accessToken");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

api.interceptors.response.use(
  response => response,
  async error => {
    const originalRequest = error.config;

    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      const refreshToken = localStorage.getItem("refreshToken");
      if (!refreshToken) {
        // Наприклад, редірект на логін або інша логіка
        return Promise.reject(error);
      }

      try {
        const response = await axios.post(`${API_URL}/auth/api/auth/refresh-token`, { refreshToken });
        const { accessToken: newAccessToken, refreshToken: newRefreshToken } = response.data;

        localStorage.setItem("accessToken", newAccessToken);
        localStorage.setItem("refreshToken", newRefreshToken);

        // Оновлюємо заголовок і повторюємо початковий запит
        originalRequest.headers.Authorization = `Bearer ${newAccessToken}`;
        return api(originalRequest);
      } catch (refreshError) {
        // Якщо оновлення не вдалось — наприклад, чистимо токени і редірект
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
        // redirect to login page (залежить від твоєї логіки)
        return Promise.reject(refreshError);
      }
    }
    return Promise.reject(error);
  }
);

