import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/auth/api/auth': {
        target: 'http://localhost:7000',
        changeOrigin: true,
        secure: false,
      },
    },
  },
});
