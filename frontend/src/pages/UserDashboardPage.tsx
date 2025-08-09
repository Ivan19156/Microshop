import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import {
  Box,
  Typography,
  Button,
  Avatar,
  Stack,
  Divider,
  Card,
  CardContent,
  CircularProgress,
} from "@mui/material";

import { getUserInfo } from "../services/userService";
import { getUserOrders } from "../services/orderService";

// Тип для користувача
interface User {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  avatarUrl?: string;
}

// Тип для замовлення
interface Order {
  id: string;
  createdAt: string;
  totalPrice: number;
  status: string;
}

export default function UserDashboardPage() {
  const navigate = useNavigate();
  const [user, setUser] = useState<User | null>(null);
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function fetchData() {
      try {
        const userData = await getUserInfo();

        // Нормалізація URL аватарки
        const normalizedImageUrl = userData.avatarUrl?.includes("azurite:10000")
          ? userData.avatarUrl.replace("azurite:10000", "localhost:10000")
          : userData.avatarUrl;

        setUser({ ...userData, avatarUrl: normalizedImageUrl ?? undefined });
      } catch (error) {
        console.error("Помилка отримання користувача", error);
      }

      try {
        const userOrders = await getUserOrders();
        setOrders(userOrders);
      } catch (error) {
        console.error("Помилка отримання замовлень", error);
      } finally {
        setLoading(false);
      }
    }

    fetchData();
  }, []);

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" mt={10}>
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Box p={4}>
      <Box
        sx={{
          display: "flex",
          flexDirection: { xs: "column", md: "row" },
          gap: 4,
        }}
      >
        {/* Ліва панель профілю */}
        <Box
          sx={{
            width: { xs: "100%", md: "25%" },
            minWidth: 0,
            mb: { xs: 4, md: 0 },
          }}
          textAlign="center"
        >
          <Avatar
            src={user?.avatarUrl || undefined}
            alt="Avatar"
            sx={{
              width: 150,
              height: 150,
              border: "1px solid #ccc",
              objectFit: "cover",
              mx: "auto",
            }}
          >
            {!user?.avatarUrl &&
              `${user?.firstName?.[0] ?? ""}${user?.lastName?.[0] ?? ""}`}
          </Avatar>

          <Typography variant="h6" mt={2}>
            {user?.lastName} {user?.firstName}
          </Typography>

          <Stack spacing={1} mt={2}>
            <Button
              variant="outlined"
              fullWidth
              onClick={() => navigate("/profile/edit")}
            >
              Редагувати профіль
            </Button>

            <Button
              variant="outlined"
              fullWidth
              onClick={() => navigate("/my-products")}
            >
              Переглянути мої товари
            </Button>

            <Button
              variant="contained"
              fullWidth
              onClick={() => navigate("/products/create")}
            >
              Опублікувати товар
            </Button>
          </Stack>
        </Box>

        {/* Права панель із замовленнями */}
        <Box sx={{ flex: 1, minWidth: 0 }}>
          <Box
            mb={2}
            display="flex"
            justifyContent="space-between"
            alignItems="center"
          >
            <Typography variant="h5">Мої замовлення</Typography>
            <Button variant="contained" onClick={() => navigate("/products")}>
              Переглянути товари
            </Button>
          </Box>
          <Divider sx={{ mb: 2 }} />

          <Stack spacing={2}>
            {orders.length === 0 ? (
              <Typography color="text.secondary">
                У вас поки немає замовлень.
              </Typography>
            ) : (
              orders.map((order) => (
                <Card key={order.id} variant="outlined">
                  <CardContent>
                    <Typography variant="subtitle1">
                      Замовлення № {order.id}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      Дата: {new Date(order.createdAt).toLocaleString()} | Сума:{" "}
                      {order.totalPrice} ₴ | Статус: {order.status}
                    </Typography>
                  </CardContent>
                </Card>
              ))
            )}
          </Stack>
        </Box>
      </Box>
    </Box>
  );
}