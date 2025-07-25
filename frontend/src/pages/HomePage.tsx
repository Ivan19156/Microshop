import { Container, Typography, Box, Button, Stack } from "@mui/material";
import { useNavigate } from "react-router-dom";

export default function HomePage() {
  const navigate = useNavigate();

  return (
    <Box
      sx={{
        minHeight: "100vh",       // висота вікна браузера
        bgcolor: "#fff",          // білий фон
        display: "flex",
        justifyContent: "center", // горизонтальне центрування
        alignItems: "center",     // вертикальне центрування
        p: 3,
      }}
    >
      <Container maxWidth="xs" sx={{ textAlign: "center" }}>
        <Typography variant="h4" gutterBottom>
          Вітаємо!
        </Typography>

        <Stack spacing={3} direction="row" justifyContent="center" mt={4}>
          <Button variant="contained" onClick={() => navigate("/login")}>
            Логін
          </Button>
          <Button variant="outlined" onClick={() => navigate("/register")}>
            Реєстрація
          </Button>
        </Stack>
      </Container>
    </Box>
  );
}