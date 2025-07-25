import { useState } from "react";
import {
  Container, Typography, TextField, Button, Box
} from "@mui/material";
import { toast } from "react-toastify";
import { api } from "../api";
import { useNavigate } from "react-router-dom"; // ✅ Import

export default function LoginPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate(); // ✅ Initialize navigate

  const handleLogin = async () => {
  try {
    const response = await api.post("/auth/api/auth/login", { email, password });

    const { accessToken, refreshToken } = response.data;
    localStorage.setItem("accessToken", accessToken);
    localStorage.setItem("refreshToken", refreshToken);

    toast.success("Успішний вхід!");
    navigate("/profile/edit");
  } catch (error) {
    toast.error("Помилка входу");
  }
};


  return (
    <Container maxWidth="xs">
      <Box mt={10}>
        <Typography variant="h4" gutterBottom align="center">Вхід</Typography>
        <TextField
          label="Email"
          fullWidth
          margin="normal"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
        <TextField
          label="Пароль"
          fullWidth
          margin="normal"
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
        <Button variant="contained" color="primary" fullWidth onClick={handleLogin}>
          Увійти
        </Button>
      </Box>
    </Container>
  );
}
