import {defineConfig} from 'vite'
import basicSsl from '@vitejs/plugin-basic-ssl';
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [
        react(),
        basicSsl()
    ],
    build: {
        outDir: 'build',
    },
    resolve: {
        alias: {
            '@': '/src',
        },
    },
    optimizeDeps: {
        exclude: ['js-big-decimal']
    },
    server: {
        host: 'localhost',
        port: 44402,
        strictPort: true,
        proxy: {
            '/api': {
                target: 'https://localhost:7022/',
                changeOrigin: true,
                secure: false
            }
        }
    },
})
