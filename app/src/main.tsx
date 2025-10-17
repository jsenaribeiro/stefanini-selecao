import App from './App'
import React from 'react'
import ReactDOM from 'react-dom/client'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import './assets/main.css'
import './assets/dev.css'
import './extensions'

const queryClient = new QueryClient()

const rootHTML = document.getElementById('root')

ReactDOM.createRoot(rootHTML!).render(
  <React.StrictMode>
    <QueryClientProvider client={queryClient}>
      <App />
    </QueryClientProvider>
  </React.StrictMode>
);