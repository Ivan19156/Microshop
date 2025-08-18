import { useEffect, useState } from "react";
import {
  Container,
  Typography,
  TextField,
  Button,
  Box,
  CircularProgress,
} from "@mui/material";
import { toast } from "react-toastify";
import { api } from "../api";
import { useNavigate } from "react-router-dom";
import { getUserInfo } from "../services/userService";

export default function UpdateProfilePage() {
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);

  const [email, setEmail] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [avatarUrl, setAvatarUrl] = useState("");
  const [dateOfBirth, setDateOfBirth] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const profile = await getUserInfo();

        setEmail(profile.email || "");
        setFirstName(profile.firstName || "");
        setLastName(profile.lastName || "");
        setAvatarUrl(profile.avatarUrl || ""); // 👈 важливо
        setDateOfBirth(profile.dateOfBirth ? profile.dateOfBirth.slice(0, 10) : "");
        setPhoneNumber(profile.phoneNumber || "");
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
      console.log("Updating with:", { firstName, lastName, avatarUrl, dateOfBirth, phoneNumber });

      await api.put("auth/api/profile/profile", {
        firstName,
        lastName,
        avatarUrl,
        dateOfBirth,
        phoneNumber, 
      });

      toast.success("Профіль оновлено успішно!");
      navigate("/dashboard");
    } catch (error: any) {
      toast.error(error.response?.data?.message || "Помилка під час оновлення профілю");
    } finally {
      setSaving(false);
    }
  };

  const handleFileUpload = async (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (!file) return;

    const formData = new FormData();
    formData.append("file", file);

    try {
      const response = await api.post("/api/files/upload", formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });

      console.log("Upload response:", response.data);
      let url = response.data as string;
      console.log("Received uploaded avatar URL:", url);
      if (url.includes("azurite:10000")) {
  url = url.replace("azurite:10000", "localhost:10000");
}
      setAvatarUrl(url);

      console.log("Received uploaded avatar URL:", url);
      setAvatarUrl(url);

      toast.success("Аватар завантажено успішно!");
    } catch (error: any) {
      toast.error(error.response?.data?.message || "Помилка завантаження аватара");
    }
  };
// const handleFileUpload = async (event: React.ChangeEvent<HTMLInputElement>) => {
//   const file = event.target.files?.[0];
//   if (!file) return;

//   const formData = new FormData();
//   formData.append("file", file);

//   try {
//     const response = await api.post("/api/files/upload", formData, {
//       headers: { "Content-Type": "multipart/form-data" },
//     });

//     let url = response.data as string;
//     if (url.includes("azurite:10000")) {
//       url = url.replace("azurite:10000", "localhost:10000");
//     }

//     // Додаємо унікальний параметр для кешу
//     setAvatarUrl(url + "?t=" + new Date().getTime());

//     toast.success("Аватар завантажено успішно!");
//   } catch (error: any) {
//     toast.error(error.response?.data?.message || "Помилка завантаження аватара");
//   }
// };


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

        <Box mt={2} display="flex" justifyContent="center">
          {avatarUrl ? (
            <img
              src={avatarUrl}
              alt="Avatar preview"
              style={{
                width: 150,
                height: 150,
                borderRadius: "50%",
                objectFit: "cover",
                border: "1px solid #ccc",
              }}
            />
          ) : (
            <Box
              width={150}
              height={150}
              borderRadius="50%"
              bgcolor="#e0e0e0"
              display="flex"
              alignItems="center"
              justifyContent="center"
              fontSize={32}
              color="#888"
              border="1px dashed #aaa"
            >
              ?
            </Box>
          )}
        </Box>

        <Box mt={2} display="flex" justifyContent="center">
          <Button variant="outlined" component="label">
            Завантажити аватар
            <input type="file" hidden accept="image/*" onChange={handleFileUpload} />
          </Button>
        </Box>

        <TextField label="Email" value={email} fullWidth margin="normal" disabled />
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
        <TextField
          label="Дата народження"
          type="date"
          value={dateOfBirth}
          onChange={(e) => setDateOfBirth(e.target.value)}
          fullWidth
          margin="normal"
          InputLabelProps={{ shrink: true }}
        />
        <TextField
          label="Номер телефону"
          value={phoneNumber}
          onChange={(e) => setPhoneNumber(e.target.value)}
          fullWidth
          margin="normal"
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
