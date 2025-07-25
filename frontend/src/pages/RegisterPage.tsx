import { useState } from "react";
import {
  Container, Typography, TextField, Button, Box
} from "@mui/material";
import { toast } from "react-toastify";
import { api } from "../api";

export default function RegisterPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");

  const handleRegister = async () => {
    if (password !== confirmPassword) {
      toast.error("Паролі не співпадають");
      return;
    }

    try {
      await api.post("/auth/api/auth/register", { email, password, confirmPassword });
      toast.success("Реєстрація успішна!");
    } catch (error: any) {
      toast.error(error.response?.data?.message || "Помилка реєстрації");
    }
  };

  return (
    <Container maxWidth="xs">
      <Box mt={10}>
        <Typography variant="h4" gutterBottom align="center">Реєстрація</Typography>
        
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

        <TextField
          label="Підтвердіть пароль"
          fullWidth
          margin="normal"
          type="password"
          value={confirmPassword}
          onChange={(e) => setConfirmPassword(e.target.value)}
        />

        <Button variant="contained" color="primary" fullWidth onClick={handleRegister}>
          Зареєструватися
        </Button>
      </Box>
    </Container>
  );
}
