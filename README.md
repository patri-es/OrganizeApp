# Organize App
Creating WebApp with React and .NET for the backendm, usign API with controllers and EF to manage the SQL Server Data Base.


**Revisar mañana: https://learn.microsoft.com/es-es/visualstudio/javascript/create-react-app?view=visualstudio**

## Install Tailwind Css
- isntalo tailwindcss para los estilos
- lucide react para los iconos.

	npm install tailwindcss @tailwindcss/vite
	npm install lucide-react

- después añadimos en el index.css la linea ```@import "tailwindcss";```

## Router para React
instalamos con la siguiente linea: 
```
npm i react-router-dom
``` 
Tenemos 3 formas de utilizar router: 
1. Declarative (la más básica)
2. Data (añade más features)
3. Framework (mas y mas)

Usaremos la declarativa. Instalando el paquete, sólo faltará añadir el componente en main.jsx

```
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter } from 'react-router'
import './index.css'
import App from './App.jsx'

createRoot(document.getElementById('root')).render(
    <StrictMode>
        <BrowserRouter>
            <App />
        </BrowserRouter> 
  </StrictMode>
)

```