import { useEffect, useState } from "react";
import {
    Accordion,
    AccordionDetails,
  AccordionSummary,
  Box,
  Button,
  IconButton,
  Paper,
  Stack,
  TextField,
  Typography,
} from "@mui/material";
import { toast } from "react-toastify";
import { api } from "../api"; // шляхи адаптуй під себе
import EditIcon from "@mui/icons-material/Edit";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import DeleteIcon from "@mui/icons-material/Delete";

interface Product {
  id: string;
  name: string;
  description: string;
  price: number;
  stock: number;
}

export default function MyProductsPage() {
  const [products, setProducts] = useState<Product[]>([]);
  const [editProductId, setEditProductId] = useState<string | null>(null);
  const [editData, setEditData] = useState<Partial<Product>>({});

  const fetchProducts = async () => {
    try {
      const response = await api.get<Product[]>("/products/my");
      setProducts(response.data);
    } catch (error) {
      toast.error("Не вдалося завантажити товари");
    }
  };

  useEffect(() => {
    fetchProducts();
  }, []);

  const handleDelete = async (id: string) => {
    if (!window.confirm("Ви впевнені, що хочете видалити цей товар?")) return;
    try {
      await api.delete(`/products/${id}`);
      toast.success("Товар видалено");
      fetchProducts();
    } catch (error) {
      toast.error("Помилка при видаленні товару");
    }
  };

  const handleEditClick = (product: Product) => {
    setEditProductId(product.id);
    setEditData({
      name: product.name,
      description: product.description,
      price: product.price,
      stock: product.stock,
    });
  };

  const handleEditChange = (
    e: React.ChangeEvent<HTMLInputElement>
  ) => {
    const { name, value } = e.target;
    setEditData((prev) => ({
      ...prev,
      [name]: name === "price" || name === "stock" ? Number(value) : value,
    }));
  };

  const handleUpdate = async () => {
    if (!editProductId) return;

    try {
      await api.put(`/products/${editProductId}`, {dto :editData});
      toast.success("Товар оновлено");
      setEditProductId(null);
      setEditData({});
      fetchProducts();
    } catch (error) {
      toast.error("Помилка при оновленні товару");
    }
  };


return (
    <Box p={4}>
      <Typography variant="h4" gutterBottom>
        Мої товари
      </Typography>

      {products.map((product) => (
        <Accordion key={product.id}>
          <AccordionSummary expandIcon={<ExpandMoreIcon />}>
            <Box
              sx={{
                flexGrow: 1,
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center",
              }}
            >
              <Typography>{product.name}</Typography>
              <Box sx={{ opacity: 0.7 }}>
                <IconButton onClick={() => handleEditClick(product)}>
                  <EditIcon />
                </IconButton>
                <IconButton onClick={() => handleDelete(product.id)}>
                  <DeleteIcon />
                </IconButton>
              </Box>
            </Box>
          </AccordionSummary>

          <AccordionDetails>
            {editProductId === product.id ? (
              <Paper sx={{ p: 2 }}>
                <Stack spacing={2}>
                  <TextField
                    label="Назва"
                    name="name"
                    value={editData.name || ""}
                    onChange={handleEditChange}
                    fullWidth
                  />
                  <TextField
                    label="Опис"
                    name="description"
                    value={editData.description || ""}
                    onChange={handleEditChange}
                    multiline
                    minRows={3}
                    fullWidth
                  />
                  <TextField
                    label="Ціна"
                    name="price"
                    type="number"
                    value={editData.price || 0}
                    onChange={handleEditChange}
                    fullWidth
                  />
                  <TextField
                    label="Кількість"
                    name="stock"
                    type="number"
                    value={editData.stock || 0}
                    onChange={handleEditChange}
                    fullWidth
                  />

                  <Stack direction="row" spacing={2}>
                    <Button
                      variant="contained"
                      onClick={handleUpdate}
                      disabled={!editData.name || !editData.description}
                    >
                      Зберегти
                    </Button>
                    <Button
                      variant="outlined"
                      color="inherit"
                      onClick={() => setEditProductId(null)}
                    >
                      Скасувати
                    </Button>
                  </Stack>
                </Stack>
              </Paper>
            ) : (
              <Box>
                <Typography variant="body1">
                  {product.description}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Ціна: {product.price} ₴ | Кількість: {product.stock}
                </Typography>
              </Box>
            )}
          </AccordionDetails>
        </Accordion>
      ))}
    </Box>
  );
}
