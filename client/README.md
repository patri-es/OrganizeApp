# frontend for ProductCrudApp

## React + Vite

This template provides a minimal setup to get React working in Vite with HMR and some ESLint rules.

Currently, two official plugins are available:

- [@vitejs/plugin-react](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react) uses [Oxc](https://oxc.rs)
- [@vitejs/plugin-react-swc](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react-swc) uses [SWC](https://swc.rs/)

## React Compiler

The React Compiler is not enabled on this template because of its impact on dev & build performances. To add it, see [this documentation](https://react.dev/learn/react-compiler/installation).

## Expanding the ESLint configuration

If you are developing a production application, we recommend using TypeScript with type-aware lint rules enabled. Check out the [TS template](https://github.com/vitejs/vite/tree/main/packages/create-vite/template-react-ts) for information on how to integrate TypeScript and [`typescript-eslint`](https://typescript-eslint.io) in your project.

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

- Preparamos componentes para probar, con datos ficticios primero.
- Tenemos un Form para agregar personas y una lista que muestra las personas que hay. 
- Añadimos una nueva librería para conectar nuestro form con nuestra API

### Submit Form
React no es listo con las acciones de los form, así que vamos a añadir esta librería: 
- react hook form: https://react-hook-form.com/
- https://react-hook-form.com/get-started
- empezamos instalando la librería: ```npm install react-hook-form```
- En el form para empezar añadimos la librería y un método para utilizarla:

        import { useForm } from "react-hook-form"
        ....
       const {
            register,
            handleSubmit,
            formState: { errors },
          } = useForm()


- Preparo el formulario registrando el nombre y las características de cada campo:

      <input type="text"
            {...register("name", {
                required: true,
                maxLength: 50
            })}

- Definimos los errores 

        {errors.name?.type === 'required' && <p className="mt-1 text-sm text-red-600 flex items-center">
            name is  required
        </p>}

        {errors.name?.type === 'maxLength' && <p className="mt-1 text-sm text-red-600 flex items-center">
            name can not exceed 50 characters
        </p>}

- Añado otro método para los valores por defecto: 

        const defaultFormValues = {
            name: '',
            lastName: '',
            birthDate: '',
            telephone: ''
        }

- Pasamos todo al componente padre que es Person (de donde venían los datos)
    - la librería, los métodos para usarla y el de default.
    - ya no hace falta en el form, los borramos de ahí, y cambiams el registro:
        
            const {
                register,
                handleSubmit,
                formState: { errors },
            } = methods;

- Tenemos un método para guardar y otro para edit que lo único que hacen es pintar los datos en la consola por ahora

        const onFormSubmit = (data) => {
            console.log(data);
        }
- Los métodos en Person.jsx quedarían así (por ahora):

        const defaultFormValues = {
            name: '',
            lastName: '',
            birthDate: '',
            telephone: ''
        }

        const methods = useForm({
            defaultValues: defaultFormValues
        });


- Añado un método para el botón reset en el Person también: 

        const handleFormReset = () => {
            methods.reset(defaultFormValues);
        }

    en el form este botón quedaría así:

        <button
            type="button"
            onClick={onFormReset}
            className="ms-3 px-4 py-2.5 rounded-full text-sm font-semibold text-white bg-gradient-to-b from-blue-400 via-blue-600 to-blue-400  hover:from-teal-600 hover:to-blue-600 shadow-md hover:shadow-lg transition-all duration-300 transform hover:scale-105">
            <RotateCcw />
        </button>

## Adaptamos los métodos para usarlos desde el componente padre
... aunque no me funciona del todo, ya iré enterandome. 

- Instalamos otra librería, React Host Toast con ```npm install react-hot-toast ```
- parece que sirve para ventanas emergentes muy monas
- Lo añadimos a App.jsx con  ```import { Toaster } from "react-hot-toast" ``` 
- agregamos ahí también su componente  <Toaster />
- En person.jsx agregamos también la referencia para usarlo, con ```import toast from "react-hot-toast" ``` 
- Añado Try{ }catch (error) {} al método submit. o edit


## Conectamos la API al front para tener datos reales!
- instalamos axios ```npm i axios```