import { useState } from "react";
import {
  Box,
  Button,
  TextField,
  Typography,
  Stack,
  Paper,
} from "@mui/material";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

import { api } from "../api";

// DTO інтерфейс
interface CreateProductDto {
  name: string;
  description: string;
  price: number;
  stock: number;
}

export default function CreateProductPage() {
  const navigate = useNavigate();

  const [formData, setFormData] = useState<CreateProductDto>({
    name: "",
    description: "",
    price: 0,
    stock: 0,
  });

  const [loading, setLoading] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;

    setFormData((prev) => ({
      ...prev,
      [name]: name === "price" || name === "stock" ? Number(value) : value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);

    try {
      await api.post("/products", { dto: formData }); // або через свій API service
      toast.success("Товар успішно створено!");
      navigate("/my-products");
    } catch (error: any) {
      toast.error(
        error.response?.data?.message || "Помилка під час створення товару"
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box maxWidth="md" mx="auto" mt={5}>
      <Paper sx={{ p: 4 }}>
        <Typography variant="h5" mb={3}>
          Створення товару
        </Typography>

        <form onSubmit={handleSubmit}>
          <Stack spacing={2}>
            <TextField
              label="Назва товару"
              name="name"
              value={formData.name}
              onChange={handleChange}
              required
              fullWidth
            />

            <TextField
              label="Опис"
              name="description"
              value={formData.description}
              onChange={handleChange}
              required
              fullWidth
              multiline
              minRows={3}
            />

            <TextField
              label="Ціна"
              name="price"
              type="number"
              value={formData.price}
              onChange={handleChange}
              required
              inputProps={{ min: 0, step: "0.01" }}
              fullWidth
            />

            <TextField
              label="Кількість на складі"
              name="stock"
              type="number"
              value={formData.stock}
              onChange={handleChange}
              required
              inputProps={{ min: 0 }}
              fullWidth
            />

            <Button
              type="submit"
              variant="contained"
              color="primary"
              disabled={loading}
            >
              {loading ? "Завантаження..." : "Опублікувати товар"}
            </Button>
          </Stack>
        </form>
      </Paper>
    </Box>
  );
}
