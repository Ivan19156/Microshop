import { useEffect, useState } from "react";
import {
  Container,
  Typography,
  TextField,
  Button,
  Box,
  CircularProgress
} from "@mui/material";
import { toast } from "react-toastify";
import { api } from "../api"; // Your Axios instance with base URL and JWT interceptor

export default function UpdateProfilePage() {
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);

  const [email, setEmail] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  //const [avatarUrl, setAvatarUrl] = useState("");
  const [dateOfBirth, setDateOfBirth] = useState("");

  // Fetch profile on component mount
  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const response = await api.get("auth/api/profile/profile");
        const profile = response.data;

        setEmail(profile.email || "");
        setFirstName(profile.firstName || "");
        setLastName(profile.lastName || "");
        //setAvatarUrl(profile.avatarUrl || "");
        setDateOfBirth(profile.dateOfBirth ? profile.dateOfBirth.slice(0, 10) : "");
      } catch (error: any) {
        toast.error(error.response?.data?.message || "Не вдалося завантажити профіль");
      } finally {
        setLoading(false);
      }
    };

    fetchProfile();
  }, []);

  const handleSave = async () => {
    setSaving(true);
    try {
      await api.put("auth/api/profile/profile", {
        firstName,
        lastName,
        //avatarUrl,
        dateOfBirth,
      });
      toast.success("Профіль оновлено успішно!");
    } catch (error: any) {
      toast.error(error.response?.data?.message || "Помилка під час оновлення профілю");
    } finally {
      setSaving(false);
    }
  };

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" mt={10}>
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Container maxWidth="sm">
      <Box mt={5}>
        <Typography variant="h5" gutterBottom align="center">
          Редагувати профіль
        </Typography>

        <TextField
          label="Email"
          value={email}
          fullWidth
          margin="normal"
          disabled
        />

        <TextField
          label="Ім’я"
          value={firstName}
          onChange={(e) => setFirstName(e.target.value)}
          fullWidth
          margin="normal"
        />

        <TextField
          label="Прізвище"
          value={lastName}
          onChange={(e) => setLastName(e.target.value)}
          fullWidth
          margin="normal"
        />

        {/* <TextField
          label="Аватар (URL)"
          value={avatarUrl}
          onChange={(e) => setAvatarUrl(e.target.value)}
          fullWidth
          margin="normal"
        /> */}

        <TextField
          label="Дата народження"
          type="date"
          value={dateOfBirth}
          onChange={(e) => setDateOfBirth(e.target.value)}
          fullWidth
          margin="normal"
          InputLabelProps={{ shrink: true }}
        />

        <Button
          variant="contained"
          color="primary"
          onClick={handleSave}
          fullWidth
          disabled={saving}
          sx={{ mt: 2 }}
        >
          {saving ? "Збереження..." : "Зберегти зміни"}
        </Button>
      </Box>
    </Container>
  );
}
