import Person from "./components/person/Person"
import Navbar from "./components/Navbar"
import Home from "./pages/Home"
import About from "./pages/About"
import NotFound from "./pages/NotFound"
import { Routes, Route } from "react-router"
import { Toaster } from "react-hot-toast"
import Product from "./components/product/Product"
import Category from "./components/categories/Category"


const App = () => {
    return (
        <>
            <Navbar />
            <Routes>
                <Route index element={<Home /> } />
                <Route path="about" element={<About /> } />
                <Route path="person" element={<Person /> } />
                <Route path="product" element={<Product /> } />
                <Route path="category" element={<Category /> } />
                <Route path="*" element={<NotFound /> } />
            </Routes>

            <Toaster />
        </>
    )
}

export default App