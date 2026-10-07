import { fileURLToPath, URL } from 'node:url'
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url)),
    },
  },
  server: {
    port: 5173,
    proxy: {
      '/hubs': {
        target: 'https://localhost:7145',
        changeOrigin: true,
        secure: false,
        ws: true,
      },
      '/api': {
        target: 'https://localhost:7145',
        changeOrigin: true,
        secure: false,
      },
    },
  },
})
