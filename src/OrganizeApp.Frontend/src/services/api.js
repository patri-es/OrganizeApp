// interceptor de Axios
// este documento será el que defina cómo el Front se comunica con la API del back

import axios from "axios"

const api = axios.create({
    baseURL: import.meta.env.VITE_BASE_API_URL, //     baseURL: "https://localhost:7049/api",
    headers: {
        "Content-Type": "application/json"
    },
    timeout: 10000
})
// Request interceptor
api.interceptors.request.use(
    (config) => {
        // La autenticación se añadirá aquí posteriormente. 
        // Por ejemplo: config.headers.Authorization = `Bearer ${token}`
        return config
    },
    (error) => {
        return Promise.reject(error)
    }
)

// Response interceptor
api.interceptors.response.use(
    (response) => {
        return response
    },
    (error) => {
        return Promise.reject(error)
    }
)

export default api