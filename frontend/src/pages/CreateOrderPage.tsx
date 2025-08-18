import {
  Box,
  Typography,
  TextField,
  Button,
  CircularProgress,
  Stack,
  Paper,
} from "@mui/material";
import { useParams, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";

import { toast } from "react-toastify";
import { api } from "../api";

interface Product {
  id: string;
  name: string;
  description: string;
  price: number;
  stock: number;
}

export default function CreateOrderPage() {
  const { productId } = useParams();
  const navigate = useNavigate();

  const [product, setProduct] = useState<Product | null>(null);
  const [quantity, setQuantity] = useState(1);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function fetchProduct() {
      try {
        const response = await api.get<Product>(`/products/${productId}`);
        setProduct(response.data);
      } catch (error) {
        toast.error("Не вдалося завантажити товар");
      } finally {
        setLoading(false);
      }
    }

    if (productId) fetchProduct();
  }, [productId]);

  const handleOrderSubmit = async () => {
    if (!product || quantity < 1) return;

    try {
      await api.post("/orders", {
        productId: product.id,
        quantity,
      });
      toast.success("Замовлення створено!");
      navigate("/dashboard");
    } catch (error) {
      toast.error("Не вдалося створити замовлення");
    }
  };

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" mt={10}>
        <CircularProgress />
      </Box>
    );
  }

  if (!product) {
    return (
      <Box mt={4}>
        <Typography color="error">Товар не знайдено.</Typography>
      </Box>
    );
  }

  const total = (product.price * quantity).toFixed(2);

  return (
    <Box maxWidth="sm" mx="auto" mt={4}>
      <Paper sx={{ p: 4 }}>
        <Typography variant="h5" gutterBottom>
          Оформлення замовлення
        </Typography>

        <Typography variant="h6" gutterBottom>
          {product.name}
        </Typography>

        <Typography gutterBottom color="text.secondary">
          {product.description}
        </Typography>

        <Typography gutterBottom>Ціна за одиницю: {product.price} ₴</Typography>

        <TextField
          label="Кількість"
          type="number"
          value={quantity}
          onChange={(e) => setQuantity(Math.max(1, Number(e.target.value)))}
          inputProps={{ min: 1, max: product.stock }}
          fullWidth
          sx={{ mt: 2 }}
        />

        <Typography variant="h6" mt={2}>
          Всього: {total} ₴
        </Typography>

        <Stack direction="row" spacing={2} mt={3}>
          <Button
            variant="contained"
            onClick={handleOrderSubmit}
            disabled={quantity < 1 || quantity > product.stock}
          >
            Підтвердити замовлення
          </Button>

          <Button variant="outlined" onClick={() => navigate(-1)}>
            Назад
          </Button>
        </Stack>
      </Paper>
    </Box>
  );
}
