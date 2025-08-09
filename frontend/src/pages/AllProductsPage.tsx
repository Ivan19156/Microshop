import {
  Accordion,
  AccordionSummary,
  AccordionDetails,
  Typography,
  Box,
  IconButton,
  Button,
  Paper,
} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import AddShoppingCartIcon from "@mui/icons-material/AddShoppingCart";
import { useEffect, useState } from "react";

import { useNavigate } from "react-router-dom";
import { api } from "../api";

interface Product {
  id: string;
  name: string;
  description: string;
  price: number;
  stock: number;
}

export default function AllProductsPage() {
  const [products, setProducts] = useState<Product[]>([]);
  const navigate = useNavigate();

  useEffect(() => {
    async function fetchProducts() {
      try {
        const response = await api.get<Product[]>("/products");
        setProducts(response.data);
      } catch (error) {
        console.error("Не вдалося отримати товари", error);
      }
    }

    fetchProducts();
  }, []);

  const handleOrder = (productId: string) => {
    navigate(`/orders/create/${productId}`);
  };

  return (
    <Box p={4}>
      <Typography variant="h4" gutterBottom>
        Всі товари
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
              <IconButton
                onClick={() => handleOrder(product.id)}
                sx={{ opacity: 0.7 }}
              >
                <AddShoppingCartIcon />
              </IconButton>
            </Box>
          </AccordionSummary>

          <AccordionDetails>
            <Paper sx={{ p: 2 }}>
              <Typography variant="body1" gutterBottom>
                {product.description}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Ціна: {product.price} ₴ | Кількість: {product.stock}
              </Typography>
              <Box mt={2}>
                <Button
                  variant="contained"
                  onClick={() => handleOrder(product.id)}
                >
                  Створити замовлення
                </Button>
              </Box>
            </Paper>
          </AccordionDetails>
        </Accordion>
      ))}
    </Box>
  );
}
